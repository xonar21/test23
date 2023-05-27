import { UserPermissions } from "~/security";

export interface IApplicationRoute {
  path: string;
  authorized?: boolean;
  annonymous?: boolean;
  permissions?: Partial<UserPermissions>;
  avoidBreadcrumbRender?: boolean;
}

export interface IApplicationRoutes {
  Home: IApplicationRoute;
  Posts: IApplicationRoute;
  Login: IApplicationRoute;
  Register: IApplicationRoute;
  PersonalData: IApplicationRoute;
  Subscriptions: IApplicationRoute;
  Notifications: IApplicationRoute;
  NotificationSettings: IApplicationRoute;
  Events: IApplicationRoute;
  Elections: IApplicationRoute;
  Circumscriptions: IApplicationRoute;
  SubscriptionLists: IApplicationRoute;
  UserManagement: IApplicationRoute;
  RoleManagement: IApplicationRoute;
  PermissionManagement: IApplicationRoute;
  VoterManagement: IApplicationRoute;
  Regions: IApplicationRoute;
  DesignationSubjects: IApplicationRoute;
  RegionTypes: IApplicationRoute;
  ElectionTypes: IApplicationRoute;
  loginSaise: IApplicationRoute;
  Public: IApplicationRoute;
  ElectionFunction: IApplicationRoute;
  SubscriptionListStatus: IApplicationRoute;
  Genders: IApplicationRoute;
  Workflows: IApplicationRoute;
  PoliticalParties: IApplicationRoute;
  ElectionStatus: IApplicationRoute;
  Audit: IApplicationRoute;
}

export const routes: IApplicationRoutes = {
  Home: {
    path: "/",
  },
  Posts: {
    path: "/posts",
    authorized: true,
  },
  Login: {
    path: "/auth/login",
    annonymous: true,
  },
  loginSaise: {
    path: "/auth/loginSaise",
    annonymous: true,
  },
  Register: {
    path: "/auth/register",
    annonymous: true,
  },
  PersonalData: {
    path: "/personal-data",
    authorized: true,
  },
  Subscriptions: {
    path: "/subscriptions",
    authorized: true,
  },
  Notifications: {
    path: "/notifications",
    authorized: true,
  },
  NotificationSettings: {
    path: "/notification-settings",
    authorized: true,
  },
  Events: {
    path: "/events",
    authorized: true,
  },
  Elections: {
    path: "/elections",
    authorized: true,
  },
  Circumscriptions: {
    path: "/circumscriptions",
    authorized: true,
  },
  SubscriptionLists: {
    path: "/subscription-lists",
    authorized: true,
  },
  UserManagement: {
    path: "/user-management",
    authorized: true,
  },
  RoleManagement: {
    path: "/role-management",
    authorized: true,
  },
  PermissionManagement: {
    path: "/permission-management",
    authorized: true,
  },
  VoterManagement: {
    path: "/voter-management",
    authorized: true,
  },
  Regions: {
    path: "/regions",
    authorized: true,
  },
  DesignationSubjects: {
    path: "/designation-subjects",
    authorized: true,
  },
  RegionTypes: {
    path: "/region-types",
    authorized: true,
  },
  ElectionTypes: {
    path: "/election-types",
    authorized: true,
  },
  ElectionFunction: {
    path: "/election-function",
    authorized: true,
  },
  ElectionStatus: {
    path: "/election-status",
    authorized: true,
  },
  SubscriptionListStatus: {
    path: "/subscription-list-status",
    authorized: true,
  },
  Genders: {
    path: "/genders",
    authorized: true,
  },
  Workflows: {
    path: "/workflows",
    authorized: true,
  },
  PoliticalParties: {
    path: "/political-parties",
    authorized: true,
  },
  Audit: {
    path: "/audit",
    authorized: true,
  },
  Public: {
    path: "/public",
  },
};
