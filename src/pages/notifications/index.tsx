import React, { useContext, useEffect, useMemo, useState } from "react";
import {
  CheckboxSelectionCallbackParams,
  ColDef,
  HeaderCheckboxSelectionCallbackParams,
  GridApi,
  IRowNode,
} from "ag-grid-community";
import Button from "@mui/material/Button";
import { GetServerSideProps, NextPage } from "next";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";

import AgGridTable from "~/pageList/components/AgGrid/AgGridTable";
import { useDispatch } from "react-redux";
import { NotificationsActions } from "~/store";
import { useTranslation } from "next-i18next";
import dayjs from "dayjs";
import customTooltip from "~/components/customTooltip";
import { Box, Checkbox, Typography } from "@mui/material";

const SelectedCountContext = React.createContext(0);
const SizeCountContext = React.createContext(10);
const PageContext = React.createContext(0);

const CustomHeader = (params: any) => {
  const [checked, setChecked] = useState(false);
  const [indeterminate, setIndeterminate] = useState(false);
  const selectedCount = useContext(SelectedCountContext);
  const sizeCount = useContext(SizeCountContext);
  const page = useContext(PageContext);
  const totalRowsCount = params.api.getRenderedNodes();
  const [currentPage, setCurrentPage] = useState(0);
  useEffect(() => {
    if (params.api?.getCacheBlockState()) {
      // setChecked(false);
      // const totalLoadedRowCount: any = Object.values(params.api?.getCacheBlockState()).reduce(
      //   (total, block: any) => total + block.loadedRowCount,
      //   0,
      // );

      const pageSize = sizeCount;
      const firstRow = params.api.getFirstDisplayedRow();

      const currentPage = Math.floor(firstRow / pageSize);
      setCurrentPage(currentPage);

      const startRow = currentPage * pageSize;
      const endRow = startRow + pageSize;

      const currentRowCount = params.api
        .getRenderedNodes()
        .filter((node: any) => node.rowIndex >= startRow && node.rowIndex < endRow).length;

      if (selectedCount < currentRowCount && selectedCount !== 0) {
        setIndeterminate(true);
        setChecked(false);
      } else if (selectedCount === 0) {
        setIndeterminate(false);
        setChecked(false);
      } else if (selectedCount === currentRowCount) {
        setIndeterminate(false);
        setChecked(true);
      }

      // if (selectedCount < totalLoadedRowCount && selectedCount !== 0) {
      //   setIndeterminate(true);
      //   setChecked(false);
      // } else if (selectedCount === 0) {
      //   setIndeterminate(false);
      //   setChecked(false);
      // } else if (selectedCount === totalLoadedRowCount) {
      //   setIndeterminate(false);
      //   setChecked(true);
      // }
    }
  }, [selectedCount, totalRowsCount, sizeCount]);

  useEffect(() => {
    setIndeterminate(false);
    setChecked(false);
  }, [page]);

  const onCheckboxChange = () => {
    setChecked(!checked);
    params.api.deselectAll();

    const pageSize = sizeCount;
    const startRow = currentPage * pageSize;
    const endRow = startRow + pageSize;

    params.api.forEachNode((node: any) => {
      if (node.data && node.rowIndex >= startRow && node.rowIndex < endRow) {
        node.setSelected(!checked);
      }
    });
  };

  return (
    <Box sx={{ display: "flex", alignItems: "center", marginLeft: "-2px" }}>
      <Checkbox
        onChange={onCheckboxChange}
        indeterminate={indeterminate}
        checked={checked}
        sx={{ padding: "0", marginRight: "7px", ".MuiSvgIcon-root": { fontSize: "20px !important" } }}
      />
      <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "700" }}>{params.displayName}</Typography>
    </Box>
  );
};

function isFirstColumn(params: CheckboxSelectionCallbackParams | HeaderCheckboxSelectionCallbackParams) {
  const displayedColumns = params.columnApi.getAllDisplayedColumns();
  const thisIsFirstColumn = displayedColumns[0] === params.column;
  return thisIsFirstColumn;
}

