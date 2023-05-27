import { NextPage } from "next";
import { useDispatch, useSelector } from "react-redux";
import { SubscriptionListActions, SubscriptionListSelectors } from "~/store";
import { useEffect, useMemo, useState } from "react";
import { useTranslation } from "next-i18next";
import React from "react";
import AgGridTable from "~/pageList/components/AgGrid/AgGridTable";
import { ColDef } from "ag-grid-community";
import customTooltip from "~/components/customTooltip";
import { Box } from "@mui/material";

const SubscriptionArhived: NextPage = () => {
  const list = useSelector(SubscriptionListSelectors.getRoot);
  const [rowData, setData] = useState<any>(list.data);
  const dispatch = useDispatch();
  const { t } = useTranslation(["common"]);

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
  ]);

  useEffect(() => {
    dispatch(SubscriptionListActions.getSubscriptionListForSigningArchived(paramsRequest(1, 10000)));
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

  useEffect(() => {
    setData(list.data);
  }, [list]);

  return (
    <>
      <Box sx={{ marginTop: "70px" }}>
        <AgGridTable containerStyle={containerStyle} additionalProps={additionalProps} />
      </Box>
    </>
  );
};

export default SubscriptionArhived;
