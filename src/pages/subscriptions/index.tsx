import { GetServerSideProps, NextPage } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import i18nConfig from "@i18n";
import { useTranslation } from "next-i18next";
import TabBar from "~/components/tab-bar";
import React from "react";
import SubscriptionActive from "~/components/subscriptions/active";
import SubscriptionSigned from "~/components/subscriptions/signed";
import SubscriptionArhived from "~/components/subscriptions/arhived";

const Subscriptions: NextPage = () => {
  const { t } = useTranslation(["common"]);

  const tabs = [
    {
      label: t("active"),
      content: <SubscriptionActive key={"active"} />,
    },
    {
      key: "signed",
      label: t("signed"),
      content: <SubscriptionSigned key={"signed"} />,
    },
    {
      key: "archived",
      label: t("archived"),
      content: <SubscriptionArhived key={"archived"} />,
    },
  ];

  return (
    <>
      <TabBar items={tabs} />
    </>
  );
};

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};

export default Subscriptions;
