using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GR.Core;
using GR.Core.Abstractions;
using GR.Core.Helpers;
using GR.Core.Helpers.Pagination;
using GR.Core.Helpers.Responses;
using GR.Crm.Abstractions.Models;
using GR.Crm.Abstractions.ViewModels.CampaignTypeViewModel;
using GR.Crm.Abstractions.ViewModels.JobPositionViewModels;
using GR.Crm.Abstractions.ViewModels.ProductConfiguration;
using GR.Crm.Abstractions.ViewModels.ProductTypeViewModels;
using GR.Crm.Abstractions.ViewModels.ServiceTypeViewModels;
using GR.Crm.Abstractions.ViewModels.SolutionTypeViewModels;
using GR.Crm.Abstractions.ViewModels.SourceViewModels;
using GR.Crm.Abstractions.ViewModels.TechnologyTypeViewModels;

namespace GR.Crm.Abstractions
{
    public interface IVocabulariesService
    {

        #region JobPosition

        /// <summary>
        /// Get all paginated jop position
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<PagedResult<JobPositionViewModel>>> GetAllPaginatedJobPositionsAsync(PageRequest request);

        /// <summary>
        /// Get all jop position
        /// </summary>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<JobPositionViewModel>>> GetAllJobPositionsAsync();

        /// <summary>
        /// Get job position by id
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        Task<ResultModel<JobPositionViewModel>> GetJobPositionByIdAsync(Guid? jobPositionId);

        /// <summary>
        /// Add new job position
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddNewJobPositionAsync(JobPositionViewModel model);

        /// <summary>
        /// Update job position
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateJobPositionAsync(JobPositionViewModel model);

        /// <summary>
        /// Delete job position by id
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteJobPositionByIdAsync(Guid? jobPositionId);

        /// <summary>
        /// Activate job position 
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateJobPositionByIdAsync(Guid? jobPositionId);

        /// <summary>
        /// Disable job position
        /// </summary>
        /// <param name="jobPositionId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableJobPositionByIdAsync(Guid? jobPositionId);

        #endregion

        #region Source

        /// <summary>
        /// Get all sources
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetSourceViewModel>>> GetAllSourcesAsync(bool includeDeleted);

        /// <summary>
        /// get all paginated sources
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetSourceViewModel>>> GetAllPaginatedSourcesAsync(PageRequest request);

        /// <summary>
        /// get source by id
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        Task<ResultModel<GetSourceViewModel>> GetSourceByIdAsync(Guid? sourceId);

        /// <summary>
        /// add source 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddSourceAsync(SourceViewModel model);

        /// <summary>
        /// update source 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateSourceAsync(SourceViewModel model);

        /// <summary>
        /// Disable source 
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableSourceAsync(Guid? sourceId);

        /// <summary>
        /// Activate source
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateSourceAsync(Guid? sourceId);

        /// <summary>
        /// Delete source
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteSourceAsync(Guid? sourceId);

        #endregion

        #region SolutionType

        /// <summary>
        /// Get all solution type
        /// </summary>
        /// <param name="IncludeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetSolutionTypeViewModel>>> GetAllSolutionTypeAsync(bool IncludeDeleted);

        /// <summary>
        /// Get paginated solution type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetSolutionTypeViewModel>>> GetAllPaginatedSolutionTypeAsync(PageRequest request);

        /// <summary>
        /// Get solution type by id
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        Task<ResultModel<GetSolutionTypeViewModel>> GetSolutionTypeByIdAsync(Guid? solutionTypeId);

        /// <summary>
        /// Add solution type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddSolutionTypeAsync(SolutionTypeViewModel model);

        /// <summary>
        /// Update solution type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateSolutionTypeAsync(SolutionTypeViewModel model);

        /// <summary>
        /// Disable solution type
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableSolutionTypeAsync(Guid? solutionTypeId);
        /// <summary>
        /// Activate solution type
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
       Task<ResultModel> ActivateSolutionTypeAsync(Guid? solutionTypeId);


        /// <summary>
        /// Delete solution type
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteSolutionTypeAsync(Guid? solutionTypeId);

        #endregion

        #region TechnologyType

        /// <summary>
        /// Get all technology type
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetTechnologyTypeViewModel>>> GetAllTechnologyTypeAsync(bool includeDeleted);

        /// <summary>
        /// Get all paginated technology type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetTechnologyTypeViewModel>>> GetAllPaginatedTechnologyTypeAsync(
            PageRequest request);

        /// <summary>
        /// Get technology type by id
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        Task<ResultModel<GetTechnologyTypeViewModel>> GetTechnologyTypeByIdAsync(Guid? solutionTypeId);

        /// <summary>
        /// Add technology type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddTechnologyTypeAsync(TechnologyTypeViewModel model);

        /// <summary>
        /// Update technology type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateTechnologyTypeAsync(TechnologyTypeViewModel model);

        /// <summary>
        /// Disable technology type 
        /// </summary>
        /// <param name="technologyTypeId"></param>
        /// <returns></returns>
       Task<ResultModel> DisableTechnologyTypeAsync(Guid? technologyTypeId);

