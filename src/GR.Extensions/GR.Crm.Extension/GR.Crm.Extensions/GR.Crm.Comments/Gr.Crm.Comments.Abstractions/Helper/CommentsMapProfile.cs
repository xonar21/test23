using AutoMapper;
using Gr.Crm.Comments.Abstractions.Models;
using Gr.Crm.Comments.Abstractions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gr.Crm.Comments.Abstractions.Helper
{
    public class CommentsMapProfile : Profile
    {
        public CommentsMapProfile()
        {
            CreateMap<Comment, AddCommentViewModel>()
                .ForMember(o => o.Message, m => m.MapFrom(x => x.Message))
                .ForMember(o => o.LeadId, m => m.MapFrom(x => x.LeadId))
                .ForMember(o => o.OrganizationId, m => m.MapFrom(x => x.OrganizationId))
                .ForMember(o => o.CommentId, m => m.MapFrom(x => x.CommentId))
                .ReverseMap();

            CreateMap<Comment, CommentViewModel>()
                .IncludeAllDerived()
                .ReverseMap();  
        }
    }
}
