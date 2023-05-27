import React, { useCallback, useEffect, useMemo, useState } from "react";
import { ElectionActions, ElectionSelectors, SelectListsActions } from "~/store";
import { GetServerSideProps, NextPage } from "next";
import { IElectionImport, IElectionPreview } from "~/models";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { usePermissions } from "~/hooks";
import { Permission } from "~/security";
import UniversalTable from "~/components/wrapper-page";
import { defaultConfig } from "~/configs/page-config";
import { useDispatch, useSelector } from "react-redux";
import AgGridTable from "~/pageList/components/AgGrid/AgGridTable";
import { ColDef, RowSelectedEvent } from "ag-grid-community";
import { useTranslation } from "next-i18next";
import dayjs from "dayjs";
import { LinearProgress } from "@mui/material";
import ReactDOM from "react-dom";
import WarningModal from "~/components/warningModal";

const election: NextPage = () => {
  const { t } = useTranslation();
  const { hasPermission, loading } = usePermissions();
  const dispatch = useDispatch();
  const { data } = useSelector(ElectionSelectors.getRoot);
  const defaultContainerStyle = useMemo(() => styles.defaultStylesContainer, []);
  const [rowData, setRowData] = useState<any>(data);
  const [clickRowData, setClickRowData] = useState<any>({});
  const [count, setCount] = useState(0);
  const columnDefsFromConfig = useMemo(() => {
    return Object.values(defaultConfig.electionsSaise.tableHeaders).map((header: any) => ({
      field: header.colName,
      headerName: t(`${header.colName}`),
      flex: header.flex,
      valueFormatter: header.formatted ? (params: { value: string }) => formatUsingDayjs(params.value) : undefined,
    }));
  }, [defaultConfig.electionsSaise]);
  const [columnDefs] = useState<ColDef[]>(columnDefsFromConfig);

  const formatUsingDayjs = (date: string) => {
    if (date) {
      return dayjs(date).format("DD/MM/YYYY");
    }
    return "";
  };

  const electionRowDataPath = (res: any) => {
    setCount(res.payload.totalCount);
    return res.payload.items;
  };

  const isActionPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateElections);
    }
  };

  const isAddPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.ImportSaiseElections);
    }
  };

  const isEditPermissions = () => {
    if (!loading) {
      return hasPermission(Permission.UpdateElections);
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

  const onRowSelected = useCallback((event: RowSelectedEvent) => {
    setClickRowData({
      electionId: event.data.saElectionId,
    });
  }, []);

  const defaultColDef = useMemo<ColDef>(() => {
    return {
      flex: 1,
      minWidth: 150,
      sortable: true,
      filter: true,
      resizable: true,
      filterParams: {
        inRangeInclusive: true,
        suppressAndOrCondition: true,
        suppressOperators: true,
      },
    };
  }, []);

  const additionalProps = {
    rowData: rowData,
    columnDefs: columnDefs,
    defaultColDef: defaultColDef,
    rowSelection: "multiple",
    showOpenedGroup: false,
    animateRows: true,
    suppressMenuHide: true,
    onRowSelected: onRowSelected,
  };

  const handlerCustom = useCallback(
    (loading: any) => {
      return loading ? (
        <LinearProgress />
      ) : (
        <AgGridTable containerStyle={defaultContainerStyle} additionalProps={additionalProps} />
      );
    },
    [rowData],
  );

  const showWarningModal = (title: string, message: string, onConfirm: any) => {
    const warningModal = document.createElement("div");
    document.body.appendChild(warningModal);

    ReactDOM.render(React.createElement(WarningModal, { title, message, onConfirm }), warningModal);
  };

  const customSave = async (data: any, dinamicTitle: any) => {
    if (dinamicTitle === t("edit")) {
      data();
      return;
    }

    let isAssigned = false;
    await dispatch(ElectionActions.getElections(paramsRequest(1, count))).then((item: any) => {
      item.payload.items.forEach((item: any) => {
        if (item.saElectionId === clickRowData.electionId) {
          isAssigned = true;

          return;
        }
      });
    });

    if (isAssigned) {
      showWarningModal("Avertizare", "Sunteți sigur că doriți să importați această înregistrare?", data);
    } else {
      data();
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      dispatch(ElectionActions.getElectionsFromSaise());
      dispatch(SelectListsActions.getElectionStatus(paramsRequest(1, 20)));
    };

    fetchData();
  }, []);

  useEffect(() => {
    setRowData(data);
  }, [data]);
  return (
    <>
      {console.log(loading)}
      {!loading ? (
        <UniversalTable<IElectionPreview, IElectionImport>
          getData={ElectionActions.getElections}
          postItem={ElectionActions.importElection}
          putItem={ElectionActions.editElection}
          tableConfig={defaultConfig.elections}
          isAction={isActionPermissions()}
          isAdd={isAddPermissions()}
          isEdit={isEditPermissions()}
          dialogContentStyle={{
            display: "grid",
            gridTemplateColumns: "1fr",
            gap: "15px",
            paddingTop: "5px !important",
          }}
          paramsRequest={paramsRequest}
          modifyRowDataPath={electionRowDataPath}
          customDialogContent={handlerCustom}
          customTitleButton={t("import")}
          customDataSend={clickRowData}
          isAutoSizeColumns={true}
          customSave={customSave}
        />
      ) : (
        <></>
      )}
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

const styles = {
  defaultStylesContainer: {
    marginBottom: "10px",
    position: "relative",
  },
};

export default election;
