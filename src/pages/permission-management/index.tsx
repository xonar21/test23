import React from "react";
import { PermissionsActions } from "~/store";
import i18nConfig from "@i18n";
import { NextPage, GetServerSideProps } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";
import { IPermissionPreview } from "~/models";

const PermissionManagement: NextPage = () => {
  const premissionsFunctionRowDataPath = (res: any) => {
    return res.payload;
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
      <UniversalTable<IPermissionPreview, any>
        getData={PermissionsActions.getPermissions}
        tableConfig={defaultConfig.permissions}
        modifyRowDataPath={premissionsFunctionRowDataPath}
        paramsRequest={paramsRequest}
      />
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

export default PermissionManagement;
