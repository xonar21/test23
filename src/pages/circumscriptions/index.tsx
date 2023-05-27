import React, { useMemo, useState, useEffect } from "react";
import { Box, FormControl, InputLabel, MenuItem, Select, TextField, Typography } from "@mui/material";
import { ColDef } from "ag-grid-community";
import AgGridTable from "~/pageList/components/AgGrid/AgGridTable";
import { ModeEditOutlined, MoreHoriz } from "@mui/icons-material";
import DefaultDialogAction from "~/pageList/components/dialogs/DefaultDialogAction";
import PopoverModal from "~/pageList/components/Popover";
import { GetServerSideProps, NextPage } from "next";
import { ICircumscriptionPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { dataCircumscriptions } from "~/pageList/data/dataActive";

const Circumscriptions: NextPage = () => {
  const containerStyle = useMemo(
    () => ({ marginLeft: "20px", marginRight: "20px", marginBottom: "10px", position: "relative" }),
    [],
  );
  const [clickRowData, setClickRowData] = useState<Partial<ICircumscriptionPreview>>({});

  const [formData, setFormData] = useState({
    id: "",
    idCircumscripțieDinSA: "",
    denumireRo: "",
    denumireRu: "",
    număr: "",
    scrutin: "",
    regiune: "",
  });
  const [rowData] = useState<ICircumscriptionPreview[]>(dataCircumscriptions);
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [openModal, setOpenModal] = useState(false);
  const [columnDefs, setColumnDefs] = useState<ColDef[]>([
    { field: "denumireRo", headerName: "Denumire Ro", flex: 3 },
    { field: "număr", headerName: "Număr", flex: 3 },
    { field: "scrutin", headerName: "Scrutin", flex: 3 },
    { field: "regiune", headerName: "Regiune", flex: 3 },
  ]);

  useEffect(() => {
    const actionColumn: ColDef = {
      field: "action",
      headerName: "Acțiuni",
      flex: 1,
      cellRendererFramework: (params: { node: any; data: any }) => {
        if (params.node.group) {
          return null;
        }

        return (
          <Box
            sx={{ height: "24px" }}
            onClick={e => {
              handleClick(e, params);
            }}
          >
            <MoreHoriz />
          </Box>
        );
      },
    };

    setColumnDefs(prevColumnDefs => [...prevColumnDefs, actionColumn]);
  }, []);

  const defaultColDef = useMemo<ColDef>(() => {
    return {
      flex: 1,
      minWidth: 150,
      sortable: true,
      filter: true,
      resizable: true,
    };
  }, []);

  const additionalProps = {
    rowData: rowData,
    columnDefs: columnDefs,
    defaultColDef: defaultColDef,
    domLayout: "autoHeight",
    showOpenedGroup: false,
    animateRows: true,
    suppressMenuHide: true,
  };

  const handleClick = (event: React.MouseEvent<HTMLElement>, params: { node: any; data: any }) => {
    setAnchorEl(event.currentTarget);
    setClickRowData(params.data);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleAddClose = () => {
    setOpenModal(false);
  };

  const handleAddOpen = () => {
    const { id, idCircumscripțieDinSA, denumireRo, denumireRu, număr, scrutin, regiune } = clickRowData;
    setOpenModal(true);
    setFormData({
      ...formData,
      id: id as string,
      idCircumscripțieDinSA: idCircumscripțieDinSA as string,
      denumireRo: denumireRo as string,
      denumireRu: denumireRu as string,
      număr: număr as string,
      scrutin: scrutin as string,
      regiune: regiune as string,
    });

    handleClose();
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };
  return (
    <>
      <Box sx={{ paddingTop: "50px" }}>
        <AgGridTable containerStyle={containerStyle} additionalProps={additionalProps} />
      </Box>

      <PopoverModal
        anchorEl={anchorEl}
        handleClose={handleClose}
        content={
          <>
            <Box sx={styles.boxPopover} onClick={handleAddOpen}>
              <ModeEditOutlined sx={styles.editIcon} />

              <Typography sx={styles.textSign}>Editează</Typography>
            </Box>
          </>
        }
      />

      <DefaultDialogAction
        openModal={openModal}
        handleClose={handleAddClose}
        dynamicTitle={"Editează"}
        styledContent={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "15px", paddingTop: "10px !important" }}
        content={
          <>
            <TextField
              value={formData.id}
              onChange={handleInputChange}
              name="id"
              label="Id"
              fullWidth
              margin="normal"
              disabled
              sx={{ margin: "0" }}
            />
            <TextField
              value={formData.idCircumscripțieDinSA}
              onChange={handleInputChange}
              name="idCircumscripțieDinSA"
              label="Id Circumscripție Din SA"
              fullWidth
              margin="normal"
              disabled
              sx={{ margin: "0" }}
            />
            <TextField
              value={formData.denumireRo}
              onChange={handleInputChange}
              name="denumireRo"
              label="Denumire Ro"
              fullWidth
              margin="normal"
              sx={{ margin: "0" }}
            />
            <TextField
              value={formData.denumireRu}
              onChange={handleInputChange}
              name="denumireRu"
              label="Denumire Ru"
              fullWidth
              margin="normal"
              sx={{ margin: "0" }}
            />
            <TextField
              value={formData.număr}
              onChange={handleInputChange}
              name="număr"
              label="Număr"
              fullWidth
              margin="normal"
              sx={{ margin: "0" }}
            />
            <FormControl sx={styles.selectField} fullWidth>
              <InputLabel id="demo-simple-select-label">Regiune</InputLabel>
              <Select
                value={formData.regiune}
                MenuProps={{ sx: styles.selectMenu }}
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                label="Regiune"
                name="regiune"
                fullWidth
              >
                <MenuItem value="option2">Ocnița</MenuItem>
              </Select>
            </FormControl>
            <FormControl
              sx={{
                ...styles.selectField,
                gridColumn: "span 2",
              }}
              fullWidth
            >
              <InputLabel id="demo-simple-select-label">Scrutin</InputLabel>
              <Select
                value={formData.scrutin}
                MenuProps={{ sx: styles.selectMenu }}
                labelId="demo-simple-select-label"
                id="demo-simple-select"
                label="Scrutin"
                name="scrutin"
                fullWidth
              >
                <MenuItem value="option1">Alegeri prezidențiale din 01.11.2024</MenuItem>
              </Select>
            </FormControl>
          </>
        }
      />
    </>
  );
};

const styles = {
  textSign: {
    color: "rgba(0, 48, 92, 1)",
    fontSize: "12px",
    margin: "0px",
  },
  boxPopover: {
    padding: 1,
    width: 150,
    height: 40,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
  },
  editIcon: {
    color: "rgba(0, 48, 92, 1)",
    fontSize: "20px",
    marginBottom: "4px",
  },
  addButton: {
    margin: "20px 0 7px 20px",
    color: "white",
    textTransform: "none",
    backgroundColor: "rgba(0, 48, 92, 1)",
    border: "1px solid rgba(0, 48, 92, 1)",
    "&:hover": {
      backgroundColor: "white",
      color: "rgba(0, 48, 92, 1)",
    },
  },
  selectMenu: {
    "& ul": {
      backgroundColor: "white",
      borderTop: "none",
      borderRadius: "4px",
      marginTop: 0,
      minWidth: "auto",
      color: "black",
    },
    "& li.Mui-selected": {
      backgroundColor: "rgba(182, 185, 192, 1)",
    },
    "& li.Mui-selected:hover": {
      backgroundColor: "rgba(182, 185, 192, 1)",
    },
    "& li:hover": {
      backgroundColor: "rgba(233, 235, 236, 1)",
    },
    "& .MuiListItem-root": {
      fontSize: "14px",
    },
  },
  selectField: {
    margin: "0px",
    "& label.Mui-focused": {
      color: "rgba(0, 48, 92, 1)",
    },
    ".MuiInputBase-input": {
      color: "black",
    },
    ".MuiFormLabel-root": {
      color: "rgba(182, 185, 192, 1)",
    },
    "& .MuiOutlinedInput-root": {
      "& fieldset": {
        borderColor: "rgba(182, 185, 192, 1)",
      },
      "&:hover fieldset": {
        borderColor: "rgba(69, 69, 69, 1)",
      },
      "&.Mui-focused fieldset": {
        borderColor: "rgba(0, 48, 92, 1)",
      },
    },
    ".MuiPaper-root": {
      color: "green",
    },
  },
};

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};

export default Circumscriptions;
