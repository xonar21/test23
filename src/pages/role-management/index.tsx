import React, { useEffect } from "react";

import { RolesActions, PermissionsSelectors, PermissionsActions } from "~/store";
import { useDispatch, useSelector } from "react-redux";
import { GetServerSideProps, NextPage } from "next";
import { IRoleCreate, IRolePreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";

const RoleManagement: NextPage = () => {
  const dispatch = useDispatch();
  const { hasPermission, loading } = usePermissions();
  const { data }: any = useSelector(PermissionsSelectors.getRoot);

  const rolesFunctionRowDataPath = (res: any) => {
    return res.payload.items;
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

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateRole) || hasPermission(Permission.DeleteRole);
    }
  };
  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateRole);
    }
  };

  useEffect(() => {
    dispatch(PermissionsActions.getPermissions());
  }, []);
  return (
    <>
      {!loading ? (
        <UniversalTable<IRolePreview, IRoleCreate>
          getData={RolesActions.getRoles}
          putItem={RolesActions.updateRoles}
          tableConfig={defaultConfig.roles}
          isAction={isActionPermissions()}
          isEdit={isEditPermissions()}
          modifyRowDataPath={rolesFunctionRowDataPath}
          selector={data}
          paramsRequest={paramsRequest}
          dialogContentStyle={{
            display: "grid",
            gridTemplateColumns: "1fr",
            gap: "15px",
          }}
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

export default RoleManagement;
