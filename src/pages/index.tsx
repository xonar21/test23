import React from "react";
import { GetServerSideProps, NextPage } from "next";
import { Main, Cards } from "~/components";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";

const Home: NextPage = () => {
  return (
    <React.Fragment>
      <Main />
      <Cards />
    </React.Fragment>
  );
};

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common", "home"], i18nConfig)),
    },
  };
};

export default Home;
