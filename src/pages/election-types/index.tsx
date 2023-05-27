import React, { useCallback, useEffect } from "react";
import { ElectionTypesActions, UsersActions, UsersSelectors } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IElectionTypesCreate, IElectionTypesPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";
import { useDispatch, useSelector } from "react-redux";

const electionTypes: NextPage = () => {
  const { hasPermission, loading } = usePermissions();
  const dispatch = useDispatch();
  const { data } = useSelector(UsersSelectors.getRoot);

  const electionTypesRowDataPath = (res: any) => {
    return res.payload.items;
  };

  const customDeactivateValue = (data: any) => {
    return [data];
  };

  const customActivateValue = (data: any) => {
    return [data];
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(
        Permission.UpdateElectionType ||
          Permission.DeleteElectionType ||
          Permission.DeactivateElectionTypes ||
          Permission.ActivateElectionTypes,
      );
    }
  };

  const isAddPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.CreateElectionType);
    }
  };

  const isDeletePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.DeleteElectionType);
    }
  };

  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateElectionType);
    }
  };

  const isDeactivatePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.DeactivateElectionTypes);
    }
  };

  const isActivatePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.ActivateElectionTypes);
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

  const customFormat = useCallback(
    (value: any) => {
      let userName = "";
      data.externalUsers.items.forEach((item: any) => {
        if (item.id === value.value) {
          userName = item.userName;
        }
      });
      return userName;
    },
    [data.externalUsers.items],
  );

  useEffect(() => {
    dispatch(UsersActions.getExternalUsers(paramsRequest()));
  }, []);

  return (
    <>
      {!loading && data.externalUsers.items ? (
        <UniversalTable<IElectionTypesPreview, IElectionTypesCreate>
          getData={ElectionTypesActions.getElectionTypes}
          putItem={ElectionTypesActions.updateElectionType}
          postItem={ElectionTypesActions.createElectionType}
          deleteItem={ElectionTypesActions.deleteElectionType}
          deactivateItem={ElectionTypesActions.deactivateElectionType}
          activateItem={ElectionTypesActions.activateElectionType}
          customActivateValue={customActivateValue}
          customDeactivateValue={customDeactivateValue}
          tableConfig={defaultConfig.electionTypes}
          isAction={isActionPermissions()}
          isAdd={isAddPermissions()}
          isDelete={isDeletePermissions()}
          isEdit={isEditPermissions()}
          isActivate={isActivatePermissions()}
          isDeactivate={isDeactivatePermissions()}
          dialogContentStyle={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "15px" }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={electionTypesRowDataPath}
          customFormat={customFormat}
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

export default electionTypes;
