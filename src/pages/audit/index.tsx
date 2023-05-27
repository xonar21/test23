import React, { useMemo, useState, useEffect, useCallback, useRef } from "react";
import { Autocomplete, Box, CircularProgress, FormControl, TextField } from "@mui/material";
import { ColDef } from "ag-grid-community";
import AgGridTable from "~/pageList/components/AgGrid/AgGridTable";
import { GetServerSideProps, NextPage } from "next";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { useDispatch, useSelector } from "react-redux";
import { AuditActions, UsersActions, UsersSelectors } from "~/store";
import { useTranslation } from "next-i18next";
import customTooltip from "~/components/customTooltip";
import dayjs from "dayjs";

const Audit: NextPage = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const [gridApi, setGridApi] = useState<any>(null);
  const users: any = useSelector(UsersSelectors.getRoot);
  const containerStyle = useMemo(
    () => ({ marginLeft: "20px", marginRight: "20px", marginBottom: "10px", position: "relative" }),
    [],
  );

  const formatUsingDayjs = (date: string) => {
    if (date) {
      return dayjs(date).format("DD-MM-YYYY");
    }
    return "";
  };

  const [searchValue, setSearchValue] = useState("");
  const [page, setPage] = useState(1);
  const [value, setValue] = useState("");
  const [totalPages, setTotalPages] = useState(0);
  const [items, setItems] = useState<any>([]);
  const [loading] = useState(false);
  const currentValueRef: any = useRef(value);
  const [columnDefs] = useState<ColDef[]>([
    { field: "eventName", headerName: t("eventName"), flex: 1 },
    {
      field: "dateTime",
      headerName: t("dateTime"),
      flex: 1,
      valueFormatter: (params: any) => formatUsingDayjs(params.value),
    },
    { field: "message", headerName: t("message"), width: 750, tooltipField: "message" },
    { field: "userName", headerName: t("userName"), flex: 1 },
    { field: "entityPrimaryKey", headerName: t("entityPrimaryKey"), flex: 1 },
    { field: "entityType", headerName: t("entityType"), flex: 1 },
  ]);

  const defaultColDef = useMemo<ColDef>(() => {
    return {
      minWidth: 50,
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

  const additionalProps = {
    columnDefs: columnDefs,
    defaultColDef: defaultColDef,
    showOpenedGroup: false,
    animateRows: true,
    suppressMenuHide: true,
    pagination: true,
    paginationPageSize: 10,
    cacheBlockSize: 10,
    rowModelType: "serverSide",
    tooltipShowDelay: 500,
  };

  useEffect(() => {
    currentValueRef.current = value;
  }, [value]);

  const paramsRequest = useCallback(
    (number?: number, size?: number, filters?: object, sortField?: string, sortOrder?: string) => {
      return {
        number,
        size,
        filters,
        sortField,
        sortOrder,
      };
    },
    [value],
  );

  const serverSideDatasource = {
    getRows: async (params: any) => {
      const startRow = params.request.startRow;
      const endRow = params.request.endRow;
      const pageSize = endRow - startRow;
      const pageNumber = startRow / pageSize;

      try {
        const response: any = await dispatch(
          AuditActions.getAudit(paramsRequest(pageNumber, pageSize, currentValueRef.current.id)),
        );
        setTimeout(function () {
          if (response.succeeded && response.payload.count !== 0) {
            params.success({
              rowData: response.payload.events,
              rowCount: response.payload.count,
            });
            if (response.payload.count === 0 && gridApi) {
              gridApi.setServerSideDatasource(null);
            }
          } else {
            params.success({
              rowData: [{}],
            });
          }
        }, 500);
      } catch (error) {
        params.fail();
      }
    },
  };

  useEffect(() => {
    const fetchData = async () => {
      dispatch(UsersActions.getUsers(paramsRequest(1, 20)));
    };

    fetchData();
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      await dispatch(
        UsersActions.getUsers(
          paramsRequest(page, 20, {
            fieldName: "value",
            values: [searchValue],
          }),
        ),
      );
    };

    fetchData();
  }, [page, searchValue]);

  useEffect(() => {
    if (users.data.users.items) {
      setTotalPages(users.data.users.totalPages);
      setItems((prevItems: any) => [...prevItems, ...users.data.users.items]);
    }
  }, [users]);

  useEffect(() => {
    if (value) {
      if (gridApi) {
        gridApi.refreshServerSide();
      }
    }
  }, [value, gridApi]);

  return (
    <>
      <Box sx={{ paddingTop: "50px" }}>
        {items ? (
          <FormControl sx={{ marginBottom: "8px", marginLeft: "20px" }}>
            <Autocomplete
              id="demo-autocomplete"
              options={items}
              value={value}
              size="small"
              sx={{ width: "300px", height: "32px", ".MuiInputBase-root": { height: "32px" } }}
              getOptionLabel={(option: any) => {
                return option.userName ? option.userName : "";
              }}
              onChange={(e, newValue) => {
                if (newValue) {
                  setValue(newValue);
                }
              }}
              onInputChange={(event, val, reason) => {
                if (reason === "reset") {
                  setPage(1);
                }
                if (reason === "input") {
                  setSearchValue(val);
                  setPage(1);
                  setItems([]);
                }
                if (reason === "clear") {
                  setPage(1);
                  setSearchValue("");
                }
                if (val === "") {
                  setValue(" ");
                  setPage(1);
                  setSearchValue("");
                }
              }}
              ListboxProps={{
                onScroll: event => {
                  const target = event.target as HTMLUListElement;
                  const { scrollTop, scrollHeight, clientHeight } = target;
                  if (scrollHeight - scrollTop === clientHeight) {
                    setPage(prevPage => {
                      if (prevPage !== totalPages) {
                        return prevPage + 1;
                      }
                      return prevPage;
                    });
                  }
                },
              }}
              renderInput={params => (
                <TextField
                  {...params}
                  label={t(`users`)}
                  InputLabelProps={{ ...params.InputLabelProps, sx: { fontSize: "14px" } }}
                  InputProps={{
                    ...params.InputProps,
                    sx: {
                      fontSize: "14px",
                      lineHeight: "16px",
                      fontWeight: "normal",
                    },
                    endAdornment: (
                      <>
                        {loading ? <CircularProgress size={20} /> : null}
                        {params.InputProps.endAdornment}
                      </>
                    ),
                  }}
                />
              )}
              renderOption={(props, option: any) => (
                <li {...props} style={{ fontSize: "14px", lineHeight: "16px", fontWeight: "normal" }}>
                  {option.userName}
                </li>
              )}
            />
          </FormControl>
        ) : (
          <></>
        )}

        <AgGridTable
          containerStyle={containerStyle}
          additionalProps={additionalProps}
          source={serverSideDatasource}
          setGridApi={setGridApi}
        />
      </Box>
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

export default Audit;
