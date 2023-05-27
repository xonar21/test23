import React, { useMemo, useState, useEffect, useRef } from "react";
import { AgGridReact } from "ag-grid-react";
import { GridApi } from "ag-grid-community";
import "ag-grid-enterprise";
import "ag-grid-community/styles/ag-grid.css";
import "ag-grid-community/styles/ag-theme-alpine.css";
import { Box, Button, Popper, Paper, Select, MenuItem, FormControl, Typography } from "@mui/material";
import UploadFileIcon from "@mui/icons-material/UploadFile";
import MenuIcon from "@mui/icons-material/Menu";
import { useTranslation } from "next-i18next";
import ReactDOM from "react-dom";
import { FilterAltOffOutlined, KeyboardArrowDown } from "@mui/icons-material";

interface AgGridTableProps {
  containerStyle: any;
  additionalProps?: any;
  setGridApi?: any;
  source?: any;
  clickRowData?: any;
  isAutoSizeColumns?: boolean;
  setGridRef?: any;
  setSize?: any;
  getPage?: any;
}

const styles = {
  groupButtonAction: {
    display: "flex",
    marginLeft: "20px",
    position: "absolute",
    zIndex: "1",
    right: "0",
    top: "-42px",
  },
  buttonOther: {
    textTransform: "none",
    backgroundColor: "rgba(0, 48, 92, 1)",
    color: "white",
    display: "flex",
    alignItems: "center",
    width: "100px",
    height: "40px",
  },
  buttonAction: {
    textTransform: "none",
    backgroundColor: "white",
    border: "1px solid rgba(0, 48, 92, 1)",
    color: "rgba(0, 48, 92, 1)",
    display: "flex",
    alignItems: "center",
    width: "max-content",
    height: "32px",
    "&:hover": {
      backgroundColor: "rgba(0, 48, 92, 1)",
      color: "white",
    },
  },
  buttonActionAdditional: {
    textTransform: "none",
    backgroundColor: "white",
    border: "1px solid rgba(0, 48, 92, 1)",
    color: "rgba(0, 48, 92, 1)",
    display: "flex",
    alignItems: "center",
    width: "max-content",
    height: "32px",
    "&:hover": {
      backgroundColor: "rgba(0, 48, 92, 1)",
      color: "white",
    },
    marginLeft: "10px",
  },
};

