import { AxiosError, AxiosRequestConfig, AxiosResponse } from "axios";
import { HttpClient } from "~/api";
import { Paths } from "~/core";
import {
  IPost,
  IPostCreate,
  IPostPreview,
  IPageRequest,
  IResponse,
  IUserPreview,
  IUser,
  IAuthModel,
  IAuthResponse,
} from "~/models";
import { getValuePath } from "~/shared";

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
        };

        return config;
      },
      (error: AxiosError) => {
        // Do something with request error
        return Promise.reject(error);
      },
    );
  };

  private readonly initializeResponseInterceptor = () => {
    /* istanbul ignore next */
    this.instance.interceptors.response.use(
      (response: AxiosResponse<unknown>) => {
        // Any status code that lies within the range of 2xx causes this function to trigger
        // Do something with response data
        return response;
      },
      (error: AxiosError) => {
        // Any status code that falls outside the range of 2xx causes this function to trigger
        // Do something with response error
        return Promise.reject(error);
      },
    );
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
    getList: (params: IPageRequest): Promise<IResponse<IUserPreview>> => this.get("/user", { params }),
    getUserById: (id: string): Promise<IUser> => this.get(`/user/${id}`),
    createUser: (user: IUser): Promise<IUser> => this.post("/user/create", user),
    updateUser: (user: IUser): Promise<IUser> => this.put(`/user/${user.id}`, user),
    deleteUser: (id: string): Promise<string> => this.delete(`/user/${id}`),
  };

  public readonly Tags = {
    getList: (): Promise<IResponse<string[]>> => this.get("/tag"),
  };

  public readonly Auth = {
    register: (model: IAuthModel): Promise<IAuthResponse> =>
      this.post("/register", model, { baseURL: process.env.NEXT_PUBLIC_AUTH_URL }),
    login: (model: IAuthModel): Promise<IAuthResponse> =>
      this.post("/login", model, { baseURL: process.env.NEXT_PUBLIC_AUTH_URL }),
  };

  public readonly Layout = {
    register: (model: IAuthModel): Promise<IAuthResponse> =>
      this.post("/register", model, { baseURL: process.env.NEXT_PUBLIC_AUTH_URL }),
    login: (model: IAuthModel): Promise<IAuthResponse> =>
      this.post("/login", model, { baseURL: process.env.NEXT_PUBLIC_AUTH_URL }),
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