const Notifications: NextPage = () => {
  const { t } = useTranslation(["common"]);
  const dispatch = useDispatch();
  const containerStyle = useMemo(() => styles.containerStyle, []);
  const [selectedCount, setSelectedCount] = useState(0);
  const [gridApi, setGridApi] = useState<GridApi>();
  const [size, setSize] = useState(10);
  const [page, getPage] = useState(0);

  const formatUsingDayjs = (date: string) => {
    if (date) {
      return dayjs(date).format("DD-MM-YYYY");
    }
    return "";
  };

  const [columnDefs] = useState<ColDef[]>([
    {
      field: "eventName",
      headerName: t("eventName"),
      flex: 1,
      tooltipField: "eventName",
      filter: "agTextColumnFilter",
      headerComponentFramework: CustomHeader,
      showDisabledCheckboxes: true,
    },
    {
      field: "properties.message",
      headerName: t("message"),
      flex: 6,
      tooltipField: "properties.message",
      filter: "agTextColumnFilter",
    },
    {
      field: "dateSent",
      headerName: t("dateSent"),
      flex: 1,
      filter: "agTextColumnFilter",
      valueFormatter: (params: any) => formatUsingDayjs(params.value),
    },
    {
      field: "isRead",
      hide: true,
    },
  ]);

  const updateSelectedCount = () => {
    if (gridApi) {
      const selectedNodes = gridApi.getSelectedNodes();

      setSelectedCount(selectedNodes.length);
    }
  };

  const getRowStyle = (params: any) => {
    if (params.data) {
      if (!params.data.isRead) {
        return { background: "rgba(0, 48, 92, 0.4)" };
      }
      return null;
    }
  };

  const defaultColDef = useMemo<ColDef>(() => {
    return {
      minWidth: 150,
      sortable: true,
      filter: true,
      resizable: true,
      tooltipComponent: customTooltip,
      checkboxSelection: isFirstColumn,
      headerCheckboxSelection: true,
      headerCheckboxSelectionFilteredOnly: true,
      filterParams: {
        inRangeInclusive: true,
        suppressAndOrCondition: true,
        suppressOperators: true,
      },
    };
  }, []);

  const isRowSelectable = useMemo(() => {
    return (params: IRowNode) => {
      return !params.data.isRead;
    };
  }, []);

  const additionalProps = {
    rowModelType: "serverSide",
    columnDefs: columnDefs,
    defaultColDef: defaultColDef,
    animateRows: true,
    suppressMenuHide: true,
    rowSelection: "multiple",
    suppressRowClickSelection: true,
    getRowStyle: getRowStyle,
    isRowSelectable: isRowSelectable,
    onSelectionChanged: updateSelectedCount,
    pagination: true,
    paginationPageSize: 10,
    cacheBlockSize: 10,
    tooltipShowDelay: 500,
  };

  const handleMarkAsRead = async () => {
    if (!gridApi) {
      return;
    }

    const selectedNodes = gridApi.getSelectedNodes();
    const selectedData: any = [];

    selectedNodes.forEach(node => {
      if (!node.data.isRead) {
        selectedData.push(node.data.id);
      }
    });
    const response = await dispatch(NotificationsActions.updatetNotifications(selectedData));

    if (response.succeeded) {
      await dispatch(NotificationsActions.getNotificationsCount());
      gridApi.refreshServerSide();
      setSelectedCount(0);
    }
  };

  const paramsRequest = (number?: number, size?: number, filters?: any, sortField?: string, sortOrder?: string) => {
    return {
      number,
      size,
      filters,
      sortField,
      sortOrder,
    };
  };

  useEffect(() => {
    dispatch(NotificationsActions.getNotifications(paramsRequest()));
    dispatch(NotificationsActions.getNotificationsCount());
  }, []);

  type FilterObject = {
    [key: string]: {
      filterType: string;
      type: string;
      filter: string;
    };
  };

  type ResultObject = {
    fieldName: string;
    values: string[];
  }[];

  const createArrayFromObject: (obj: FilterObject) => ResultObject = obj => {
    const result: ResultObject = [];

    for (const key in obj) {
      if (Object.prototype.hasOwnProperty.call(obj, key)) {
        result.push({
          fieldName: key,
          values: [obj[key].filter],
        });
      }
    }

    return result;
  };

  const serverSideDatasource = {
    getRows: async (params: any) => {
      const startRow = params.request.startRow;
      const endRow = params.request.endRow;
      const pageSize = endRow - startRow;
      const pageNumber = startRow / pageSize + 1;
      const filterModel = params.request.filterModel;
      const sortModel = params.request.sortModel;
      const sort = sortModel[0] ? sortModel[0].sort : "asc";
      const colId = sortModel[0] ? sortModel[0].colId : "isRead";
      try {
        const response: any = dispatch(
          NotificationsActions.getNotifications(
            paramsRequest(pageNumber, pageSize, JSON.stringify(createArrayFromObject(filterModel)), colId, sort),
          ),
        );
        setTimeout(function () {
          response.then((e: any) => {
            if (e.succeeded) {
              params.success({
                rowData: e.payload.items,
                rowCount: e.payload.totalCount,
              });
            }
          });
        }, 500);
      } catch (error) {
        params.fail();
      }
    },
  };

  return (
    <>
      <PageContext.Provider value={page}>
        <SelectedCountContext.Provider value={selectedCount}>
          <SizeCountContext.Provider value={size}>
            <Button sx={styles.buttonFilter} onClick={handleMarkAsRead} disabled={selectedCount === 0}>
              Marcați ca citit
            </Button>

            <AgGridTable
              setGridApi={setGridApi}
              containerStyle={containerStyle}
              additionalProps={additionalProps}
              source={serverSideDatasource}
              setSize={setSize}
              getPage={getPage}
            />
          </SizeCountContext.Provider>
        </SelectedCountContext.Provider>
      </PageContext.Provider>
    </>
  );
};

const styles = {
  textSign: {
    color: "rgba(85, 183, 213, 1)",
    fontSize: "12px",
    margin: "0px",
    textDecoration: "underline",
  },
  imgSign: {
    alignSelf: "center",
    paddingBottom: "2px",
  },
  containerStyle: {
    marginLeft: "20px",
    marginRight: "20px",
    marginBottom: "10px",
    position: "relative",
  },
  notRead: {
    background: "rgba(0, 48, 92, 0.4)",
  },
  buttonFilter: {
    background: "rgba(0, 48, 92, 1)",
    borderRadius: "3px",
    color: "white",
    textTransform: "none",
    margin: "0 0 10px 20px",
    "&:hover": {
      backgroundColor: "rgba(0, 48, 92, 1)",
    },
    "&:disabled": {
      backgroundColor: "rgba(211, 211, 211, 1)",
      color: "rgba(169, 169, 169, 1)",
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

export default Notifications;
