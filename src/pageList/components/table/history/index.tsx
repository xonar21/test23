import React, { useMemo, useState } from "react";
import { Box, Typography } from "@mui/material";
import { ColDef } from "ag-grid-community";
import Image from "next/image";

import { IOlympicData } from "../interfaces";
import AgGridTable from "~/pageList/components/AgGrid/AgGridTable";
import { dataActive } from "~/pageList/data/dataActive";
import { styles } from "./styles";

const TableHistory = () => {
  const containerStyle = useMemo(() => styles.containerStyle, []);
  const [rowData, setRowData] = useState<IOlympicData[]>(dataActive);
  const [columnDefs, setColumnDefs] = useState<ColDef[]>([
    { field: "type", headerName: "Tip scrutin", rowGroup: true, hide: true },
    { field: "country", headerName: "Denumirea scrutinului" },
    { field: "athlete", headerName: "Funcția electivă" },
    { field: "year", headerName: "Circumscripția electorală" },
    { field: "gold", headerName: "Concurent electoral" },
    { field: "silver", headerName: "Subiect de desemnare" },
    { field: "bronze", headerName: "Data început" },
    { field: "total", headerName: "Data sfârșit" },
    {
      field: "action",
      headerName: "Semnătura",
      cellRendererFramework: (params: { node: any; data: any }) => {
        if (params.node.group) {
          return null;
        }

        const handleClick = () => {
          console.log("Данные строки:", params.data);
        };

        return (
          <Box sx={styles.groupSign} onClick={handleClick}>
            <Image src="/svg/sign.svg" alt="CEC Logo" width={20} height={20} />
            <Typography sx={styles.textSign}>Semnează</Typography>
          </Box>
        );
      },
    },
  ]);

  const defaultColDef = useMemo<ColDef>(() => {
    return {
      flex: 1,
      minWidth: 150,
      sortable: true,
      filter: true,
      resizable: true,
    };
  }, []);

  const autoGroupColumnDef = useMemo<ColDef>(() => {
    return {
      headerName: "Tip scrutin",
      minWidth: 300,
      cellRendererParams: {
        suppressCount: true,
      },
    };
  }, []);

  const additionalProps = {
    rowData: rowData,
    columnDefs: columnDefs,
    defaultColDef: defaultColDef,
    autoGroupColumnDef: autoGroupColumnDef,
    groupDisplayType: "singleColumn",
    domLayout: "autoHeight",
    showOpenedGroup: false,
    animateRows: true,
    suppressMenuHide: true,
  };

  return (
    <>
      <AgGridTable containerStyle={containerStyle} additionalProps={additionalProps} />
    </>
  );
};

export default TableHistory;
