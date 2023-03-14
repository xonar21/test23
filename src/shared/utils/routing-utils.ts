import { IApplicationRoute, routes } from "~/shared/routes";

/**
 * Returns a valid hypertext reference, based on the application route object,
 * replacing all route and query parameters with passed values
 * @param route Application route object
 * @param routeParams Route parameters
 * @param queryParams Query string parameters
 */
export const getRoutePath = (
  route: IApplicationRoute,
  routeParams?: { [x: string]: string | number },
  queryParams?: { [x: string]: string | number },
) => {
  let path = route.path;

  if (routeParams) {
    Object.keys(routeParams).forEach(key => {
      if (path.includes(`[${key}]`)) {
        path = path.replace(`[${key}]`, routeParams[key].toString());
      }
    });
  }

  if (queryParams) {
    path +=
      "?" +
      Object.keys(queryParams)
        .map(key => `${key}=${queryParams[key]}`)
        .join("&");
  }

  return path;
};

export const getRouteByPath = (path: string): IApplicationRoute => {
  return Object.values(routes).find((route: IApplicationRoute) => route.path === path);
};
