import axios, { AxiosInstance, AxiosResponse, AxiosRequestConfig } from "axios";

export default abstract class HttpClient {
  protected readonly instance: AxiosInstance;

  constructor(baseURL: string) {
    this.instance = axios.create({
      baseURL,
    });
  }

  private readonly responseBody = (response: AxiosResponse) => response.data;

  protected readonly get = (url: string, config?: AxiosRequestConfig) =>
    this.instance.get(url, config).then(this.responseBody);

  protected readonly post = (url: string, body?: object, config?: AxiosRequestConfig) =>
    this.instance.post(url, body, config).then(this.responseBody);

  protected readonly put = (url: string, body?: object, config?: AxiosRequestConfig) =>
    this.instance.put(url, body, config).then(this.responseBody);

  protected readonly patch = (url: string, body?: object, config?: AxiosRequestConfig) =>
    this.instance.patch(url, body, config).then(this.responseBody);

  // protected readonly delete = (url: string, body?: object, config?: AxiosRequestConfig) =>
  //   this.instance.delete(url, body, config).then(this.responseBody);
  protected readonly delete = (url: string, body?: object, config?: AxiosRequestConfig) =>
    this.instance.delete(url, { ...config, data: body }).then(this.responseBody);
}
