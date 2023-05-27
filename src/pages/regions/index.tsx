import React from "react";
import { RegionsActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IRegionPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { Permission } from "~/security";
import { usePermissionRedirect, usePermissions } from "~/hooks";
import { getRoutePath, routes } from "~/shared";
import { defaultConfig } from "~/configs/page-config";
import UniversalTable from "~/components/wrapper-page";

const Regions: NextPage = () => {
  usePermissionRedirect([Permission.ViewRegionList], getRoutePath(routes.Home));

  const { hasPermission, loading } = usePermissions();

  const regionsFunctionRowDataPath = (res: any) => {
    return res.payload.items;
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateRegion);
    }
  };

  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateRegion);
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
        <UniversalTable<IRegionPreview, IRegionPreview>
          getData={RegionsActions.getRegions}
          putItem={RegionsActions.updateRegions}
          tableConfig={defaultConfig.regions}
          isAction={isActionPermissions()}
          isEdit={isEditPermissions()}
          dialogContentStyle={{
            display: "grid",
            gridTemplateColumns: "1fr 1fr",
            gap: "15px",
            paddingTop: "5px !important",
            ".MuiFormControl-marginNormal": { margin: "0px" },
          }}
          modifyRowDataPath={regionsFunctionRowDataPath}
          paramsRequest={paramsRequest}
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

export default Regions;
