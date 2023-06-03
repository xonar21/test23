import React, { useEffect, useState, useMemo } from "react";
import {
  Box,
  Button,
  TextField,
  MenuItem,
  Select,
  Checkbox,
  Typography,
  FormControl,
  InputLabel,
  FormControlLabel,
  FormLabel,
  Radio,
  RadioGroup,
  FormHelperText,
} from "@mui/material";
import { ColDef } from "ag-grid-community";
import AgGridTable from "~/pageList/components/AgGrid/AgGridTable";
import {
  ModeEditOutlined,
  AddBoxOutlined,
  MoreHoriz,
  DeleteOutline,
  Preview,
  BlockOutlined,
  CheckCircleOutlineOutlined,
} from "@mui/icons-material";
import DefaultDialogAction from "~/pageList/components/dialogs/DefaultDialogAction";
import PopoverModal from "~/pageList/components/Popover";
import { useDispatch, useSelector } from "react-redux";
import { IActionResult, IAxiosErrorPayload } from "~/core";
import { useTranslation } from "next-i18next";
import { getRoutePath, routes } from "~/shared";
import { usePermissionRedirect } from "~/hooks";
import { Permission } from "~/security";
import dayjs from "dayjs";
import { useRouter } from "next/router";
// import { DatePicker, DateValidationError, LocalizationProvider } from "@mui/x-date-pickers";
// import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { IResponse } from "~/models";
import customTooltip from "~/components/customTooltip";
import "dayjs/locale/ro";
import SuccessModal from "../successModal";
import ReactDOM from "react-dom";
import { SelectListsSelectors } from "~/store";
import AutocompleteField from "../AutocompleteField";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DateValidationError } from "@mui/x-date-pickers/models";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";

dayjs.extend(utc);
dayjs.extend(timezone);

interface UniversalTableProps<GetType = any, PostType = any> {
  getData?: (paramsPayload?: object) => IActionResult<IResponse<GetType> | IAxiosErrorPayload<unknown>>;
  postItem?: (item: PostType) => IActionResult<IAxiosErrorPayload<unknown> | PostType>;
  putItem?: (item: PostType) => IActionResult<IAxiosErrorPayload<unknown> | PostType>;
  deleteItem?: (id: string) => IActionResult<string | IAxiosErrorPayload<unknown>>;
  deactivateItem?: (id: any) => IActionResult<string | IAxiosErrorPayload<unknown>>;
  activateItem?: (id: any) => IActionResult<string | IAxiosErrorPayload<unknown>>;
  isAction?: boolean | undefined;
  isEdit?: boolean;
  isDelete?: boolean;
  isAdd?: boolean;
  isView?: boolean;
  isDeactivate?: boolean;
  isActivate?: boolean;
  viewPermission?: Permission;
  tableConfig: {
    tableHeaders: Record<
      string,
      {
        colName: string;
        flex?: number;
        formatted?: boolean;
        customFormat?: boolean;
        customField?: boolean;
        isTooltip?: boolean;
        width?: number;
      }
    >;
    formFields?: Record<
      string,
      {
        fieldName: string;
        component?: string;
        keyName: string;
        getter?: string;
        style?: any;
        action?: string;
        parent?: string;
        keySelect?: string;
        required?: boolean;
        disabled?: boolean;
        default?: any;
      }
    >;
  };
  containerStyle?: React.CSSProperties;
  selector?: any;
  pinnedAction?: boolean | "right" | "left" | null | undefined;
  pathRedirectToView?: string;
  dialogContentStyle?: any;
  modifyRowDataPath?: (response: any) => any;
  paramsRequest: any;
  customDialogContent?: (loading: any, setLoading: any) => any;
  customTitleButton?: string;
  customDataSend?: any;
  clickData?: (clickData: any) => any;
  customFormat?: (data: any) => any;
  customField?: (data: any) => any;
  updateComponent?: any;
  customPutValue?: (data: any) => any;
  customActivateValue?: (data: any) => any;
  customDeactivateValue?: (data: any) => any;
  isAutoSizeColumns?: boolean;
  customSave?: (data: any, title: any) => any;
  successTextPost?: string;
  successTextPut?: string;
  successTextDelete?: string;
  successTextDeactivate?: string;
  successTextActivate?: string;
  customData?: any;
  customLabelDeactivate?: any;
  customLabelActivate?: any;
}

