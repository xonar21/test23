import React from "react";
import { SubscriptionListStatusActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { ISubscriptionListStatusCreate, ISubscriptionListStatusPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";

const subscriptionListStatus: NextPage = () => {
  const { hasPermission, loading } = usePermissions();

  const subscriptionListStatusFunctionRowDataPath = (res: any) => {
    console.log(res);
    return res.payload.items;
  };

  const isActionPermissions = () => {
    if (!loading) {
      return (
        hasPermission(Permission.UpdateSubscriptionListStatus) || hasPermission(Permission.DeleteSubscriptionListStatus)
      );
    }
  };
  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateSubscriptionListStatus);
    }
  };
  const isDeletePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.DeleteSubscriptionListStatus);
    }
  };

  const paramsRequest = (number?: number, size?: number, filters?: object, sortField?: string, sortOrder?: string) => {
    return {
      number,
      size,
      filters,
      sortField,
      sortOrder,
    };
  };
  return (
    <>
      {!loading ? (
        <UniversalTable<ISubscriptionListStatusPreview, ISubscriptionListStatusCreate>
          getData={SubscriptionListStatusActions.getSubscriptionListStatus}
          putItem={SubscriptionListStatusActions.updateSubscriptionListStatus}
          deleteItem={SubscriptionListStatusActions.deleteSubscriptionListStatus}
          tableConfig={defaultConfig.subscriptionListStatus}
          isAction={isActionPermissions()}
          isEdit={isEditPermissions()}
          isDelete={isDeletePermissions()}
          dialogContentStyle={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "15px" }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={subscriptionListStatusFunctionRowDataPath}
          isAutoSizeColumns={true}
          pinnedAction={"right"}
        />
      ) : (
        <></>
      )}
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

export default subscriptionListStatus;
