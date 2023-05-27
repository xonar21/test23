import React, { useEffect } from "react";
import { GetServerSideProps, NextPage } from "next";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { useRouter } from "next/router";
import { CookieKeys, getRoutePath, routes } from "~/shared";
import { useCookie } from "react-use";

const Home: NextPage = () => {
  const router = useRouter();
  const [token] = useCookie(CookieKeys.SaiseToken);

  useEffect(() => {
    if (token) {
      router.replace(getRoutePath(routes.Elections));

      return;
    }

    router.replace(getRoutePath(routes.Subscriptions));
  });

  return <></>;
};

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common", "home"], i18nConfig)),
    },
  };
};

export default Home;