const UniversalTable = <GetType, PostType>({
  getData,
  postItem,
  putItem,
  deleteItem,
  deactivateItem,
  activateItem,
  tableConfig,
  isAction,
  isEdit,
  isDelete,
  isAdd,
  isView,
  isDeactivate,
  isActivate,
  containerStyle,
  viewPermission,
  selector,
  pinnedAction,
  pathRedirectToView,
  dialogContentStyle,
  modifyRowDataPath,
  paramsRequest,
  customDialogContent,
  customTitleButton,
  customDataSend,
  clickData,
  customFormat,
  updateComponent,
  customPutValue,
  customActivateValue,
  customDeactivateValue,
  isAutoSizeColumns,
  customSave,
  successTextPost,
  successTextPut,
  successTextDelete,
  successTextDeactivate,
  successTextActivate,
  customData,
  customLabelDeactivate,
  customLabelActivate,
}: UniversalTableProps<GetType, PostType>): React.ReactElement => {
  if (viewPermission) {
    usePermissionRedirect([viewPermission], getRoutePath(routes.Home));
  }
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const defaultContainerStyle = useMemo(() => styles.defaultStylesContainer, []);
  const [value, setValue] = useState<any>({});
  const appliedContainerStyle = containerStyle ?? defaultContainerStyle;
  const [clickRowData, setClickRowData]: any = useState<Partial<GetType & { id: string; idnp: string }>>({});
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [dinamicTitle, setDinamicTitle] = useState("");
  const [openModal, setOpenModal] = useState(false);
  const [loading, setLoading] = useState(false);
  const [loadingBtn, setLoadingBtn] = useState(false);
  const [errors, setErrors] = useState<any>({});
  const [touched, setTouched] = useState<any>(false);
  const selectList = useSelector(SelectListsSelectors.getRoot);
  const router = useRouter();
  const columnDefsFromConfig = useMemo(() => {
    return Object.values(tableConfig.tableHeaders).map(header =>
      header.colName === "isAction" && isAction
        ? {
            field: "action",
            headerName: t("action"),
            width: router.locale === "ro" ? 76 : 100,
            pinned: pinnedAction,
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
                  <MoreHoriz />
                </Box>
              );
            },
          }
        : {
            field: header.colName,
            headerName: t(`${header.colName}`),
            flex: header.flex,
            width: header.width,
            valueFormatter: header.customFormat
              ? (params: any) => (customFormat ? customFormat(params) : [])
              : header.formatted
              ? (params: { value: string }) => formatUsingDayjs(params.value)
              : undefined,
            filter: "agTextColumnFilter",
            hide: header.colName === "isAction" ? true : false,
            tooltipField: header.isTooltip ? header.colName : "",
          },
    );
  }, [tableConfig.tableHeaders, updateComponent]);
  const [columnDefs] = useState<ColDef[]>(columnDefsFromConfig);
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

  const formatUsingDayjs = (date: string) => {
    if (date) {
      return dayjs(date).format("DD-MM-YYYY");
    }
    return "";
  };

  const handleClosePopover = () => {
    setAnchorEl(null);
  };

  useEffect(() => {
    if (!touched) return;

    const newErrors: any = {};
    if (!tableConfig.formFields) {
      return;
    }
    Object.keys(tableConfig.formFields).forEach((fieldKey: any) => {
      const field = (tableConfig.formFields as Record<string, any>)[fieldKey];
      if (
        field.required &&
        dayjs(value[fieldKey]).format("DD-MM-YYYY") === "Invalid Date" &&
        field.component === "Date"
      ) {
        newErrors[fieldKey] = "Acest câmp este obligatoriu";
        return;
      }
      if (field.required && Array.isArray(value[fieldKey])) {
        if (value[fieldKey].length === 0) {
          newErrors[fieldKey] = "Acest câmp este obligatoriu";
        }
      }
      if (field.required && (value[fieldKey] === undefined || value[fieldKey] === "" || value[fieldKey] === null)) {
        newErrors[fieldKey] = "Acest câmp este obligatoriu";
      }
    });
    setErrors(newErrors);
  }, [value]);

  const handleClickPopover = (event: React.MouseEvent<HTMLElement>, params: { node: any; data: any }) => {
    setAnchorEl(event.currentTarget);
    setClickRowData(params.data);
    if (clickData) {
      clickData(params.data);
    }
  };

  const handleOpenModal = (title: React.SetStateAction<string>) => {
    setErrors({});
    setTouched(false);
    setOpenModal(true);
    setDinamicTitle(title);

    if (title === t("edit") && tableConfig.formFields) {
      const initialValues: any = {};

      Object.keys(tableConfig.formFields).forEach(fieldKey => {
        if (tableConfig.formFields) {
          const field = tableConfig.formFields[fieldKey];
          initialValues[fieldKey] = (clickRowData as Record<string, any>)[field.keyName];
          if ("id" in clickRowData) {
            initialValues.id = clickRowData.id;
          }
        }
      });

      setValue(initialValues);
    }

    handleClosePopover();
  };

  const handleCloseModal = () => {
    setOpenModal(false);
    setValue({});
    setClickRowData({});
  };

  const showSuccessModal = (title: string, message: string) => {
    const successModal = document.createElement("div");
    document.body.appendChild(successModal);

    ReactDOM.render(React.createElement(SuccessModal, { title, message }), successModal);
  };

  const handleSave = async () => {
    if (!tableConfig.formFields) {
      return;
    }
    setTouched(true);

    let valid = true;
    const newErrors: any = {};

    Object.keys(tableConfig.formFields).forEach((fieldKey: any) => {
      const field = (tableConfig.formFields as Record<string, any>)[fieldKey];

      if (
        field.required &&
        dayjs(value[fieldKey]).format("DD-MM-YYYY") === "Invalid Date" &&
        field.component === "Date"
      ) {
        valid = false;
        newErrors[fieldKey] = "Acest câmp este obligatoriu";
        return;
      }
      if (field.required && Array.isArray(value[fieldKey])) {
        if (value[fieldKey].length === 0) {
          valid = false;
          newErrors[fieldKey] = "Acest câmp este obligatoriu";
        }
      }
      if (field.required && (value[fieldKey] === undefined || value[fieldKey] === "" || value[fieldKey] === null)) {
        valid = false;
        newErrors[fieldKey] = "Acest câmp este obligatoriu";
      }
    });

    setErrors(newErrors);
    if (!valid && !customDialogContent) {
      return;
    }

    setLoadingBtn(true);

    if (dinamicTitle === t("add") && postItem) {
      if (customDataSend) {
        setLoading(true);
        await dispatch(postItem(customDataSend)).then((e: any) => {
          handleCloseModal();
          if (e.succeeded) {
            showSuccessModal("Succes", successTextPost ? successTextPost : "Date adăugate cu succes");
          }
        });
        setTimeout(() => {
          setLoading(false);
        }, 500);
        setLoadingBtn(false);
        return;
      }
      dispatch(postItem(value)).then((e: any) => {
        if (e.succeeded) {
          showSuccessModal("Succes", successTextPost ? successTextPost : "Date adăugate cu succes");
        }
        handleCloseModal();
      });
      setLoadingBtn(false);
      return;
    }
    if (putItem) {
      dispatch(putItem(customPutValue ? customPutValue(value) : value)).then((e: any) => {
        if (e.succeeded) {
          showSuccessModal("Succes", successTextPut ? successTextPut : "Datele au fost actualizate cu succes");
        }
        handleCloseModal();
      });
      setLoadingBtn(false);
    }
  };

  const handleDelete = () => {
    if (deleteItem && clickRowData.id) {
      dispatch(deleteItem(clickRowData.id)).then(() => {
        setClickRowData({});
        showSuccessModal("Succes", successTextDelete ? successTextDelete : "Date șterse cu succes");
      });

      handleClosePopover();
    }
  };

  const handleDeactivate = () => {
    if (deactivateItem && clickRowData.id && customDeactivateValue) {
      dispatch(deactivateItem(customDeactivateValue(clickRowData.id))).then(() => {
        showSuccessModal("Succes", successTextDeactivate ? successTextDeactivate : "Date dezactivate cu succes");
        setClickRowData({});
      });

      handleClosePopover();
    }
  };

  const handleActivate = () => {
    if (activateItem && clickRowData.id && customActivateValue) {
      dispatch(activateItem(customActivateValue(clickRowData.id))).then(() => {
        showSuccessModal("Succes", successTextActivate ? successTextActivate : "Date activate cu succes");
        setClickRowData({});
      });

      handleClosePopover();
    }
  };

  const handleDateChange = (date: dayjs.Dayjs | null, fieldName: string, value: any) => {
    const dateWithAddedHour = dayjs(date).add(2, "hour");

    setValue({ ...value, [fieldName]: dayjs.utc(dateWithAddedHour).tz("Europe/Chisinau") });
  };

  const viewRedirect = () => {
    if (clickRowData.idnp && router.pathname.includes(getRoutePath(routes.VoterManagement))) {
      router.replace(`${pathRedirectToView}/${clickRowData.idnp}`);
      return;
    }
    if (clickRowData.id) {
      router.replace(`${pathRedirectToView}/${clickRowData.id}`);
      return;
    }
    if (clickRowData.subscriptionListId && router.pathname.includes(getRoutePath(routes.Signatures))) {
      router.replace(`${pathRedirectToView}/${clickRowData.subscriptionListId}`);
      return;
    }
  };

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
      const sort = sortModel[0] ? sortModel[0].sort : "";
      const colId = sortModel[0] ? sortModel[0].colId : "";

      try {
        if (!getData) {
          return;
        }
        const response: any = customData
          ? await dispatch(getData(customData))
          : await dispatch(
              getData(
                paramsRequest(pageNumber, pageSize, JSON.stringify(createArrayFromObject(filterModel)), colId, sort),
              ),
            );
        setTimeout(function () {
          if (response.succeeded && modifyRowDataPath) {
            if (response.payload) {
              if (response.payload) {
                params.success({
                  rowData: modifyRowDataPath(response),
                  rowCount: response.payload.totalCount
                    ? response.payload.totalCount
                    : modifyRowDataPath(response).length,
                });
                return;
              }
              if (response.payload.totalCount) {
                params.success({
                  rowData: modifyRowDataPath(response),
                  rowCount: response.payload.totalCount,
                });
                return;
              }

              params.success({
                rowData: modifyRowDataPath(response),
                rowCount: response.payload.totalCount,
              });
            }
          } else {
            params.fail();
          }
        }, 500);
      } catch (error) {
        params.fail();
      }
    },
  };

  const renderFormFields = (formFields: any, setValue: any, value: any) => {
    if (!formFields && !customDialogContent) {
      return;
    }

    if (customDialogContent && dinamicTitle === t("add")) {
      return customDialogContent(loading, setLoading);
    }
    return Object.keys(formFields).map(fieldKey => {
      const field = formFields[fieldKey];
      const fieldValue = value[fieldKey];
      const error = errors[fieldKey];
      if (field.component === "TextField") {
        return (
          <TextField
            error={errors[fieldKey] ? true : false}
            required={field.required}
            key={fieldKey}
            value={fieldValue}
            label={t(`${field.fieldName}`)}
            fullWidth
            sx={{ ...field.style, margin: 0 }}
            helperText={error ? error : " "}
            margin="normal"
            multiline
            onChange={e => {
              setValue({ ...value, [fieldKey]: e.target.value });
            }}
          />
        );
      }
      if (field.component === "Autocomplete") {
        return (
          <AutocompleteField
            key={fieldKey}
            field={field}
            fieldValue={fieldValue}
            fieldKey={fieldKey}
            setValue={setValue}
            value={value}
            dinamicTitle={dinamicTitle}
            t={t}
            openModal={openModal}
            selectList={selectList}
            paramsRequest={paramsRequest}
            error={errors[fieldKey]}
            helperText={error}
            required={field.required}
            isDisabled={field.disabled}
            defaulted={field.default}
            // setTouched={setTouched}
          />
        );
      }
      if (field.component === "Select") {
        return (
          <FormControl fullWidth required={field.required} error={errors[fieldKey] ? true : false}>
            <InputLabel id="demo-simple-select-label">{t(`${field.fieldName}`)}</InputLabel>

            <Select
              // error={errors[fieldKey] ? true : false}
              multiple
              value={fieldValue ? fieldValue : []}
              labelId="demo-simple-select-label"
              label={t(`${field.fieldName}`)}
              onChange={e => setValue({ ...value, [fieldKey]: e.target.value })}
              renderValue={selected => (selected as string[]).join(", ")}
              fullWidth
              MenuProps={{
                PaperProps: {
                  style: {
                    maxHeight: "300px",
                  },
                },
              }}
            >
              {(selector as Record<string, any>) &&
                (selector as Record<string, any>).map((permission: any, index: number) => (
                  <MenuItem key={index} value={permission.type}>
                    <Checkbox checked={fieldValue && fieldValue.indexOf(permission.type) > -1} />

                    {permission.type}
                  </MenuItem>
                ))}
            </Select>
            <FormHelperText id="my-helper-text">{error ? error : " "}</FormHelperText>
          </FormControl>
        );
      }
      if (field.component === "Radio") {
        return (
          <FormControl sx={{ marginTop: "0px" }} required={field.required} error={errors[fieldKey] ? true : false}>
            <FormLabel
              id="demo-controlled-radio-buttons-group"
              sx={{
                marginBottom: "5px",
                fontSize: "12px",
                lineHeight: "14px",
                fontWeight: "normal",
              }}
            >
              {t(`${field.fieldName}`)}
            </FormLabel>
            <RadioGroup
              aria-labelledby="demo-controlled-radio-buttons-group"
              name="controlled-radio-buttons-group"
              value={fieldValue}
              onChange={e => {
                const booleanValue = e.target.value === "true";
                setValue({ ...value, [fieldKey]: booleanValue });
              }}
              sx={{
                display: "grid",
                gridTemplateColumns: "1fr 1fr",
                height: "32px",
                width: "max-content",
                marginLeft: "-10px",
              }}
            >
              <FormControlLabel
                value={true}
                sx={{
                  margin: "0px",
                  paddingRight: "15px",
                  height: "32px",
                  width: "max-content",
                  ".MuiTypography-root": {
                    fontSize: "12px",
                    lineHeight: "14px",
                    color: fieldValue === true ? "#00305C !important" : "grey",
                    fontWeight: "bold",
                  },
                }}
                control={
                  <Radio
                    size="small"
                    sx={{
                      color: fieldValue === true ? "#00305C !important" : "grey",
                      "&:hover": {
                        background: "none",
                      },
                    }}
                  />
                }
                label="Da"
              />
              <FormControlLabel
                value={false}
                sx={{
                  margin: "0px",
                  paddingRight: "15px",
                  height: "32px",
                  width: "max-content",
                  ".MuiTypography-root": {
                    fontSize: "12px",
                    lineHeight: "14px",
                    color: fieldValue === false ? "#00305C !important" : "grey",
                    fontWeight: "bold",
                  },
                }}
                control={
                  <Radio
                    size="small"
                    sx={{
                      color: fieldValue === false ? "#00305C !important" : "grey",
                      "&:hover": {
                        background: "none",
                      },
                    }}
                  />
                }
                label="Nu"
              />
            </RadioGroup>
            <FormHelperText id="my-helper-text" sx={{ margin: "0px" }}>
              {error ? error : " "}
            </FormHelperText>
          </FormControl>
        );
      }
      if (field.component === "Date") {
        const fieldValue = value[field.keyName] ? dayjs(value[field.keyName]) : null;
        return (
          <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="ro">
            <DatePicker
              value={fieldValue}
              onChange={date => handleDateChange(date, field.keyName, value)}
              label={t(`${field.fieldName}`)}
              format="DD-MM-YYYY"
              sx={{ ".MuiFormHelperText-root": { height: "19.91px" } }}
              slotProps={{
                textField: {
                  helperText: error ? error : "  ",
                  required: field.required,
                  error: errors[fieldKey] ? true : false,
                },
              }}
            />
          </LocalizationProvider>
        );
      }
    });
  };

  const isEmpty = (obj: any) => {
    for (const key in obj) {
      if (Object.prototype.hasOwnProperty.call(obj, key)) return false;
    }
    return true;
  };

  return (
    <>
      <Button
        sx={{
          ...styles.addButton,
          visibility: isAdd ? "inherit" : "hidden",
        }}
        onClick={() => handleOpenModal(t("add"))}
      >
        <AddBoxOutlined sx={{ marginRight: "5px", fontSize: "20px", marginBottom: "2px" }} />
        {customTitleButton ? t(customTitleButton) : t("add")}
      </Button>

      <AgGridTable
        containerStyle={appliedContainerStyle}
        additionalProps={additionalProps}
        source={serverSideDatasource}
        clickRowData={clickRowData}
        isAutoSizeColumns={isAutoSizeColumns}
      />

      <PopoverModal
        anchorEl={anchorEl}
        handleClose={handleClosePopover}
        content={
          <>
            <Box
              sx={{
                ...styles.boxPopover,
                display: isView ? "flex" : "none",
                borderBottom: "1px solid white",
              }}
              onClick={() => viewRedirect()}
            >
              <Preview sx={styles.editIcon} />

              <Typography sx={styles.textSign}>{t("view")}</Typography>
            </Box>
            <Box
              sx={{
                ...styles.boxPopover,
                display: isEdit ? "flex" : "none",
                borderBottom: "1px solid white",
              }}
              onClick={() => handleOpenModal(t("edit"))}
            >
              <ModeEditOutlined sx={styles.editIcon} />

              <Typography sx={styles.textSign}>{t("edit")}</Typography>
            </Box>

            <Box
              sx={{
                ...styles.boxPopover,
                display: isDelete ? "flex" : "none",
                backgroundColor: "white",
              }}
              onClick={handleDelete}
            >
              <DeleteOutline sx={styles.editIcon} />

              <Typography sx={styles.textSign}>{t("delete")}</Typography>
            </Box>

            <Box
              sx={{
                ...styles.boxPopover,
                display: isDeactivate ? "flex" : "none",
                backgroundColor: "white",
              }}
              onClick={handleDeactivate}
            >
              <BlockOutlined sx={styles.editIcon} />

              <Typography sx={styles.textSign}>
                {customLabelDeactivate ? customLabelDeactivate : t("deactivate")}
              </Typography>
            </Box>

            <Box
              sx={{
                ...styles.boxPopover,
                display: isActivate ? "flex" : "none",
                backgroundColor: "white",
              }}
              onClick={handleActivate}
            >
              <CheckCircleOutlineOutlined sx={styles.editIcon} />

              <Typography sx={styles.textSign}>{customLabelActivate ? customLabelActivate : t("activate")}</Typography>
            </Box>
          </>
        }
      />

      <DefaultDialogAction
        openModal={openModal}
        handleClose={handleCloseModal}
        dynamicTitle={() => {
          if (dinamicTitle === t("add")) {
            return customTitleButton ? customTitleButton : dinamicTitle;
          }
          return dinamicTitle;
        }}
        handleSave={() => {
          if (customSave) {
            return customSave(handleSave, dinamicTitle);
          }
          return handleSave();
        }}
        content={<>{renderFormFields(tableConfig.formFields, setValue, value)}</>}
        styledContent={dialogContentStyle}
        loading={loading}
        loadingBtn={loadingBtn}
        disabled={isEmpty(errors) ? false : true}
      />
    </>
  );
};

