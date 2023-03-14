import React from "react";
import { GetServerSideProps } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import i18nConfig from "@i18n";
import { NextPageWithLayout } from "~/core";
import { AuthLayout } from "~/layouts";
import { AuthForm } from "~/components";

const Register: NextPageWithLayout = () => {
  return <AuthForm type="register" />;
};

Register.layout = AuthLayout;

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};

export default Register;
