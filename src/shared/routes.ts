import { EntityType, UserPermissions } from "~/security";

export interface IApplicationRoute {
  path: string;
  authorized?: boolean;
  annonymous?: boolean;
  permissions?: Partial<UserPermissions>;
  avoidBreadcrumbRender?: boolean;
}

export interface IApplicationRoutes {
  Home: IApplicationRoute;
  Layout: IApplicationRoute;
  Posts: IApplicationRoute;
  Login: IApplicationRoute;
  Register: IApplicationRoute;
}

export const routes: IApplicationRoutes = {
  Home: {
    path: "/",
  },
  Layout: {
    path: "/layout",
  },
  Posts: {
    path: "/posts",
    authorized: true,
    permissions: { read: [EntityType.Post] },
  },
  Login: {
    path: "/auth/login",
    annonymous: true,
  },
  Register: {
    path: "/auth/register",
    annonymous: true,
  },
};
