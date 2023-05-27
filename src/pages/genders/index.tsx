import React from "react";
import { GendersActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IGendersPreview, IGendersUpdate } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";

const genders: NextPage = () => {
  const { hasPermission, loading } = usePermissions();

  const subscriptionListStatusFunctionRowDataPath = (res: any) => {
    console.log(res);
    return res.payload.items;
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateGender);
    }
  };
  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateGender);
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
        <UniversalTable<IGendersPreview, IGendersUpdate>
          getData={GendersActions.getGenders}
          putItem={GendersActions.updateGender}
          tableConfig={defaultConfig.genders}
          isAction={isActionPermissions()}
          isEdit={isEditPermissions()}
          dialogContentStyle={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "15px" }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={subscriptionListStatusFunctionRowDataPath}
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

export default genders;
