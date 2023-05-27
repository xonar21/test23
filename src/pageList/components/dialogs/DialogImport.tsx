import React from "react";
import { AgGridReact } from "ag-grid-react";
import "ag-grid-community/styles/ag-grid.css";
import "ag-grid-community/styles/ag-theme-alpine.css";
import { Dialog, DialogContent, DialogActions, Button, Box } from "@mui/material";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import { styled } from "@mui/material/styles";

interface DialogImportProps {
  openDialog: boolean;
  handleCloseDialog: any;
  activeCheckboxes: any[];
  setTempCheckboxes: any;
  handleConfirmDialog: any;
  gridStyle: any;
  localData: any[];
  tempCheckboxes: any[];
  setGridApi: any;
  handleSelectionChanged: any;
}

const styles = {
  title: {
    color: "rgba(0, 48, 92, 1)",
    fontSize: "12px",
    fontWeight: "bold",
    margin: "0px",
  },
  subTable: {
    color: "rgba(212, 215, 217, 1)",
    fontSize: "12px",
    margin: "0px",
  },
  cancelButton: {
    textTransform: "none",
    color: "rgba(0, 48, 92, 1)",
    width: "120px",
    height: "32px",
    border: "1px solid rgba(0, 48, 94, 1);",
  },
  confirmButton: {
    textTransform: "none",
    backgroundColor: "rgba(0, 48, 92, 1)",
    color: "white",
    width: "120px",
    height: "32px",
  },
};

const DialogImport: React.FC<DialogImportProps> = ({
  openDialog,
  handleCloseDialog,
  activeCheckboxes,
  setTempCheckboxes,
  handleConfirmDialog,
  gridStyle,
  localData,
  tempCheckboxes,
  setGridApi,
  handleSelectionChanged,
}) => {
  const StyledTitleTable = styled("p")(styles.title);

  const StyledSubTable = styled("p")(styles.subTable);

  const modalColumnDefs = [
    {
      field: "checkbox",
      headerCheckboxSelection: true,
      checkboxSelection: true,
      headerCheckboxSelectionFilteredOnly: true,
      width: 50,
    },
    { field: "country", headerName: "Denumirea" },
    { field: "athlete", headerName: "Data petrecerii" },
  ];

  return (
    <Dialog
      open={openDialog}
      onClose={handleCloseDialog}
      PaperProps={{
        style: {
          width: "532px",
          backgroundColor: "white",
          borderRadius: "3px",
          padding: "40px",
        },
      }}
    >
      <DialogContent sx={{ backgroundColor: "white", padding: "0px" }}>
        <StyledTitleTable>Import scrutin</StyledTitleTable>

        <Box sx={{ display: "flex", marginBottom: "13px", marginTop: "8px" }}>
          <InfoOutlinedIcon sx={{ fontSize: "16px", color: styles.subTable.color, marginRight: "7px" }} />

          <StyledSubTable>Selectați scrutin pentru importare:</StyledSubTable>
        </Box>

        <div style={gridStyle} className="ag-theme-alpine">
          <AgGridReact
            rowData={localData}
            columnDefs={modalColumnDefs}
            domLayout={"autoHeight"}
            suppressRowClickSelection
            rowSelection="multiple"
            onFirstDataRendered={params => {
              params.api.forEachNode(node => {
                node.setSelected(tempCheckboxes[node.childIndex]);
              });
            }}
            onGridReady={params => {
              setGridApi(params.api);
            }}
            onSelectionChanged={handleSelectionChanged}
          />
        </div>
      </DialogContent>

      <DialogActions sx={{ marginTop: "20px" }}>
        <Button
          sx={styles.cancelButton}
          onClick={() => {
            setTempCheckboxes(activeCheckboxes);
            handleCloseDialog();
          }}
          color="primary"
        >
          Anulează
        </Button>

        <Box sx={{ flexGrow: 1 }} />

        <Button sx={styles.confirmButton} onClick={handleConfirmDialog} color="primary" variant="contained">
          Confirmă
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default DialogImport;