const styles = {
  defaultStylesContainer: {
    marginBottom: "10px",
    position: "relative",
  },
  textSign: {
    color: "rgba(0, 48, 92, 1)",
    fontSize: "12px",
    margin: "0px",
  },
  boxPopover: {
    width: 150,
    height: 40,
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    transition: "border 0.2s ease-in-out",
    "&:hover": {
      cursor: "pointer",
    },
  },
  editIcon: {
    color: "rgba(0, 48, 92, 1)",
    fontSize: "20px",
    alignItems: "center",
    marginRight: "3px",
  },
  addButton: {
    margin: "20px 0 10px 0px",
    color: "white",
    textTransform: "none",
    backgroundColor: "rgba(0, 48, 92, 1)",
    border: "1px solid rgba(0, 48, 92, 1)",
    height: "32px",
    width: "160px",
    padding: "6.5px 8px",
    "&:hover": {
      backgroundColor: "white",
      color: "rgba(0, 48, 92, 1)",
    },
  },
  radioGroup: {
    display: "grid",
    gridTemplateColumns: "1fr 1fr",
    border: "1px solid gray",
    borderRadius: "5px",
  },
  formControlLabelRadio: {
    margin: "0px",
    paddingRight: "15px",
    transition: "background-color 0.2s ease-in-out",
  },
};
export default UniversalTable;
