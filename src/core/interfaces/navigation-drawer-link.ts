import React from "react";
import { IApplicationRoute } from "~/shared";

export interface INavigationDrawerLink {
  icon: React.ElementType;
  title: string;
  route: IApplicationRoute;
}
