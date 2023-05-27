import React from "react";
import { VoterActions } from "~/store";
import i18nConfig from "@i18n";
import { NextPage, GetServerSideProps } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";
import { IVoterProfilePreview } from "~/models";
import { Permission } from "~/security";
import { usePermissions } from "~/hooks";
import { getRoutePath, routes } from "~/shared";

const UserManagement: NextPage = () => {
  const { hasPermission, loading } = usePermissions();

  const votersRowDataPath = (res: any) => {
    return res.payload.items;
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.ViewVoterProfileItem);
    }
  };
  const isViewPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.ViewVoterProfileItem);
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
        <UniversalTable<IVoterProfilePreview, any>
          getData={VoterActions.getVoterProfiles}
          tableConfig={defaultConfig.voterProfilePerson}
          isAction={isActionPermissions()}
          isView={isViewPermissions()}
          pathRedirectToView={getRoutePath(routes.PersonalData)}
          modifyRowDataPath={votersRowDataPath}
          paramsRequest={paramsRequest}
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

export default UserManagement;
