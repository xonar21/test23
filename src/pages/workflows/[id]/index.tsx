import React, { useEffect, useState } from "react";
import { WorkFlowsActions, WorkFlowsSelectors, RolesActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IWorkFlowsPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";
import { useDispatch, useSelector } from "react-redux";
import { useTranslation } from "next-i18next";
import { styleButton } from "~/pageList/styles/defaultStyleButtonForDialog";
import { Box, Button, Container, TextField } from "@mui/material";
import TabBar from "~/components/tab-bar";
import { useRouter } from "next/router";
import { SaveOutlined } from "@mui/icons-material";
import { Transitions } from "~/components/transitions";
import SuccessModal from "~/components/successModal";
import ReactDOM from "react-dom";

const workFlows: NextPage = () => {
  const { t } = useTranslation();
  const { hasPermission, loading } = usePermissions();
  const router = useRouter();
  const dispatch = useDispatch();
  const { data } = useSelector(WorkFlowsSelectors.getRoot);

  const [dataWorkFlow, setDataWorkFlow] = useState({
    code: "",
  });

  const [custom, setCustom] = useState({});

  const showSuccessModal = (title: string, message: string) => {
    const successModal = document.createElement("div");
    document.body.appendChild(successModal);

    ReactDOM.render(React.createElement(SuccessModal, { title, message }), successModal);
  };

  const statusesRowDataPath = (res: any) => {
    return res.payload.responsePayload;
  };

  const paramsRequest = (number?: number, size?: number, filters?: object, sortField?: string, sortOrder?: string) => {
    return {
      workFlowId: router.query.id,
      number,
      size,
      filters,
      sortField,
      sortOrder,
    };
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateSubscriptionListStatus);
    }
  };

  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateSubscriptionListStatus);
    }
  };

  useEffect(() => {
    dispatch(WorkFlowsActions.getWorkflows());
    dispatch(RolesActions.getRoles());
    dispatch(WorkFlowsActions.getWorkFlowTransitions(paramsRequest()));
  }, []);

  const handleSave = () => {
    dispatch(WorkFlowsActions.updateWorkflow({ entities: [dataWorkFlow] })).then((e: any) => {
      if (e.succeeded) {
        showSuccessModal("Succes", "Numele fluxului de lucru a fost actualizat cu succes");
      }
    });
  };

  useEffect(() => {
    data.forEach(async (e: any) => {
      if (router.query.id === e.id) {
        setCustom({ id: router.query.id, entityType: e.entityType });
        setDataWorkFlow(e);
      }
    });
  }, [data]);

  const customPutValue = (value: any) => {
    return { workflowId: router.query.id, stateId: value.id, stateName: value.code };
  };

  const tabs = [
    {
      key: "workflow",
      label: t("workflow"),
      content: (
        <Container
          className="fadeIn"
          sx={{
            border: "1px solid #00305C",
            maxWidth: "none !important",
            borderRadius: "10px",
            boxShadow: "0px 10px 15px #ccc",
            display: "grid",
            gridTemplateColumns: "1fr 1fr 1fr",
            gridTemplateRows: "max-content max-content",
            gap: "00px",
            paddingTop: "24px",
            paddingBottom: "24px",
            marginTop: "40px",
          }}
        >
          <Box sx={{ display: "flex", flexDirection: "column", gap: "16px" }}>
            <TextField
              label={t("name")}
              key={"code"}
              value={dataWorkFlow.code}
              variant="outlined"
              onChange={e => setDataWorkFlow({ ...dataWorkFlow, ["code"]: e.target.value })}
            />
          </Box>
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              gap: "16px",
              gridColumnStart: "3",
              gridRowStart: "2",
              justifyContent: "end",
              alignItems: "end",
            }}
          >
            <Button onClick={handleSave} color="primary" sx={styleButton.buttonConfirm}>
              <SaveOutlined sx={{ marginRight: "5px" }} />
              {t("save")}
            </Button>
          </Box>
        </Container>
      ),
    },
    {
      key: "statuses",
      label: t("statuses"),
      content: (
        <>
          <UniversalTable<IWorkFlowsPreview, any>
            key={"statuses"}
            getData={WorkFlowsActions.getWorkflowStatus}
            putItem={WorkFlowsActions.updateWorkflowStatus}
            tableConfig={defaultConfig.workFlowsStatus}
            modifyRowDataPath={statusesRowDataPath}
            paramsRequest={paramsRequest}
            isAction={isActionPermissions()}
            isEdit={isEditPermissions()}
            customPutValue={customPutValue}
            customData={custom}
          />
        </>
      ),
    },
    {
      key: "transitions",
      label: t("transitions"),
      content: (
        <>
          <Transitions key={"transitions"} />
        </>
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

export default workFlows;
