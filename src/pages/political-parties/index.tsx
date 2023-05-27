import React from "react";
import { PoliticalPartiesActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IPoliticalPartiesCreate, IPoliticalPartiesPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";

const politicalParties: NextPage = () => {
  const { hasPermission, loading } = usePermissions();

  const politicalPartiesRowDataPath = (res: any) => {
    return res.payload.items;
  };

  const customDeactivateValue = (data: any) => {
    return {
      partiesId: [data],
    };
  };

  const customActivateValue = (data: any) => {
    return {
      partiesId: [data],
    };
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(
        Permission.UpdatePoliticalParty ||
          Permission.DeletePoliticalParty ||
          Permission.DeactivatePolticalPartyItems ||
          Permission.ActivatePoliticalPartyItems,
      );
    }
  };

  const isAddPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.CreatePoliticalParty);
    }
  };

  const isDeletePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.DeletePoliticalParty);
    }
  };

  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdatePoliticalParty);
    }
  };

  const isDeactivatePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.DeactivatePolticalPartyItems);
    }
  };

  const isActivatePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.ActivatePoliticalPartyItems);
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
        <UniversalTable<IPoliticalPartiesPreview, IPoliticalPartiesCreate>
          getData={PoliticalPartiesActions.getPoliticalParties}
          putItem={PoliticalPartiesActions.updatePoliticalParties}
          postItem={PoliticalPartiesActions.createPoliticalParties}
          deleteItem={PoliticalPartiesActions.deletePoliticalParties}
          deactivateItem={PoliticalPartiesActions.deactivatePoliticalParties}
          activateItem={PoliticalPartiesActions.activatePoliticalParties}
          customActivateValue={customActivateValue}
          customDeactivateValue={customDeactivateValue}
          tableConfig={defaultConfig.politicalParties}
          isAction={isActionPermissions()}
          isAdd={isAddPermissions()}
          isDelete={isDeletePermissions()}
          isEdit={isEditPermissions()}
          isActivate={isActivatePermissions()}
          isDeactivate={isDeactivatePermissions()}
          dialogContentStyle={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "15px" }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={politicalPartiesRowDataPath}
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

export default politicalParties;
