import React from "react";
import { GetServerSideProps } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import i18nConfig from "@i18n";
import { NextPageWithLayout } from "~/core";
import { RedirectLayout } from "~/layouts";

const LoginSaise: NextPageWithLayout = () => {
  return <></>;
};

LoginSaise.layout = RedirectLayout;

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};

export default LoginSaise;
