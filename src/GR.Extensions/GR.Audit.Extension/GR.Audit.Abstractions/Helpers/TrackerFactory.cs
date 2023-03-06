using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using GR.Audit.Abstractions.Attributes;
using GR.Audit.Abstractions.Enums;
using GR.Audit.Abstractions.Models;
using GR.Core;
using GR.Core.Abstractions;
using GR.Core.Extensions;
using GR.Core.Helpers;

namespace GR.Audit.Abstractions.Helpers
{
    public static class TrackerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private static IHttpContextAccessor ContextAccessor => IoC.Resolve<IHttpContextAccessor>();

        /// <summary>
        /// Adding values to BaseModel fields
        /// </summary>
        /// <param name="context"></param>
        public static void Track(DbContext context)
        {
            try
            {
                if (ContextAccessor == null) return;

                var currentUsername = ContextAccessor?.HttpContext?.User?.Identity?.Name ?? GlobalResources.Roles.ANONIMOUS_USER;
                var tenantId = ContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "tenant")?.Value.ToGuid();

                var entities = context.ChangeTracker
                    .Entries()
                    .Where(x =>
                        x.Entity is IBaseModel
                        && !(x.Entity is TrackAudit)
                        && !(x.Entity is TrackAuditDetails)
                        && (x.State == EntityState.Modified || x.State == EntityState.Added))
                    .ToList();

                if (!entities.Any()) return;

                foreach (var entity in entities)
                {
                    if (!(entity.Entity is IBaseModel model)) continue;
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            model.Created = DateTime.UtcNow;
                            model.Author = currentUsername;
                            model.ModifiedBy = currentUsername;
                            model.TenantId = model.TenantId ?? tenantId;
                            model.Version = 1;
                            break;
                        case EntityState.Modified:
                            model.Changed = DateTime.UtcNow;
                            model.ModifiedBy = currentUsername;
                            if (model.TenantId == Guid.Empty) model.TenantId = tenantId;
                            if (model.Version > 1)
                                ++model.Version;
                            else model.Version = 1;
                            break;
                    }
                }

                var audit = entities.Where(x => x.State != EntityState.Added).Select(x => Audit(x, context))
                    .Where(x => x.IsSuccess && x.Result.Item1.AuditDetailses.Count > 0)
                    .Select(x => x.Result.Item1)
                    .ToList();
                context.AddRange(audit);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Check and get track able entity
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        public static ResultModel<(TrackAudit, object)> Audit(EntityEntry eventArgs, DbContext context)
        {
            var response = new ResultModel<(TrackAudit, object)>();
            //If is trackable entity return
            if (eventArgs.Entity is TrackAudit || eventArgs.Entity is TrackAuditDetails) return response;

            if (!(eventArgs.Entity is BaseModel baseModel)) return response;

            var (isTrackable, trackOption) = IsTrackable(eventArgs.Entity.GetType());

            if (!isTrackable) return response;
            try
            {
                dynamic entry = eventArgs.Entity;
                var contextName = eventArgs.Context.GetType().FullName;
                var audit = new TrackAudit
                {
                    TypeFullName = eventArgs.Entity.GetType().FullName,
                    Author = nameof(System),
                    ModifiedBy = nameof(System),
                    Version = baseModel.Version,
                    TrackEventType = GetRecordState(eventArgs.State),
                    DatabaseContextName = contextName,
                    RecordId = baseModel.Id,
                    UserName = baseModel.ModifiedBy,
                    TenantId = baseModel.TenantId
                };

                var auditDetails = new List<TrackAuditDetails>();
                var properties = context.Entry(eventArgs.Entity).Properties;

                if (trackOption.Equals(TrackEntityOption.SelectedFields))
                {
                    auditDetails.AddRange(eventArgs.Entity.GetType().GetProperties().Where(x => IsFieldTrackable(x, properties.FirstOrDefault(p => p.Metadata.Name == x.Name)))
                            .Select(x => new TrackAuditDetails
                            {
                                ModifiedBy = audit.UserName,
                                PropertyName = x.Name,
                                PropertyType = x.PropertyType.FullName,
                                Value = x.GetValue(eventArgs.Entity)?.ToString()
                            }));
                    /*if (eventArgs.State == EntityState.Modified)
                    {
                        auditDetails.AddRange(eventArgs.Entity.GetType().GetProperties().Where(IsFieldTrackable)
                        .Select(x => new TrackAuditDetails
                        {
                            ModifiedBy = audit.UserName,
                            PropertyName = x.Name,
                            PropertyType = x.PropertyType.FullName,
                            Value = x.GetValue(eventArgs.Entity)?.ToString()
                        }));
                    }
                    else
                    {
                        
                    }*/
                }
                else
                {
                    auditDetails.AddRange(eventArgs.Entity.GetType().GetProperties().Where(IsFieldNotIgnored)
                        .Select(x => new TrackAuditDetails
                        {
                            ModifiedBy = audit.UserName,
                            PropertyName = x.Name,
                            PropertyType = x.PropertyType.FullName,
                            Value = x.GetValue(eventArgs.Entity)?.ToString()
                        }));
                }

                audit.AuditDetailses = auditDetails;
                response.IsSuccess = true;
                response.Result = (audit, entry);
                return response;
            }
            catch (Exception e)
            {
                response.Errors.Add(new ErrorModel(nameof(Exception), e.Message));
                response.Errors.Add(new ErrorModel(nameof(Exception), e.InnerException?.ToString()));
            }

            return response;
        }

