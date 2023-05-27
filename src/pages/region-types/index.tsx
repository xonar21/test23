import React from "react";
import { RegionTypesActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IRegionTypePreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { Permission } from "~/security";
import { usePermissionRedirect, usePermissions } from "~/hooks";
import { getRoutePath, routes } from "~/shared";
import { defaultConfig } from "~/configs/page-config";
import UniversalTable from "~/components/wrapper-page";

const RegionType: NextPage = () => {
  usePermissionRedirect([Permission.ViewRegionTypeList], getRoutePath(routes.Home));

  const { hasPermission, loading } = usePermissions();

  const regionsTypesFunctionRowDataPath = (res: any) => {
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
        <UniversalTable<IRegionTypePreview, IRegionTypePreview>
          getData={RegionTypesActions.getRegionTypes}
          putItem={RegionTypesActions.updateRegionTypes}
          tableConfig={defaultConfig.regionTypes}
          isAction={isActionPermissions()}
          isEdit={isEditPermissions()}
          dialogContentStyle={{
            display: "grid",
            gridTemplateColumns: "1fr 1fr",
            gap: "15px",
          }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={regionsTypesFunctionRowDataPath}
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

export default RegionType;