        /// <summary>
        /// Activate technology type
        /// </summary>
        /// <param name="technologyTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateTechnologyTypeAsync(Guid? technologyTypeId);

        /// <summary>
        /// Delete technology type
        /// </summary>
        /// <param name="technologyTypeId"></param>
        /// <returns></returns>
       Task<ResultModel> DeleteTechnologyTypeAsync(Guid? technologyTypeId);

       Task<ResultModel> AddTechnologyTypeAsync(List<string> technologyType);

        #endregion

        #region ProductType

        /// <summary>
        /// Get all product type
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetProductTypeViewModel>>> GetAllProductTypeAsync(bool includeDeleted);

        /// <summary>
        /// get all paginated product type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetProductTypeViewModel>>> GetAllPaginatedProductTypeAsync(
            PageRequest request);

        /// <summary>
        /// Get product type by id
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        Task<ResultModel<GetProductTypeViewModel>> GetProductTypeByIdAsync(
            Guid? serviceTypeId);

        /// <summary>
        /// Add product type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddProductTypeAsync(ProductTypeViewModel model);

        /// <summary>
        /// Update product type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateProductTypeAsync(ProductTypeViewModel model);

        /// <summary>
        /// Disable product type
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
       Task<ResultModel> DisableProductTypeAsync(Guid? productTypeId);

        /// <summary>
        /// Activate product type
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
       Task<ResultModel> ActivateProductTypeAsync(Guid? productTypeId);

        /// <summary>
        /// Delete product type
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteProductTypeAsync(Guid? productTypeId);

        #endregion

        #region ServiceType

        /// <summary>
        /// Get all service type
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetServiceTypeViewModel>>> GetAllServiceTypeAsync(bool includeDeleted);

        /// <summary>
        /// get all paginated service type
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetServiceTypeViewModel>>> GetAllPaginatedServiceTypeAsync(PageRequest request);

        /// <summary>
        /// Get service type by id
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        Task<ResultModel<GetServiceTypeViewModel>> GetServiceTypeByIdAsync(Guid? serviceTypeId);

        /// <summary>
        /// Add service type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddServiceTypeAsync(ServiceTypeViewModel model);

        /// <summary>
        /// Update service type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateServiceTypeAsync(ServiceTypeViewModel model);

        /// <summary>
        /// Disable service type
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableServiceTypeAsync(Guid? serviceTypeId);

        /// <summary>
        /// Activate service type
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
       Task<ResultModel> ActivateServiceTypeAsync(Guid? serviceTypeId);

        /// <summary>
        /// Delete service type
        /// </summary>
        /// <param name="serviceTypeId"></param>
        /// <returns></returns>
       Task<ResultModel> DeleteServiceTypeAsync(Guid? serviceTypeId);

        #endregion

        #region CampaignType

        /// <summary>
        /// Get all campaign types
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetCampaignTypeViewModel>>> GetAllCampaignTypeAsync(bool includeDeleted);

        /// <summary>
        /// Get all paginated campaign types
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetCampaignTypeViewModel>>> GetAllPaginatedCampaignTypeAsync(
            PageRequest request);

        /// <summary>
        /// Get campaign type by id
        /// </summary>
        /// <param name="solutionTypeId"></param>
        /// <returns></returns>
        Task<ResultModel<GetCampaignTypeViewModel>> GetCampaignTypeByIdAsync(Guid? solutionTypeId);

        /// <summary>
        /// Add campaign type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel<Guid>> AddCampaignTypeAsync(CampaignTypeViewModel model);

        /// <summary>
        /// Update campaign type
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateCampaignTypeAsync(CampaignTypeViewModel model);

        /// <summary>
        /// Disable technology type 
        /// </summary>
        /// <param name="campaignTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> DisableCampaignTypeAsync(Guid? campaignTypeId);

        /// <summary>
        /// Activate campaign type
        /// </summary>
        /// <param name="campaignTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> ActivateCampaignTypeAsync(Guid? campaignTypeId);

        /// <summary>
        /// Delete campaign type
        /// </summary>
        /// <param name="campaignTypeId"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteCampaignTypeAsync(Guid? campaignTypeId);

        #endregion


        #region Product Configuration
        /// <summary>
        /// Add product or service
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        Task<ResultModel> AddProductOrServiceAsync(List<string> values);

        /// <summary>
        /// Delete product or service
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteProductOrServiceAsync(Guid Id);

        /// <summary>
        /// Get all products and  services
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetProductOrServiceViewModel>>> GetAllProductsAndServicesAsync(bool includeDeleted);

        #region ConsultancyVariations
        /// <summary>
        /// Get all consultancy service
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetConsultancyServiceViewModel>>> GetAllConsultancyVariationsAsync(bool includeDeleted);

        /// <summary>
        /// Add consultancy variation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddConsultancyVariationAsync(AddServiceVariationViewModel model);

        /// <summary>
        /// Delete Service Consultancy
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteConsultancyVariationAsync(Guid Id);

        /// <summary>
        /// Update consultancy variation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateConsultancyVariationAsync(UpdateConsultancyVariationViewModel model);

        /// <summary>
        /// Get consultancy variation by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<GetConsultancyServiceViewModel>> GetConsultancyVariationByIdAsync(Guid Id);

        /// <summary>
        /// Get all paginated consultancy variations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetConsultancyServiceViewModel>>> GetAllPaginatedConsultancyVariationsAsync(PageRequest request);
        #endregion ConsultancyVariations

        #region DesigneVariations
        /// <summary>
        /// Get all designe service
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetDesigneServiceViewModel>>> GetAllDesigneVariationsAsync(bool includeDeleted);

        /// <summary>
        /// AddDesigneVariation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddDesigneVariationAsync(AddServiceVariationViewModel model);

        /// <summary>
        /// Delete Service Designe
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteDesigneVariationByIdAsync(Guid Id);

        /// <summary>
        /// Update designe variation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateDesigneVariationAsync(UpdateDesigneVariationViewModel model);

        /// <summary>
        /// Get desinge variation
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<GetDesigneServiceViewModel>> GetDesigneVariationsByIdAsync(Guid Id);

        /// <summary>
        /// GetPaginatedDesigneVariations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetDesigneServiceViewModel>>> GetPaginatedDesigneVariationsAsync(PageRequest request);
        #endregion

        #region DevelopmentVariations
        /// <summary>
        /// Get all developement service
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetDevelopementServiceViewModel>>> GetAllDevelopmentVariations(bool includeDeleted);

        /// <summary>
        /// Add Developement variation
        /// </summary>
        /// <param name="serviceDevelopement"></param>
        /// <returns></returns>
        Task<ResultModel> AddDevelopmentVariationAsync(AddServiceVariationViewModel model);

        /// <summary>
        /// Delete development variation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteDevelopmentVariationAsync(Guid id);

        /// <summary>
        /// Update development variation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateDevelopmentVariationAsync(UpdateDevelopmentVariationViewModel model);

        /// <summary>
        /// Get development service 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<GetDevelopementServiceViewModel>> GetDevelopmentServiceByIdAsyc(Guid Id);

        /// <summary>
        /// Get Paginated development service
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetDevelopementServiceViewModel>>> GetPaginatedDevelopmentVariationsAsync(PageRequest request);
        #endregion

        #region QaVariations

        /// <summary>
        /// Add QaVariation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddQaVariationAsync(AddServiceVariationViewModel model);

        /// <summary>
        /// DeleteService QAAsync
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteQaVariationAsync(Guid Id);

        /// <summary>
        /// Update QaVariation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateQaVariationsAsync(UpdateQAVariationViewModel model);

        /// <summary>
        /// Get QaVariationBy id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<GetQAServiceViewModel>> GetQaVariationByIdAsync(Guid Id);

        /// <summary>
        /// Get paginated QaVariations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetQAServiceViewModel>>> GetPaginatedQaVariationsAsync(PageRequest request);

        /// <summary>
        /// Get all qa service
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetQAServiceViewModel>>> GetAllQAVariationsAsync(bool includeDeleted);
        #endregion

        #region PMFrameWork

        /// <summary>
        /// Get all pm frameworks
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetPMFramewoksViewModel>>> GetAllPMFramewoksAsync(bool includeDeleted);

        /// <summary>
        /// Add Pm Framework
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddPmFrameworkAsync(AddFrameWorkViewModel model);

        /// <summary>
        /// Updarte Pm Framework
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdatePmFrameworkAsync(UpdatePMFrameworkViewModel model);

        /// <summary>
        /// Delete pm framewok
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DeletePmFrameworkByIdAsync(Guid Id);

        /// <summary>
        /// Get Pm framewoks by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<GetPMFramewoksViewModel>> GetPmFrameWorkByIdAsync(Guid Id);

        /// <summary>
        /// Get Paginated pm frameworks
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetPMFramewoksViewModel>>> GetPaginatedPmFramewoksAsync(PageRequest request);
        #endregion

        #region DevFrameWork

        
        /// <summary>
        /// Get all development frameworks
        /// </summary>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        Task<ResultModel<IEnumerable<GetDevelopementFrameWorkViewModel>>> GetAllDevelopementFramewoksAsync(bool includeDeleted);

        /// <summary>
        /// Add dev framework
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> AddDevFrameworkAsync(AddFrameWorkViewModel model);

        /// <summary>
        /// Update dev framework
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ResultModel> UpdateDevFrameworkAsync(UpdateDevelopmentFrameworkViewModel model);

        /// <summary>
        /// Delete dev framework
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel> DeleteDevFrameworkByIdAsync(Guid Id);

        /// <summary>
        /// Get paginated dev frameworks
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResultModel<PagedResult<GetDevelopementFrameWorkViewModel>>> GetPaginatedDevFrameworksAsync(PageRequest request);

        /// <summary>
        /// Get dev framework by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultModel<GetDevelopementFrameWorkViewModel>> GetDevelopmentFrameworkByIdAsync(Guid Id);
        #endregion
        #endregion
    }
}
