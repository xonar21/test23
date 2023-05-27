import { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import React from "react";
import ReactDOM from "react-dom";
import { HttpClient } from "~/api";
import ErrorModal from "~/components/errorModal";
import { Paths } from "~/core";
import {
  IPost,
  IPostCreate,
  IPostPreview,
  IPageRequest,
  IResponse,
  IUserPreview,
  IRolePreview,
  IRoleCreate,
  IPermissionPreview,
  IAuthModel,
  IAuthResponse,
  ISaiseModel,
  IMpassModel,
  IMpassResponse,
  IRegionPreview,
  IRegionTypePreview,
  IVoterProfilePreview,
  IElectionFunctionPreview,
  IElectionFunctionCreate,
  IUsersPreview,
  ISubscriptionListStatusCreate,
  ISubscriptionListStatusPreview,
  IGendersPreview,
  IGendersUpdate,
  ISubscriptionListCreate,
  ISubscriptionListsPreview,
  IElectionPreview,
  IElectionImport,
  IWorkFlowsPreview,
  IWorkFlowUpdate,
  IPoliticalPartiesPreview,
  IPoliticalPartiesCreate,
  IElectionTypesCreate,
  IElectionTypesPreview,
  IRefreshModel,
  IElectionStatusPreview,
  IElectionStatusUpdate,
  IAuditPreview,
  INotificationEventsPreview,
  INotificationProfilesPreview,
  INotificationProfileUpdate,
  INotificationsPreview,
} from "~/models";
import { getValuePath } from "~/shared";
import { store } from "~/store";

interface PendingRequest {
  resolve: (value: AxiosResponse<any> | PromiseLike<AxiosResponse<any>>) => void;
  reject: (reason?: any) => void;
}
export class WebApi extends HttpClient {
  private static webApiInstance: WebApi;
  private constructor() {
    super(process.env.NEXT_PUBLIC_API_URL as string);
    this.initializeRequestInterceptor();
    this.initializeResponseInterceptor();
  }

  private readonly initializeRequestInterceptor = () => {
    /* istanbul ignore next */
    this.instance.interceptors.request.use(
      (config: AxiosRequestConfig) => {
        // Do something before request is sent
        config.headers = {
          ["app-id"]: process.env.NEXT_PUBLIC_APP_ID as string,
          Authorization: `Bearer ${store.getState().AUTH.token}`,
          // Authorization: `Bearer ${store.getState().AUTH.token}`,
        };

        return config;
      },
      (error: AxiosError) => {
        // Do something with request error
        return Promise.reject(error);
      },
    );
  };

  private initializeResponseInterceptor = () => {
    /* istanbul ignore next */
    this.instance.interceptors.response.use(
      (response: AxiosResponse) => {
        // Any status code that lies within the range of 2xx causes this function to trigger
        // Do something with response data
        if (response.data.statusCode === 400) {
          this.showErrorModal("Eroare de autorizare", `${response.data.value}`);
        }
        return response;
      },
      async (error: AxiosError) => {
        // Any status code that falls outside the range of 2xx causes this function to trigger
        // Do something with response error

        if (error.response) {
          if (error.response.status === 400) {
            this.showErrorModal(
              "Eroare de validare",
              "A apărut o eroare de validare. Fereastra se va închide în.",
              error,
            );
            return;
          }
          if (error.response.status === 404) {
            this.showErrorModal("Eroare 404", "Ne pare rău, pagina pe care ați solicitat-o nu a fost găsită.", error);
            return;
          }
          if (error.response.status === 403) {
            this.showErrorModal(
              "Eroare de permisiune",
              "Din păcate, nu aveți permisiunea pentru această acțiune.",
              error,
            );
            return;
          }
          if (error.response.status === 401) {
            this.showErrorModal("Eroare de autorizare", "A apărut o eroare de autorizare, veți fi deconectat.", error);
            return;
          }
          this.showErrorModal(
            "Eroare de conectare",
            "S-a produs o eroare de conexiune. Fereastra se va închide în.",
            error,
          );
        }

        return Promise.reject(error);
      },
    );
  };

  private showErrorModal = (title: string, message: string, error?: any) => {
    const errorModal = document.createElement("div");
    document.body.appendChild(errorModal);

    ReactDOM.render(React.createElement(ErrorModal, { title, message, error }), errorModal);
  };

  public static getInstance(): WebApi {
    if (!WebApi.webApiInstance) WebApi.webApiInstance = new WebApi();

    return WebApi.webApiInstance;
  }

  public readonly Posts = {
    getList: (params: IPageRequest): Promise<IResponse<IPostPreview>> => this.get("/post", { params }),
    getListByUser: (userId: string, params: IPageRequest): Promise<IResponse<IPostPreview[]>> =>
      this.get(`/user/${userId}/post`, { params }),
    getListByTag: (tag: string, params: IPageRequest): Promise<IResponse<IPostPreview[]>> =>
      this.get(`/tag/${tag}/post`, { params }),
    getPostById: (id: string): Promise<IPost> => this.get(`/post/${id}`),
    createPost: (post: IPostCreate): Promise<IPost> => this.post("/post/create", post),
    updatePost: (post: IPost): Promise<IPost> => this.put(`/post/${post.id}`, post),
    deletePost: (id: string): Promise<string> => this.delete(`/post/${id}`),
  };

  public readonly Users = {
    getList: (params: any): Promise<IResponse<IUserPreview>> => this.get(`/users/`, { params }),
    getUserById: (id: string): Promise<IUserPreview> => this.get(`/identity/${id}`),
    getInternalUsers: (params: any): Promise<IResponse<IUsersPreview>> => this.get("/users/internal/", { params }),
    getExternalUsers: (params: any): Promise<IResponse<IUsersPreview>> => this.get("/users/external/", { params }),
  };

  public readonly Roles = {
    getList: (params: any): Promise<IResponse<IRolePreview>> => this.get(`/roles`, { params }),
    getRoleId: (params: any): Promise<IResponse<IRolePreview>> => this.get(`/role/${params.id}`),
    createRoles: (role: IRoleCreate): Promise<IRolePreview> => this.post("/roles", role),
    deleteRoles: (id: string): Promise<string> => this.delete(`/roles/${id}`),
    updateRoles: (role: IRolePreview): Promise<IRolePreview> => this.put("/roles", role),
  };

  public readonly Permissions = {
    getList: (): Promise<IResponse<IPermissionPreview>> => this.get("/claims"),
  };

  public readonly Regions = {
    getList: (params: any): Promise<IResponse<IRegionPreview>> => this.get(`/regions`, { params }),
    updateRegions: (region: IRegionPreview): Promise<IRegionPreview> => this.put("/regions", region),
  };

  public readonly RegionTypes = {
    getList: (params: any): Promise<IResponse<IRegionTypePreview>> => this.get(`/regiontypes`, { params }),
    updateRegionTypes: (regionType: IRegionTypePreview): Promise<IRegionTypePreview> =>
      this.put("/regiontypes", regionType),
  };

  public readonly Tags = {
    getList: (): Promise<IResponse<string[]>> => this.get("/tag"),
  };

  public readonly VoterProfile = {
    getProfile: (): Promise<IResponse<IVoterProfilePreview>> => this.get("/voter-profiles/own"),
    getProfiles: (params: any): Promise<IResponse<IVoterProfilePreview>> => this.get("/voter-profiles/", { params }),
    getProfileIdnp: (params: any): Promise<IResponse<IVoterProfilePreview>> =>
      this.get(`/voter-profiles/${params.idnp}`),
    getProfileAdress: (params: any): Promise<IResponse<IVoterProfilePreview>> =>
      this.get("/voter-profiles/own/revisions/addresses", { params }),
    getProfileAdressIdnp: (params: {
      idnp: string;
      PageNumber?: number;
      PageSize?: number;
      Filters?: object;
      SortField?: string;
      SortOrder?: string;
    }): Promise<IResponse<IVoterProfilePreview>> =>
      this.get(`/voter-profiles/${params.idnp}/revisions/addresses`, {
        params: {
          PageNumber: params.PageNumber,
          PageSize: params.PageSize,
          Filters: params.Filters,
          SortField: params.SortField,
          SortOrder: params.SortOrder,
        },
      }),
    getProfilePerson: (params: any): Promise<IResponse<IVoterProfilePreview>> =>
      this.get("/voter-profiles/own/revisions/persons", { params }),
    getProfilePersonIdnp: (params: {
      idnp: string;
      PageNumber?: number;
      PageSize?: number;
      Filters?: object;
      SortField?: string;
      SortOrder?: string;
    }): Promise<IResponse<IVoterProfilePreview>> =>
      this.get(`/voter-profiles/${params.idnp}/revisions/persons`, {
        params: {
          PageNumber: params.PageNumber,
          PageSize: params.PageSize,
          Filters: params.Filters,
          SortField: params.SortField,
          SortOrder: params.SortOrder,
        },
      }),
    postEmailAttach: (params: any): Promise<any> => this.post(`/voter-profiles/${params.email}/attach`, params),
    postConfirmAttached: (params: any): Promise<any> => this.post(`/voter-profiles/confirm-attached-email`, params),
  };

  public readonly ElectionFunction = {
    getList: (params: any): Promise<IResponse<IElectionFunctionPreview>> => this.get("/elective-functions", { params }),
    getSelectList: (params: any): Promise<IResponse<IElectionFunctionPreview>> =>
      this.get("/elective-functions/select-list", { params }),
    createElectionFunction: (electionFunction: IElectionFunctionCreate): Promise<IElectionFunctionCreate> =>
      this.post("/elective-functions", electionFunction),
    deleteElectionFunction: (id: string): Promise<string> => this.delete(`/elective-functions/${id}`),
    updateElectionFunction: (electionFunction: IElectionFunctionCreate): Promise<IElectionFunctionCreate> =>
      this.put(`/elective-functions/${electionFunction.id}`, electionFunction),
  };

  public readonly ElectionStatus = {
    getList: (params: any): Promise<IResponse<ISubscriptionListStatusPreview>> =>
      this.get("/election-statuses", { params }),
    getSelectList: (params: any): Promise<IResponse<IElectionStatusPreview>> =>
      this.get("/election-statuses/select-list", { params }),
    updateElectionStatus: (electionStatus: IElectionStatusUpdate): Promise<IElectionStatusUpdate> =>
      this.put(`/election-statuses/${electionStatus.id}`, electionStatus),
  };

  public readonly SubscriptionListStatus = {
    getList: (params: any): Promise<IResponse<ISubscriptionListStatusPreview>> =>
      this.get("/subscription-list-statuses", { params }),
    getSelectList: (params: any): Promise<IResponse<ISubscriptionListStatusPreview>> =>
      this.get("/subscription-list-statuses/select-list", { params }),
    deleteSubscriptionListStatus: (id: string): Promise<string> => this.delete(`/subscription-list-statuses/${id}`),
    updateSubscriptionListStatus: (
      subscriptionListStatus: ISubscriptionListStatusCreate,
    ): Promise<ISubscriptionListStatusCreate> =>
      this.put(`/subscription-list-statuses/${subscriptionListStatus.id}`, subscriptionListStatus),
  };

  public readonly SubscriptionList = {
    getList: (params: any): Promise<IResponse<ISubscriptionListsPreview>> =>
      this.get("/subscription-lists", { params }),
    deleteSubscriptionList: (id: string): Promise<string> => this.delete(`/subscription-lists/${id}`),
    updateSubscriptionList: (subscriptionList: ISubscriptionListCreate): Promise<ISubscriptionListCreate> =>
      this.put(`/subscription-lists/${subscriptionList.id}`, subscriptionList),
    createSubscriptionList: (subscriptionList: ISubscriptionListCreate): Promise<ISubscriptionListCreate> =>
      this.post("/subscription-lists", subscriptionList),
    getListForSigning: (params: any): Promise<IResponse<ISubscriptionListsPreview>> =>
      this.get("/subscription-lists/available-for-signing", { params }),
    getListForSigningMy: (params: any): Promise<IResponse<ISubscriptionListsPreview>> =>
      this.get("/signatures/my-signatures", { params }),
    getListForSigningActive: (params: any): Promise<IResponse<ISubscriptionListsPreview>> =>
      this.get("/signatures/my-signatures/active", { params }),
    getListForSigningArhived: (params: any): Promise<IResponse<ISubscriptionListsPreview>> =>
      this.get("/signatures/my-signatures/archived", { params }),
    subscriptionListActivate: (subscriptionList: any): Promise<any> =>
      this.put(`/subscription-lists/${subscriptionList.subscriptionListId}/activate`, subscriptionList),
    subscriptionListDeactivate: (subscriptionList: any): Promise<any> =>
      this.put(`/subscription-lists/${subscriptionList.subscriptionListId}/stop`, subscriptionList),
  };

  public readonly Genders = {
    getList: (params: any): Promise<IResponse<IGendersPreview>> => this.get("/genders", { params }),
    getSelectList: (params: any): Promise<IResponse<IGendersPreview>> => this.get("/genders/select-list", { params }),
    deleteGenders: (id: string): Promise<string> => this.delete(`/genders/${id}`),
    updateGenders: (gender: IGendersUpdate): Promise<IGendersUpdate> => this.put(`/genders/${gender.id}`, gender),
  };

  public readonly Elections = {
    getList: (params: any): Promise<IResponse<IElectionPreview>> => this.get("/elections/existing", { params }),
    getSelectList: (params: any): Promise<IResponse<IElectionPreview>> =>
      this.get("/elections/select-list", { params }),
    getSubSelectList: (params: {
      id: string;
      PageNumber?: number;
      PageSize?: number;
      Filters?: any;
      SortField?: string;
      SortOrder?: string;
    }): Promise<IResponse<IVoterProfilePreview>> =>
      this.get(`/elections/${params.id}/circumscriptions/select-list`, {
        params: {
          PageNumber: params.PageNumber,
          PageSize: params.PageSize,
          Filters: params.Filters,
          SortField: params.SortField,
          SortOrder: params.SortOrder,
        },
      }),
    getListFromSaise: (): Promise<IResponse<IElectionPreview>> => this.get("/elections"),
    importElection: (election: IElectionImport): Promise<IElectionImport> => this.post("/elections/import", election),
    updateElection: (election: IElectionImport): Promise<IElectionImport> => this.put(`/elections`, election),
  };

  public readonly WorkFlows = {
    getWorkFlows: (params: any): Promise<IResponse<IWorkFlowsPreview>> => this.get("/workflows", { params }),
    getWorkFlowsStatusSubscriptionList: (): Promise<IResponse<IWorkFlowsPreview>> =>
      this.get("/workflows/subscription-list/states"),
    getWorkFlowsStatus: (id: any): Promise<IResponse<IWorkFlowsPreview>> => this.get(`/workflows/${id}/states`),
    getWorkFlowStateTransitions: (params: {
      workFlowId: string;
      stateId: string;
    }): Promise<IResponse<IWorkFlowsPreview>> =>
      this.get(`/workflows/${params.workFlowId}/states/${params.stateId}/transitions`),
    getWorkFlowTransitions: (params: { workFlowId: string }): Promise<IResponse<IWorkFlowsPreview>> =>
      this.get(`/workflows/${params.workFlowId}/transitions`),
    updateWorkFlow: (workFlow: IWorkFlowUpdate): Promise<IWorkFlowUpdate> => this.put(`/workflows`, workFlow),
    updateWorkFlowTransitions: (workFlow: IWorkFlowUpdate): Promise<IWorkFlowUpdate> =>
      this.put(`/workflows/${workFlow.workflowId}/transitions`, workFlow),
    updateWorkFlowStatus: (status: any): Promise<IWorkFlowUpdate> =>
      this.put(`/workflows/${status.workflowId}/states/${status.stateId}`, status),
  };

  public readonly PoliticalParties = {
    getList: (params: any): Promise<IResponse<IPoliticalPartiesPreview>> => this.get("/political-parties", { params }),
    getSelectList: (params: any): Promise<IResponse<IPoliticalPartiesPreview>> =>
      this.get("/political-parties/select-list", { params }),
    deletePoliticalParties: (id: string): Promise<string> => this.delete(`/political-parties/${id}`),
    deactivatePoliticalParties: (politicalParties: any): Promise<any> =>
      this.delete(`/political-parties/deactivate-political-parties`, politicalParties),
    activatePoliticalParties: (politicalParties: any): Promise<any> =>
      this.delete(`/political-parties/activate-political-parties`, politicalParties),
    updatePoliticalParties: (politicalParties: IPoliticalPartiesCreate): Promise<IPoliticalPartiesCreate> =>
      this.put(`/political-parties/${politicalParties.id}`, politicalParties),
    createPoliticalParties: (politicalParties: IPoliticalPartiesCreate): Promise<IPoliticalPartiesCreate> =>
      this.post("/political-parties", politicalParties),
  };

  public readonly ElectionTypes = {
    getList: (params: any): Promise<IResponse<IElectionTypesPreview>> => this.get("/election-types", { params }),
    deleteElectionType: (id: string): Promise<string> => this.delete(`/election-types/${id}`),
    deactivateElectionType: (electionTypes: any): Promise<any> => this.put(`/election-types/deactivate`, electionTypes),
    activateElectionType: (electionTypes: any): Promise<any> => this.put(`/election-types/activate`, electionTypes),
    updateElectionType: (electionTypes: IElectionTypesCreate): Promise<IElectionTypesCreate> =>
      this.put(`/election-types/${electionTypes.id}`, electionTypes),
    createElectionType: (electionTypes: IElectionTypesCreate): Promise<IElectionTypesCreate> =>
      this.post("/election-types", electionTypes),
  };

  public readonly NotificationEvent = {
    getList: (): Promise<IResponse<INotificationEventsPreview>> => this.get("/notification-events"),
  };

  public readonly NotificationProfile = {
    getListOwn: (): Promise<IResponse<INotificationProfilesPreview>> => this.get("/notification-profiles/own"),
    updateNotificationProfile: (notificationProfile: any): Promise<INotificationProfileUpdate> =>
      this.put(`/notification-profiles`, notificationProfile),
  };

  public readonly Notifications = {
    getList: (params: any): Promise<IResponse<INotificationsPreview>> =>
      this.get("/notification-profiles/own-notifications", { params }),
    getCount: (): Promise<IResponse<any>> => this.get("/notification-profiles/count-unread"),
    updateNotificationsRead: (notifications: any): Promise<any> =>
      this.put(`/notification-profiles/mark-notifications-as-read`, notifications),
  };

  public readonly Msign = {
    signTest: (sign: any): Promise<any> => this.post("/msign/sign/test", sign),
  };

  public readonly Audit = {
    getList: (params: any): Promise<IResponse<IAuditPreview>> => this.get("/audit-events", { params }),
  };

  public readonly Auth = {
    register: (model: IAuthModel): Promise<IAuthResponse> =>
      this.post("/register", model, { baseURL: process.env.NEXT_PUBLIC_API_URL }),
    login: (model: IAuthModel): Promise<IAuthResponse> =>
      this.post("Identity/login", model, { baseURL: process.env.NEXT_PUBLIC_AUTH_URL }),
    loginMpass: (model: IMpassModel): Promise<IMpassResponse> =>
      this.post("identity/login/mpass/test", model, { baseURL: process.env.NEXT_PUBLIC_AUTH_URL }),
    loginSaise: (model: ISaiseModel): Promise<IAuthResponse> =>
      this.post("identity/login/saiseadmin", model, { baseURL: process.env.NEXT_PUBLIC_API_URL }),
    refreshToken: (model: IRefreshModel): Promise<IAuthResponse> =>
      this.post("identity/refresh", model, { baseURL: process.env.NEXT_PUBLIC_API_URL }),
    invalidToken: (model: IRefreshModel): Promise<IAuthResponse> =>
      this.post("identity/token/invalidate", model, { baseURL: process.env.NEXT_PUBLIC_API_URL }),
    getCurrentUser: (): Promise<IResponse<any>> => this.get("identity/current-user"),
  };
}

const webApi = WebApi.getInstance();

/**
 * Gets the path to a given promise function. Used to pass a serialized value to the Redux store
 * @param api WebApi promise function selector
 * @see @link https://redux.js.org/faq/organizing-state#can-i-put-functions-promises-or-other-non-serializable-items-in-my-store-state
 */
/* istanbul ignore next */
export const getPromisePath = <TArgs>(api: (args: TArgs) => Promise<unknown>): Paths<WebApi> => {
  return getValuePath(webApi, api) as Paths<WebApi>;
};

export default webApi;
