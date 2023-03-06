using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Gr.Crm.Products.Abstractions;
using Gr.Crm.Products.Abstractions.Models;
using GR.Core.Extensions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions;
using GR.Crm.Leads.Abstractions;
using GR.Crm.Leads.Abstractions.Models;
using GR.Crm.Products.Abstractions;
using GR.Crm.Products.Abstractions.Enums;
using GR.Crm.Products.Abstractions.Models;
using GR.Crm.Products.Abstractions.ViewModels.ProductsViewModels;
using Mapster;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GR.Crm.Products.Infrastructure
{
    public class ProductService : IProductService
    {

        #region Injectable

        /// <summary>
        /// Inject context
        /// </summary>
        private readonly ILeadContext<Lead> _context;

        /// <summary>
        /// Inject mapper
        /// </summary>
        private readonly IMapper _mapper;

        private readonly ICrmService _crmService;

        #endregion


        public ProductService(ILeadContext<Lead> context,
            IMapper mapper,
            IConfiguration configuration,
            ICrmService crmService)
        {
            _context = context;
            _mapper = mapper;
            GlobalCurrency = configuration.GetSection("GeneralConfigurations").GetSection("CurrencyCode").Value;
            _crmService = crmService;
            ConversionRate = _crmService.ConvertCurrencyToDefaultCurrencyAsync("EUR");
        }


        private string GlobalCurrency { get; set; }

        private Task<decimal> ConversionRate { get; set; }
        /// <summary>
        /// Add product 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<Guid>> AddProductAsync(AddProductViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var product = _mapper.Map<ProductTemplate>(model);
            product.Category = null;
            product.ProductManufactories = null;
            product.Currency = null;
            product.ProductCode = model.Name;
            _context.Products.Add(product);

            var result = await _context.PushAsync();

            return new ResultModel<Guid> { IsSuccess = result.IsSuccess, Errors = result.Errors, Result = product.Id };
        }

        /// <summary>
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> UpdateProductAsync(AddProductViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();


            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.Id.Equals(model.Id));

            if (product == null)
                return new NotFoundResultModel();


            product.Name = model.Name;
            product.Type = model.Type;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.Warranty = model.Warranty;
            product.CanBeSold = model.CanBeSold;
            product.EAN = model.EAN;
            product.ProductManufactoriesId = model.ProductManufactoriesId;
            product.SellingPrice = Math.Round(model.SellingPrice  / ConversionRate.Result, 2);
            product.CurrencyCode = model.CurrencyCode;

            _context.Products.Update(product);
            return await _context.PushAsync();
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<GetProductViewModel>> GetProductByIdAsync(Guid? productId)
        {
            if (productId == null)
                return new InvalidParametersResultModel<GetProductViewModel>();

            var product = await _context.Products
                .Include(x => x.ProductManufactories)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id.Equals(productId));

            if (product == null)
                return new NotFoundResultModel<GetProductViewModel>();
            if (!GlobalCurrency.IsNullOrEmpty())
            {
                product.SellingPrice = Math.Round(product.SellingPrice * ConversionRate.Result, 2);
                product.CurrencyCode = GlobalCurrency;
            }

            return new SuccessResultModel<GetProductViewModel>(_mapper.Map<GetProductViewModel>(product));

        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetProductViewModel>>> GetProductsByProductTypeAsync(TypeOfProduct type, bool includeDeleted)
        {
            var products = await _context.Products
                .Where(x => x.Type == type && (!x.IsDeleted || includeDeleted))
                .ToListAsync();

            return new SuccessResultModel<IEnumerable<GetProductViewModel>>(_mapper.Map<IEnumerable<GetProductViewModel>>(products));

        }


        /// <summary>
        /// Get all product
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<IEnumerable<GetProductViewModel>>> GetAllProductsAsync(
            bool includeDeleted = false)
       {
            var products = await _context.Products
                .Include(i => i.Category)
                .Where(x => !x.IsDeleted || includeDeleted)
                .ToListAsync();

            if (!GlobalCurrency.IsNullOrEmpty())
            {
                products.ForEach(product =>
                {
                    product.SellingPrice = Math.Round(product.SellingPrice * ConversionRate.Result, 2);
                    product.CurrencyCode = GlobalCurrency;
                });
            }

            return new SuccessResultModel<IEnumerable<GetProductViewModel>>(_mapper.Map<IEnumerable<GetProductViewModel>>(products));
        }

        /// <summary>
        /// Get all paginated product
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel<PagedResult<GetProductViewModel>>> GetAllPaginatedProductsAsync(PageRequest request)
        {
            var pagedResult = await _context.Products
                .Include(i => i.Category)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .Select(s => new GetProductViewModel
                {
                    Id = s.Id,
                    CategoryName = s.Category.Name,
                    Name = s.Name,
                    ProductCode = s.ProductCode,
                    Description = s.Description,
                    EAN = s.EAN,
                    Warranty = s.Warranty,
                    CanBeSold = s.CanBeSold,
                    SellingPrice = s.SellingPrice,
                    CurrencyCode = s.CurrencyCode
                })
                .GetPagedAsync(request);

            var map = pagedResult.Result; 
            if (!GlobalCurrency.IsNullOrEmpty())
            {
                foreach (var product in map)
                {
                    product.SellingPrice = Math.Round(product.SellingPrice * ConversionRate.Result, 2);
                    product.CurrencyCode = GlobalCurrency;
                };
            }
            return new SuccessResultModel<PagedResult<GetProductViewModel>>(pagedResult.Map(map));
        }




        /// <summary>
        /// Disable product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DisableProductAsync(Guid? productId)
        {
            var result = await _context.DisableRecordAsync<ProductTemplate>(productId);

            return result;
        }
            

        /// <summary>
        /// Activate product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> ActivateProductAsync(Guid? productId) =>
            await _context.ActivateRecordAsync<ProductTemplate>(productId);

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public virtual async Task<ResultModel> DeleteProductAsync(Guid? productId) =>
            await _context.RemovePermanentRecordAsync<ProductTemplate>(productId);

        public async Task<ResultModel<Guid>> AddProductVariationAsync(AddProductVariationViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var productVariation = _mapper.Map<ProductVariation>(model);

            _context.ProductVariations.Add(productVariation);
            var result = await _context.PushAsync();

            if (!result.IsSuccess) return new ResultModel<Guid> { Errors = result.Errors, IsSuccess = false };

            if (model.SelectedDeliverablesIds != null)
            {
                var res = await AddDeliverablesToVariationAsync(model.SelectedDeliverablesIds, productVariation.Id);
                if (!res.IsSuccess)
                {
                    result.Errors.Concat(res.Errors);
                    result.IsSuccess = false;
                }

            }

            return new ResultModel<Guid> { IsSuccess = true, Result = productVariation.Id };
        }

        public async Task<ResultModel> UpdateProductVariationAsync(UpdateProductVariationViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var productVariation = await _context.ProductVariations
                .FirstOrDefaultAsync(x => x.Id == model.ProducVariation.Id);

            productVariation.TechnologyStack = model.ProducVariation.TechnologyStack;
            productVariation.ProductType = model.ProducVariation.ProductType;
            productVariation.ProductTemplateId = model.ProducVariation.ProductTemplateId;

            _context.ProductVariations.Update(productVariation);

            var result = await _context.PushAsync();

            if(model.DeliverablesIds != null)
            {
                var res = await RemoveDeliverableFromVariationAsync(model.DeliverablesIds);
                if (!res.IsSuccess)
                {
                    result.Errors.Concat(res.Errors);
                    result.IsSuccess = false;
                }
            }

            if(model.SelectedDeliverablesIds != null)
            {
                var res = await AddDeliverablesToVariationAsync(model.SelectedDeliverablesIds, model.ProducVariation.Id);
                if (!res.IsSuccess)
                {
                    result.Errors.Concat(res.Errors);
                    result.IsSuccess = false;
                }

            }

            return result;
        }

        public async Task<ResultModel> DeleteProductVariationAsync(Guid? Id)
        {
            var result = await _context.RemovePermanentRecordAsync<ProductVariation>(Id);

            return result;
        }

        public async Task<ResultModel<IEnumerable<ProductVariationViewModel>>> GetProductVariationByProductTemplateId(Guid Id)
        {
            if (Id == null)
                return new InvalidParametersResultModel<IEnumerable<ProductVariationViewModel>>();

            var productVariationList = _context.ProductVariations
                .Include(i => i.ProductTemplate)
                .Where(x => x.ProductTemplateId == Id);


            return new SuccessResultModel<IEnumerable<ProductVariationViewModel>>(_mapper.Map<IEnumerable<ProductVariationViewModel>>(productVariationList));
        }

        public async Task<ResultModel<Guid>> AddProductDeliverableAsync(AddProductDeliverableViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel<Guid>();

            var productDeliverable = _mapper.Map<ProductDeliverables>(model);

            _context.ProductDeliverables.Add(productDeliverable);
            var result = await _context.PushAsync();

            if (!result.IsSuccess) return new ResultModel<Guid> { Errors = result.Errors, IsSuccess = false };

            return new ResultModel<Guid> { IsSuccess = true, Result = productDeliverable.Id };
        }

        public async Task<ResultModel> UpdateProductDeliverableAsync(ProductDeliverableViewModel model)
        {
            if (model == null)
                return new InvalidParametersResultModel();

            var productDeleiveralbe = await _context.ProductDeliverables.FirstOrDefaultAsync(x => x.Id == model.Id);

            productDeleiveralbe.Name = model.Name;
            productDeleiveralbe.ProductVariationId = model.ProductVariationId;

            _context.ProductDeliverables.Update(productDeleiveralbe);
            var result = await _context.PushAsync();

            return result;
        }

        public async Task<ResultModel> DeleteProductDeliverableAsync(Guid? Id) =>
            await _context.RemovePermanentRecordAsync<ProductDeliverables>(Id);


        public async Task<ResultModel<IEnumerable<ProductDeliverableViewModel>>> GetProductDeliverableByProductVariationId(Guid Id)
        {
            if (Id == null)
                return new InvalidParametersResultModel<IEnumerable<ProductDeliverableViewModel>>();

            var productDeliverables = _context.ProductDeliverables
                .Include(i => i.ProductVariation)
                .Where(x => x.ProductVariationId == Id);

            return new SuccessResultModel<IEnumerable<ProductDeliverableViewModel>>(_mapper.Map<IEnumerable<ProductDeliverableViewModel>>(productDeliverables));

        }

        public async Task<ResultModel> ActivateProductVariationAsync(Guid Id) =>
            await _context.ActivateRecordAsync<ProductVariation>(Id);


        public async Task<ResultModel> DisableProductVariationAsync(Guid Id) =>
            await _context.DisableRecordAsync<ProductVariation>(Id);

        public async Task<ResultModel<ProductVariationViewModel>> GetProductVariationByIdAsync(Guid? productVariationId)
        {
            if (productVariationId == null)
                return new InvalidParametersResultModel<ProductVariationViewModel>();

            var productVariation = await _context.ProductVariations
                .Include(i => i.ProductTemplate)
                .Include(i => i.ProductDeliverables)
                .FirstOrDefaultAsync(x => x.Id == productVariationId);

            return new SuccessResultModel<ProductVariationViewModel>(_mapper.Map<ProductVariationViewModel>(productVariation));
        }

        public async Task<ResultModel<ProductDeliverableViewModel>> GetProductDeliverableByIdAsync(Guid? productDeliverableId)
        {
            if (productDeliverableId == null)
                return new InvalidParametersResultModel<ProductDeliverableViewModel>();

            var productDeliverable = await _context.ProductDeliverables
                .Include(i => i.ProductVariation)
                .FirstOrDefaultAsync(x => x.Id == productDeliverableId);

            return new SuccessResultModel<ProductDeliverableViewModel>(_mapper.Map<ProductDeliverableViewModel>(productDeliverable));
        }


        public async Task<ResultModel> RemoveDeliverableFromVariationAsync(List<Guid> deliverablesIds)
        {
            if(deliverablesIds.Count > 0)
            {
                var deliverables = await _context.ProductDeliverables.Where(x => deliverablesIds.Contains(x.Id) && x.ProductVariationId != null).ToListAsync();
                foreach(var deliverable in deliverables)
                {
                    deliverable.ProductVariationId = null;
                }

                _context.ProductDeliverables.UpdateRange(deliverables);
                var result = await _context.PushAsync();

                return result;
            }

            return new ResultModel { IsSuccess = true } ;
        }


        public async Task<ResultModel> AddDeliverablesToVariationAsync(List<Guid> deliverablesIds, Guid variationId)
        {
            if (deliverablesIds.Count > 0)
            {
                var deliverables = await _context.ProductDeliverables.Where(x => deliverablesIds.Contains(x.Id)).ToListAsync();
                foreach (var deliverable in deliverables)
                {
                    deliverable.ProductVariationId = variationId;
                }

                _context.ProductDeliverables.UpdateRange(deliverables);
                var result = await _context.PushAsync();

                return result;
            }

            return new ResultModel { IsSuccess = true };
        }

        public async Task<ResultModel<PagedResult<ProductDeliverableViewModel>>> GetAllPaginatedProductDeliverablesAsync(PageRequest request)
        {
            var pagedResult = await _context.ProductDeliverables
                .Include(i => i.ProductVariation)
                .Where(x => !x.IsDeleted || request.IncludeDeleted)
                .GetPagedAsync(request);

            var map = pagedResult.Map(_mapper.Map<IEnumerable<ProductDeliverableViewModel>>(pagedResult.Result));
            return new SuccessResultModel<PagedResult<ProductDeliverableViewModel>>(map);
        }

        public async Task<ResultModel<IEnumerable<ProductVariationViewModel>>> GetAllProductVariationsAsync()
        {
            var ProductVariations = _context.ProductVariations
                .Include(i => i.ProductTemplate)
                .Where(x => !x.IsDeleted);

            return new SuccessResultModel<IEnumerable<ProductVariationViewModel>>{ Result = _mapper.Map<IEnumerable<ProductVariationViewModel>>(ProductVariations)};
        }


        public async Task<ResultModel> DisableProductDeliverableAsync(Guid? Id) =>
            await _context.DisableRecordAsync<ProductDeliverables>(Id);


        public async Task<ResultModel> ActivateProductDeliverableAsync(Guid? Id) =>
            await _context.ActivateRecordAsync<ProductDeliverables>(Id);


        public async Task<ResultModel<IEnumerable<ProductDeliverableViewModel>>> GetProductDeliverablesWithNoVariationAsync()
        {
            var productDeliverables = _context.ProductDeliverables
                .Where(x => x.ProductVariationId == null);

            return new SuccessResultModel<IEnumerable<ProductDeliverableViewModel>>(_mapper.Map<IEnumerable<ProductDeliverableViewModel>>(productDeliverables));
        }

    }
}
