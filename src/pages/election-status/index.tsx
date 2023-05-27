import React from "react";
import { SubscriptionListStatusActions, ElectionStatusActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import {
  ISubscriptionListStatusCreate,
  ISubscriptionListStatusPreview,
  IElectionStatusPreview,
  IElectionStatusUpdate,
} from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";

const electionStatus: NextPage = () => {
  const { hasPermission, loading } = usePermissions();

  const electionStatusFunctionRowDataPath = (res: any) => {
    console.log(res);
    return res.payload.items;
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateElectionStatus);
    }
  };
  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateElectionStatus);
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
        <UniversalTable<IElectionStatusPreview, IElectionStatusUpdate>
          getData={ElectionStatusActions.getElectionStatus}
          putItem={ElectionStatusActions.updateElectionStatus}
          tableConfig={defaultConfig.electionStatus}
          isAction={isActionPermissions()}
          isEdit={isEditPermissions()}
          dialogContentStyle={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "15px" }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={electionStatusFunctionRowDataPath}
          isAutoSizeColumns={true}
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

export default electionStatus;
