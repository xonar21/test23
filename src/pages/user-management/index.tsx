import React from "react";
import { UsersActions } from "~/store";
import i18nConfig from "@i18n";
import { NextPage, GetServerSideProps } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";
import { IUsersPreview } from "~/models";
import TabBar from "~/components/tab-bar";
import { useTranslation } from "next-i18next";

const UserManagement: NextPage = () => {
  const { t } = useTranslation(["common"]);

  const usersRowDataPath = (res: any) => {
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

  const tabs = [
    {
      label: t("externalUsers"),
      content: (
        <UniversalTable<IUsersPreview, any>
          key={"externalUsers"}
          getData={UsersActions.getExternalUsers}
          tableConfig={defaultConfig.users}
          modifyRowDataPath={usersRowDataPath}
          paramsRequest={paramsRequest}
        />
      ),
    },
    {
      label: t("internalUsers"),
      content: (
        <UniversalTable<IUsersPreview, any>
          key={"internalUsers"}
          getData={UsersActions.getInternalUsers}
          tableConfig={defaultConfig.users}
          modifyRowDataPath={usersRowDataPath}
          paramsRequest={paramsRequest}
        />
      ),
    },
  ];

  return (
    <>
      <TabBar items={tabs} />
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