const AgGridTable: React.FC<AgGridTableProps> = ({
  containerStyle,
  additionalProps,
  setGridApi,
  source,
  clickRowData,
  setGridRef,
  setSize,
  getPage,
}) => {
  const { t } = useTranslation();
  const gridRef = useRef<AgGridReact>(null);
  const gridStyle = useMemo(() => ({}), []);
  const [api, setApi] = useState<GridApi | null>(null);
  const [anchorEl, setAnchorEl] = useState<HTMLElement | null>(null);
  const popperRef = useRef<HTMLDivElement>(null);
  const [pageSize, setPageSize] = useState(10);
  const [page, setPage] = useState(10);
  const pageSizeSelectContainer = useRef<HTMLDivElement | null>(null);

  const handleResetFilters = () => {
    if (api) {
      api.setFilterModel(null);
    }
  };
  const handleExportExcel = () => {
    if (api) {
      api.exportDataAsExcel();
    }
  };
  const handleExportCSV = () => {
    if (api) {
      api.exportDataAsCsv();
    }
  };

  const refreshTable = () => {
    if (api) {
      api.refreshServerSide();
    }
  };

  const handleOpenPopper = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClickOutsidePopper = (event: MouseEvent) => {
    if (popperRef.current && !popperRef.current.contains(event.target as HTMLElement)) {
      setAnchorEl(null);
    }
  };

  const handlePageSizeChange = (event: React.ChangeEvent<{ value: unknown }>) => {
    const newPageSize = event.target.value as number;
    setPageSize(newPageSize);
    if (api) {
      api.paginationSetPageSize(newPageSize);
    }
  };

  useEffect(() => {
    if (api) {
      refreshTable();
    }
  }, [api, clickRowData]);

  useEffect(() => {
    if (setSize) {
      setSize(pageSize);
    }
  }, [pageSize]);

  useEffect(() => {
    if (getPage) {
      getPage(page);
    }
  }, [page]);

  useEffect(() => {
    window.addEventListener("mousedown", handleClickOutsidePopper);
    return () => {
      window.removeEventListener("mousedown", handleClickOutsidePopper);
    };
  }, []);
  const CustomSelect = () => {
    const [value, setValue] = useState(pageSize);

    const handleChange = (event: any) => {
      handlePageSizeChange(event);
      setValue(event.target.value);
    };

    return (
      <FormControl variant="outlined" size="small">
        <Select
          value={value}
          onChange={handleChange}
          inputProps={{
            name: "rows-per-page",
            id: "rows-per-page",
          }}
          sx={{
            fontSize: "12px",
            fontWeight: "700",
            ".MuiOutlinedInput-input": {
              border: "1px solid #7c93b3",
            },
          }}
          IconComponent={KeyboardArrowDown}
        >
          <MenuItem value={10}>10</MenuItem>
          <MenuItem value={50}>50</MenuItem>
          <MenuItem value={100}>100</MenuItem>
        </Select>
      </FormControl>
    );
  };

  return (
    <>
      <div style={containerStyle}>
        {source ? (
          <Box sx={styles.groupButtonAction}>
            <Button
              variant="contained"
              color="primary"
              sx={{ ...styles.buttonAction, width: "174px", paddingLeft: "10px", paddingRight: "10px" }}
              onClick={handleResetFilters}
            >
              <FilterAltOffOutlined sx={{ marginRight: "6px", fontSize: "20px" }} />
              <Typography sx={{ fontSize: "16px", lineHeight: "19px", fontWeight: "normal" }}>
                {t("resetFilters")}
              </Typography>
            </Button>
            <Button
              variant="contained"
              color="primary"
              sx={{ ...styles.buttonActionAdditional, width: "174px", paddingLeft: "14px", paddingRight: "14px" }}
              onClick={handleOpenPopper}
            >
              <MenuIcon sx={{ marginRight: "6px", fontSize: "20px" }} />
              <Typography sx={{ fontSize: "16px", lineHeight: "19px", fontWeight: "normal" }}>
                {t("additional")}
              </Typography>
            </Button>
          </Box>
        ) : (
          <></>
        )}

        <div style={gridStyle} className="ag-theme-alpine">
          <AgGridReact
            {...additionalProps}
            ref={gridRef}
            class="my-ag-grid-table"
            onGridReady={params => {
              setApi(params.api);
              if (gridRef) {
                setTimeout(() => {
                  gridRef.current?.api.forEachNode(node => {
                    if (node.key) {
                      gridRef.current?.api.setRowNodeExpanded(node, true);
                    }
                  });
                }, 500);
              }
              if (setGridApi) {
                setGridApi(params.api);
              }
              if (source) {
                params.api.setServerSideDatasource(source);
                params.api.addEventListener("paginationChanged", (e: any) => {
                  const currentPage = Math.floor(e.api.getFirstDisplayedRow() / pageSize);
                  setPage(currentPage);
                  params.api.purgeInfiniteCache();
                });
              }
            }}
            onFirstDataRendered={params => {
              if (!source) {
                return;
              }

              const paginationPanel = document.querySelector(".ag-paging-panel");

              const selectContainer = document.createElement("div");
              selectContainer.style.display = "inline-block";
              selectContainer.style.marginLeft = "10px";

              if (paginationPanel) {
                paginationPanel.insertBefore(selectContainer, paginationPanel.firstChild);
              }

              pageSizeSelectContainer.current = selectContainer;
              if (pageSizeSelectContainer.current) {
                ReactDOM.render(<CustomSelect />, pageSizeSelectContainer.current);
              }
            }}
          />
        </div>

        <Popper open={Boolean(anchorEl)} anchorEl={anchorEl} ref={popperRef}>
          <Paper
            sx={{
              padding: "10px",
              display: "grid",
              gap: "10px",
              backgroundColor: "white",
              boxShadow: "1px 6px 32px rgba(0, 0, 0, 0.2)",
            }}
          >
            <Button
              variant="contained"
              color="primary"
              sx={{ ...styles.buttonAction, width: "174px", paddingLeft: "14px", paddingRight: "14px" }}
              onClick={handleExportExcel}
            >
              <UploadFileIcon sx={{ marginRight: "6px", fontSize: "20px", marginBottom: "2px" }} />
              <Typography sx={{ fontSize: "16px", lineHeight: "19px", fontWeight: "normal" }}>Export Excel</Typography>
            </Button>
            <Button
              variant="contained"
              color="primary"
              sx={{ ...styles.buttonAction, width: "174px", paddingLeft: "14px", paddingRight: "14px" }}
              onClick={handleExportCSV}
            >
              <UploadFileIcon sx={{ marginRight: "6px", fontSize: "20px", marginBottom: "2px" }} />
              <Typography sx={{ fontSize: "16px", lineHeight: "19px", fontWeight: "normal" }}>Export CSV</Typography>
            </Button>
          </Paper>
        </Popper>
      </div>

      <style>
        {`
          .ag-theme-alpine {
            --ag-font-size: 12px;
            --ag-font-family: 'Roboto';
            --ag-data-color: #00305C;
            --ag-header-foreground-color: #00305C;
            --ag-odd-row-background-color: #F8F8F8;
            --ag-header-background-color: #F8F8F8;
            --ag-row-border-color: transparent;
            --ag-border-color: #7c93b3;
            --ag-row-border-width: 1px;
            --ag-border-width: 1px;
            --ag-border-style: solid;
            --ag-border-radius: 0px;
            --ag-header-column-resize-handle-height: 50%;
            height: 520px;
          }
          .ag-root-wrapper-body {
            height: 470px !important;
          }
          .ag-theme-alpine .ag-popup {
            height: auto;
          }
          .ag-header {
            border: none;
          }

          .ag-filter-select {
            display: none;
          }

          .ag-theme-alpine .ag-icon-menu {
            font-size: 20px;
            color: rgba(0, 48, 92, 1);
          }

          .ag-header-cell-text {
            white-space: normal;
            line-height: 1.2;
            padding-top: 4px;
            padding-bottom: 4px;
          }

          .ag-theme-alpine .ag-body-horizontal-scroll-viewport {
            height: 16px;
          }

          .ag-theme-alpine .ag-body-horizontal-scroll-viewport::-webkit-scrollbar {
            height: 4px;
          }

          .ag-theme-alpine .ag-body-horizontal-scroll-viewport::-webkit-scrollbar-track {
            background-color: transparent;
            border-radius: 4px;
          }

          .ag-theme-alpine .ag-body-horizontal-scroll-viewport::-webkit-scrollbar-thumb {
            background-color: #D4D7D9;
            border-radius: 4px;
          }

          .ag-theme-alpine .ag-body-horizontal-scroll-viewport::-webkit-scrollbar-thumb:hover {
            background-color: rgba(0, 48, 92, 0.8);
          }

          @keyframes fadeIn {
            0% {
              opacity: 0;
            }
            100% {
              opacity: 1;
            }
          }

          .ag-theme-alpine {
            animation: fadeIn 0.5s ease-in-out;
          }

          .ag-row-hover {
            transition: background-color 1s ease-in-out;
          }
          .ag-theme-alpine .ag-row-hover {
            background-color: rgba(0, 48, 92, 0.2);
          }
          .ag-horizontal-right-spacer {
            overflow: hidden;
            border-left: none !important;
            // width: 76px !important;
            // max-width: 76px !important;
            // min-width: 76px !important;
          }

          .ag-theme-alpine  .ag-body-vertical-scroll {
            width: 4px !important;
            max-width: 4px !important;
            min-width: 4px !important;
            position: absolute;
            right: 0;
          }
          .ag-theme-alpine  .ag-body-vertical-scroll-viewport {
            width: 4px !important;
            max-width: 4px !important;
            min-width: 4px !important;

          }
          .ag-theme-alpine .ag-body-vertical-scroll-viewport::-webkit-scrollbar {
            width: 4px;
          }

          .ag-theme-alpine .ag-body-vertical-scroll-viewport::-webkit-scrollbar-track {
            background-color: transparent;
            border-radius: 4px;
          }

          .ag-theme-alpine .ag-body-vertical-scroll-viewport::-webkit-scrollbar-thumb {
            background-color: #D4D7D9;
            border-radius: 4px;
          }

          .ag-theme-alpine .ag-body-vertical-scroll-viewport::-webkit-scrollbar-thumb:hover {
            background-color: rgba(0, 48, 92, 0.8);
          }

          .ag-theme-alpine .ag-pinned-right-header {
            width: 76px !important;
            max-width: 76px !important;
            min-width: 76px !important;
          }
          .ag-theme-alpine .ag-details-row {
            padding: 0 !important;
            // height: 50px !important;
          }
        `}
      </style>
    </>
  );
};

export default AgGridTable;
