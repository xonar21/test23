import React, { useCallback, useEffect, useState } from "react";
import { PermissionsActions, WorkFlowsActions, WorkFlowsSelectors, RolesActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IElectionImport, IWorkFlowsPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";
import { useDispatch, useSelector } from "react-redux";
import { getRoutePath, routes } from "~/shared";

const workFlows: NextPage = () => {
  const { hasPermission, loading } = usePermissions();
  const dispatch = useDispatch();
  const { data, workflowsStatus } = useSelector(WorkFlowsSelectors.getRoot);
  const [statusWorkflows, setStatusWorkflows] = useState<any>({});

  const electionRowDataPath = (res: any) => {
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

  useEffect(() => {
    const fetchData = async () => {
      await Promise.all([
        dispatch(WorkFlowsActions.getWorkflows()),
        dispatch(PermissionsActions.getPermissions()),
        dispatch(RolesActions.getRoles()),
      ]);
    };

    fetchData();
  }, [dispatch]);

  useEffect(() => {
    if (data) {
      data.forEach(async (e: any) => {
        if (e.id) {
          await dispatch(WorkFlowsActions.getWorkflowStatus({ id: e.id, entityType: e.entityType }));
        }
      });
    }
  }, [data]);

  const customFormat = useCallback(
    (data: any) => {
      if (data.column.colId === "workflowStates" && data.value.length !== 0) {
        const values: string[] = [];
        data.value.map(async (e: any) => {
          statusWorkflows[data.data.entityType].map((el: any) => {
            if (el.id === e.stateId) {
              values.push(el.code);
            }
          });
        });
        return values.join(", ");
      }
      return [];
    },
    [data.value, statusWorkflows],
  );

  useEffect(() => {
    if (workflowsStatus) {
      setStatusWorkflows(workflowsStatus);
    }
  }, [workflowsStatus]);
  return (
    <>
      {!loading && Object.keys(statusWorkflows).length === data.length ? (
        <UniversalTable<IWorkFlowsPreview, IElectionImport>
          getData={WorkFlowsActions.getWorkflows}
          tableConfig={defaultConfig.workFlows}
          isAction={isActionPermissions()}
          isView={isViewPermissions()}
          dialogContentStyle={{
            display: "grid",
            gridTemplateColumns: "1fr",
            gap: "15px",
            paddingTop: "5px !important",
          }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={electionRowDataPath}
          pathRedirectToView={getRoutePath(routes.Workflows)}
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

export default workFlows;