        /// <summary>
        /// Get record state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static TrackEventType GetRecordState(EntityState state)
        {
            var recState = TrackEventType.Updated;
            switch (state)
            {
                case EntityState.Detached:
                    break;

                case EntityState.Unchanged:
                    break;

                case EntityState.Deleted:
                    recState = TrackEventType.PermanentDeleted;
                    break;

                case EntityState.Modified:
                    recState = TrackEventType.Updated;
                    break;

                case EntityState.Added:
                    recState = TrackEventType.Added;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            return recState;
        }

        /// <summary>
        /// Is entity track able
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static (bool, TrackEntityOption) IsTrackable(MemberInfo type)
        {
            var attrs = Attribute.GetCustomAttributes(type);
            foreach (var attr in attrs)
            {
                if (!(attr is TrackEntity bind)) continue;
                if (!bind.Option.Equals(TrackEntityOption.Ignore))
                {
                    return (true, bind.Option);
                }

                break;
            }

            return (default, default);
        }

        /// <summary>
        /// Is property track able
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsFieldTrackable(PropertyInfo property, PropertyEntry entry)
        {
            if (entry == null || (entry.CurrentValue == null && entry.OriginalValue == null)) return false;
            if (entry.CurrentValue == null || entry.OriginalValue == null) goto Attributes;
            else if (entry.CurrentValue.ToString() == entry.OriginalValue.ToString()) return false;
            goto Attributes;
        Attributes:
            var attrs = property.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                if (!(attr is TrackField bind)) continue;
                if (bind.Option.Equals(TrackFieldOption.Allow))
                {
                    return true;
                }

                break;
            }

            return false;
        }

        private static bool IsFieldTrackable(PropertyInfo property)
        {
            var attrs = property.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                if (!(attr is TrackField bind)) continue;
                if (bind.Option.Equals(TrackFieldOption.Allow))
                {
                    return true;
                }

                break;
            }

            return false;
        }

        /// <summary>
        /// Check if field is ignored
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static bool IsFieldNotIgnored(PropertyInfo property)
        {
            var attrs = property.GetCustomAttributes(true);
            foreach (var attr in attrs)
            {
                if (!(attr is TrackField bind)) continue;
                if (bind.Option.Equals(TrackFieldOption.Ignore))
                {
                    return false;
                }

                break;
            }

            return true;
        }
    }
}
