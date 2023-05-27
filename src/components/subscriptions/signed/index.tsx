import { NextPage } from "next";
import { useDispatch, useSelector } from "react-redux";
import { SubscriptionListActions, SubscriptionListSelectors } from "~/store";
import { useEffect, useMemo, useState } from "react";
import { useTranslation } from "next-i18next";
import SuccessModal from "~/components/successModal";
import ReactDOM from "react-dom";
import React from "react";
import AgGridTable from "~/pageList/components/AgGrid/AgGridTable";
import { ColDef } from "ag-grid-community";
import customTooltip from "~/components/customTooltip";
import { Box, Button, DialogActions, Typography } from "@mui/material";
import { DoNotDisturbAltOutlined } from "@mui/icons-material";
import DefaultDialogAction from "~/pageList/components/dialogs/DefaultDialogAction";
import dayjs from "dayjs";

const SubscriptionSigned: NextPage = () => {
  const list = useSelector(SubscriptionListSelectors.getRoot);
  const [rowData, setData] = useState<any>(list.data);
  const dispatch = useDispatch();
  const { t } = useTranslation(["common"]);
  const [openModal, setOpenModal] = useState(false);
  const [contetntSign, setContetntSign] = useState<any>({});

  const containerStyle = useMemo(() => ({ marginBottom: "10px", position: "relative" }), []);

  const [columnDefs] = useState<ColDef[]>([
    { field: "electionTypeNameRo", rowGroup: true, hide: true },
    { field: "ballotFunctionNameRo", headerName: t("ballotFunctionName"), flex: 1 },
    { field: "electionNameRo", headerName: t("electionName"), flex: 2, tooltipField: "electionNameRo" },
    {
      field: "circumscriptionNameRo",
      headerName: t("circumscriptionName"),
      flex: 2,
      tooltipField: "circumscriptionNameRo",
    },
    { field: "subscriptionListNameRo", headerName: t("nameRo"), flex: 1 },
    {
      field: "politicalPartyNameRo",
      headerName: t("politicalPartyName"),
      flex: 1,
      tooltipField: "politicalPartyNameRo",
    },
    { field: "candidateProfessionRo", headerName: t("professionRo"), flex: 1 },
    {
      field: "action",
      headerName: t("sign"),
      width: 150,
      suppressMenu: true,
      cellRenderer: (params: any) => {
        if (params.node.group) {
          return null;
        }

        return (
          <Box
            sx={{
              height: "100%",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
            }}
            onClick={e => {
              handleClickPopover(e, params);
            }}
          >
            <DoNotDisturbAltOutlined sx={{ color: "#55B7D5", fontSize: "20px" }} />
            <Typography
              sx={{
                textDecoration: "underline",
                color: "#55B7D5",
                fontSize: "12px",
                lineHeight: "14px",
                fontWeight: "normal",
              }}
            >
              {t("unsubscribe")}
            </Typography>
          </Box>
        );
      },
    },
  ]);

  const handleClickPopover = (e: any, params: any) => {
    setOpenModal(true);
    setContetntSign(params.data);
  };

  const dynamicTitle = () => {
    return "";
  };

  const handleCloseModal = () => {
    setOpenModal(false);
  };

  useEffect(() => {
    dispatch(SubscriptionListActions.getSubscriptionListForSigningActive(paramsRequest(1, 10000)));
  }, []);

  const paramsRequest = (number?: number, size?: number, filters?: object, sortField?: string, sortOrder?: string) => {
    return {
      number,
      size,
      filters,
      sortField,
      sortOrder,
    };
  };

  const defaultColDef = useMemo<ColDef>(() => {
    return {
      sortable: true,
      filter: true,
      resizable: true,
      tooltipComponent: customTooltip,
      filterParams: {
        inRangeInclusive: true,
        suppressAndOrCondition: true,
        suppressOperators: true,
      },
    };
  }, []);

  const autoGroupColumnDef = useMemo<ColDef>(() => {
    return {
      headerName: t("electionTypeNameRo"),
      flex: 2.5,
      suppressMenu: true,
      tooltipField: "electionTypeNameRo",
    };
  }, []);

  const additionalProps = {
    rowData: rowData,
    columnDefs: columnDefs,
    defaultColDef: defaultColDef,
    autoGroupColumnDef: autoGroupColumnDef,
    showOpenedGroup: false,
    animateRows: true,
    suppressMenuHide: true,
    tooltipShowDelay: 500,
  };

  const showSuccessModal = (title: string, message: string) => {
    const successModal = document.createElement("div");
    document.body.appendChild(successModal);

    ReactDOM.render(React.createElement(SuccessModal, { title, message }), successModal);
  };

  const handleSave = () => {
    showSuccessModal("Succes", "Ați reușit să vă anulați cu succes votul");
    setOpenModal(false);
  };

  useEffect(() => {
    setData(list.data);
  }, [list]);

  const formatForSign = (date: string) => {
    if (date) {
      return dayjs(date).format("YYYY");
    }
    return "";
  };

  return (
    <>
      <Box sx={{ marginTop: "70px" }}>
        <AgGridTable containerStyle={containerStyle} additionalProps={additionalProps} />
      </Box>

      <DefaultDialogAction
        openModal={openModal}
        handleClose={handleCloseModal}
        dynamicTitle={dynamicTitle}
        customActions={
          <DialogActions sx={{ padding: "10px 40px 20px", justifyContent: "center" }}>
            <Button
              onClick={handleSave}
              sx={{
                backgroundColor: "#00305C",
                color: "white",
                border: "1px solid #00305C",
                "&:hover": { color: "#00305C", backgroundColor: "white" },
              }}
            >
              <Typography sx={{ fontSize: "16px", lineHeight: "19px", fontWeight: "normal" }}>
                {t("confirm")}
              </Typography>
            </Button>
          </DialogActions>
        }
        content={
          <>
            <Typography
              sx={{
                fontSize: "12px",
                lineHeight: "14px",
                fontWeight: "bold",
                color: "#00305C",
                textAlign: "center",
                marginBottom: "20px",
              }}
            >
              {contetntSign.electionNameRo}
            </Typography>
            <Box>
              <Typography
                sx={{
                  fontSize: "12px",
                  lineHeight: "14px",
                  fontWeight: "normal",
                  color: "#00305C",
                  display: "flex",
                  marginBottom: "20px",
                }}
              >
                {t("signInSupport")}
                <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "bold", marginLeft: "5px" }}>
                  {contetntSign.subscriptionListNameRo}
                </Typography>
              </Typography>
              <Typography
                sx={{
                  fontSize: "12px",
                  lineHeight: "14px",
                  fontWeight: "normal",
                  color: "#00305C",
                  display: "flex",
                  marginBottom: "5px",
                }}
              >
                {t("candidateYearOfBirth")}
                <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "bold", marginLeft: "5px" }}>
                  {formatForSign(contetntSign.dateOfBirth)}
                </Typography>
              </Typography>
              <Typography
                sx={{
                  fontSize: "12px",
                  lineHeight: "14px",
                  fontWeight: "normal",
                  color: "#00305C",
                  display: "flex",
                  marginBottom: "5px",
                }}
              >
                {t("function")}
                <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "bold", marginLeft: "5px" }}>
                  {contetntSign.candidatePositionRo}
                </Typography>
              </Typography>
              <Typography
                sx={{
                  fontSize: "12px",
                  lineHeight: "14px",
                  fontWeight: "normal",
                  color: "#00305C",
                  display: "flex",
                  marginBottom: "5px",
                }}
              >
                {t("candidateProfession")}
                <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "bold", marginLeft: "5px" }}>
                  {contetntSign.candidateProfessionRo}
                </Typography>
              </Typography>
              <Typography
                sx={{
                  fontSize: "12px",
                  lineHeight: "14px",
                  fontWeight: "normal",
                  color: "#00305C",
                  display: "flex",
                  marginBottom: "5px",
                }}
              >
                {t("candidatePlaceOfWork")}
                <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "bold", marginLeft: "5px" }}>
                  {contetntSign.candidateWorkPlaceRo}
                </Typography>
              </Typography>
              <Typography
                sx={{
                  fontSize: "12px",
                  lineHeight: "14px",
                  fontWeight: "normal",
                  color: "#00305C",
                  display: "flex",
                  marginBottom: "40px",
                }}
              >
                {t("subjectDesignation")}
                <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "bold", marginLeft: "5px" }}>
                  {contetntSign.politicalPartyNameRo}
                </Typography>
              </Typography>
            </Box>
          </>
        }
      />
    </>
  );
};

export default SubscriptionSigned;
