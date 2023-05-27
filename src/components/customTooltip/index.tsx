import React, { useMemo } from "react";
import { ColDef, ITooltipParams } from "ag-grid-community";
import { Box } from "@mui/material";

type CustomTooltipParams = ITooltipParams & {
  color: string;
  colDef: ColDef;
  shouldShowTooltip: boolean;
  rowData: any;
};
export default (props: CustomTooltipParams) => {
  const data = useMemo(() => props.api.getDisplayedRowAtIndex(props.rowIndex!)!.data, []);

  if (props && props.column) {
    if ("colId" in props.column) {
      const cellSelector = `.ag-row[row-id="${props.rowIndex}"] .ag-cell[col-id="${props.column.getColId()}"]`;
      const cellElement = document.querySelector(cellSelector);
      if (cellElement && cellElement) {
        if (cellElement.scrollWidth === cellElement.clientWidth) {
          return null;
        }
      }
    }
  }

  const subProperties = (data: any, props: any) => {
    const [firstField, secondField] = props.colDef.field.split(".");

    if (secondField) {
      return data[firstField][secondField];
    }

    return data[firstField];
  };
  return (
    <>
      {props.colDef.field ? (
        <Box
          sx={{
            backgroundColor: props.color || "white",
            border: "1px solid rgba(0, 48, 92, 1)",
            minWidth: "100px",
            borderRadius: "3px",
            padding: "5px",
            transition: "all 1s linear",
            maxWidth: "300px",
            whiteSpace: "pre-wrap !important",
            overflowWrap: "break-word !important",
            boxSizing: "border-box !important",
          }}
        >
          {subProperties(data, props)}
          {/* {data[props.colDef.field]} */}
        </Box>
      ) : (
        <></>
      )}
    </>
  );
};
