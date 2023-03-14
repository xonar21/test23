import { ComponentType } from "react";
import { NextPage } from "next";
import { AppProps } from "next/app";

export type NextPageWithLayout = NextPage & {
  layout?: ComponentType;
};

export type AppPropsWithLayout = AppProps & {
  Component: NextPageWithLayout;
};
