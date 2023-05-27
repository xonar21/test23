import React, { useEffect } from "react";
import { SelectListsActions, SubscriptionListActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { ISubscriptionListsPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";
import { useDispatch } from "react-redux";
import { useTranslation } from "next-i18next";

const subscriptionList: NextPage = () => {
  const { hasPermission, loading } = usePermissions();
  const dispatch = useDispatch();
  const { t } = useTranslation();

  const subscriptionListRowDataPath = (res: any) => {
    return res.payload.items;
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateSubscriptionList) || hasPermission(Permission.DeleteSubscriptionList);
    }
  };
  const isAddPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.CreateSubscriptionList);
    }
  };
  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateSubscriptionList);
    }
  };
  const isDeletePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.DeleteSubscriptionList);
    }
  };
  const isActivatePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.ActivateSubscriptionList);
    }
  };
  const isDeactivatePermissions = () => {
    if (!loading) {
      return hasPermission(Permission.StopSubscriptionList);
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

  useEffect(() => {
    const fetchData = async () => {
      dispatch(SelectListsActions.getGenders(paramsRequest(1, 20)));
      dispatch(SelectListsActions.getElectionFunction(paramsRequest(1, 20)));
      dispatch(SelectListsActions.getSubscriptionListStatus(paramsRequest(1, 20)));
      dispatch(SelectListsActions.getElections(paramsRequest(1, 20)));
      dispatch(SelectListsActions.getPoliticalParties(paramsRequest(1, 20)));
    };

    fetchData();
  }, []);

  const customActivateValue = (id: any) => {
    return {
      subscriptionListId: id,
      hash: "string",
    };
  };

  const customDeactivateValue = (id: any) => {
    return {
      subscriptionListId: id,
      hash: "string",
      cancellationReason: "string",
    };
  };
  return (
    <>
      {!loading ? (
        <UniversalTable<ISubscriptionListsPreview, any>
          getData={SubscriptionListActions.getSubscriptionList}
          postItem={SubscriptionListActions.createSubscriptionList}
          putItem={SubscriptionListActions.updateSubscriptionList}
          deleteItem={SubscriptionListActions.deleteSubscriptionList}
          tableConfig={defaultConfig.subscriptionList}
          isAdd={isAddPermissions()}
          isAction={isActionPermissions()}
          isEdit={isEditPermissions()}
          isDelete={isDeletePermissions()}
          isActivate={isActivatePermissions()}
          isDeactivate={isDeactivatePermissions()}
          activateItem={SubscriptionListActions.subscriptionListActivate}
          deactivateItem={SubscriptionListActions.subscriptionListDeactivate}
          customLabelActivate={t("activater")}
          customLabelDeactivate={t("stoper")}
          customActivateValue={customActivateValue}
          customDeactivateValue={customDeactivateValue}
          dialogContentStyle={{
            display: "grid",
            gridTemplateColumns: "1fr 1fr",
            gap: "15px",
            paddingTop: "5px !important",
          }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={subscriptionListRowDataPath}
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

export default subscriptionList;
