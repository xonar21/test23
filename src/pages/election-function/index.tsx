import React from "react";
import { ElectionFunctionActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IElectionFunctionCreate, IElectionFunctionPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";

const electionFunction: NextPage = () => {
  const { hasPermission, loading } = usePermissions();

  const electionFunctionRowDataPath = (res: any) => {
    return res.payload.items;
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateElectionFunction) || hasPermission(Permission.DeleteElectionFunction);
    }
  };
  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateElectionFunction);
    }
  };
  const isDeletePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.DeleteElectionFunction);
    }
  };
  const isAddPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.CreateElectionFunction);
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
        <UniversalTable<IElectionFunctionPreview, IElectionFunctionCreate>
          getData={ElectionFunctionActions.getElectionFunctions}
          postItem={ElectionFunctionActions.createElectionFunction}
          putItem={ElectionFunctionActions.updateElectionFunction}
          deleteItem={ElectionFunctionActions.deleteElectionFunction}
          tableConfig={defaultConfig.electionFunction}
          isAction={isActionPermissions()}
          isEdit={isEditPermissions()}
          isDelete={isDeletePermissions()}
          isAdd={isAddPermissions()}
          dialogContentStyle={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "15px" }}
          modifyRowDataPath={electionFunctionRowDataPath}
          paramsRequest={paramsRequest}
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

export default electionFunction;
