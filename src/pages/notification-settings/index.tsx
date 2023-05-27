import { GetServerSideProps, NextPage } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import i18nConfig from "@i18n";
import SubscribeToNotification from "~/components/SubscribeToNotification";
import React, { useEffect } from "react";
import { useTranslation } from "next-i18next";
import NotificationSettingsEvents from "~/components/NotificationSettingsEvents";
import TabBar from "~/components/tab-bar";
import { useDispatch, useSelector } from "react-redux";
import { VoterActions, VoterSelectors } from "~/store";

const NotificationSettings: NextPage = () => {
  const { t } = useTranslation(["common"]);
  const dispatch = useDispatch();
  const voterProfile = useSelector(VoterSelectors.getRoot);

  useEffect(() => {
    dispatch(VoterActions.getVoterProfileOwn());
  }, []);

  const isVoterEmail = () => {
    if (voterProfile.data) {
      if (voterProfile.data.email !== "") {
        return <NotificationSettingsEvents />;
      }
      return <></>;
    }
    return <></>;
  };

  const tabs = [
    {
      key: "subscribeToNotification",
      label: t("subscribeToNotification"),
      content: <SubscribeToNotification />,
    },
    {
      key: "notificationSettingsEvents",
      label: voterProfile.data ? (voterProfile.data.email !== "" ? t("notificationSettingsEvents") : "") : "",
      content: <>{isVoterEmail()}</>,
    },
  ];
  return <>{voterProfile.data ? <TabBar items={tabs} /> : <></>}</>;
};

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};

export default NotificationSettings;
