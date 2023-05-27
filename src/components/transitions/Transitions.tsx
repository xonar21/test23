import React, { useCallback, useEffect, useState } from "react";
import { Box, Button, Checkbox, Container, FormControl, InputLabel, MenuItem, Select, Typography } from "@mui/material";
import { useTranslation } from "next-i18next";
import { useRouter } from "next/router";
import { useDispatch, useSelector } from "react-redux";
import { RolesActions, RolesSelectors, WorkFlowsActions, WorkFlowsSelectors } from "~/store";
import { styleButton } from "~/pageList/styles/defaultStyleButtonForDialog";
import { SaveOutlined } from "@mui/icons-material";
import { NextPage } from "next";
import SuccessModal from "../successModal";
import ReactDOM from "react-dom";

const Transitions: NextPage = () => {
  const { t } = useTranslation();
  const { data, workflowTransitions, workflowsStatus } = useSelector(WorkFlowsSelectors.getRoot);
  const [rowData, setRowData] = useState(workflowTransitions);
  const [status, setStatus] = useState([]);
  const rolesData = useSelector(RolesSelectors.getRoot);
  const router = useRouter();
  const dispatch = useDispatch();

  const sortTransitions = (transitions: any, orderedStateIds: any) => {
    return orderedStateIds.map((stateId: any) => transitions.find((transition: any) => transition.stateId === stateId));
  };

  const changeData = (updatedValue: any, indexRow: any, path: any, select: any) => {
    const formatValue: string[] = [];

    if (path === "rolesToNotify" || path === "requiredClaims") {
      updatedValue.map((e: any) => {
        select.forEach((roles: any) => {
          if (roles.name === e) {
            formatValue.push(roles.id);
          }
        });
      });
    }

    if (path === "toStates") {
      updatedValue.map((e: any) => {
        select.forEach((status: any) => {
          if (status.code === e) {
            formatValue.push(status.id);
          }
        });
      });
    }

    const changePayload = rowData.transitions.map((row: any, index: any) =>
      index === indexRow ? { ...row, [path]: formatValue } : row,
    );

    setRowData({ ...rowData, transitions: changePayload });
  };

  const paramsRequest = (number?: number, size?: number, filters?: object, sortField?: string, sortOrder?: string) => {
    return {
      workFlowId: router.query.id,
      number,
      size,
      filters,
      sortField,
      sortOrder,
    };
  };

  const formatValue = (data: any, path: any) => {
    const selected: string[] = [];

    if (path === "rolesToNotify" || path === "requiredClaims") {
      data.map((item: any) => {
        rolesData.data.items.map((roles: any) => {
          if (item === roles.id) {
            selected.push(roles.name);
          }
        });
      });
    }

    if (path === "toStates") {
      data.map((item: any) => {
        status.map((status: any) => {
          if (item === status.id) {
            selected.push(status.code);
          }
        });
      });
    }

    return selected;
  };

  const renderField = useCallback(() => {
    if (status.length === 0) {
      return;
    }
    console.log(status);
    return rowData.transitions.map((item: any, index: any) => {
      return (
        <>
          <Typography variant="h1" sx={{ color: "#00305C", fontWeight: "400" }}>
            {item.stateName}
          </Typography>
          <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">{t(`requiredClaims`)}</InputLabel>
            <Select
              multiple
              sx={{
                ".MuiPaper-root": { height: "200px" },
              }}
              value={item ? formatValue(item.requiredClaims, "requiredClaims") : []}
              renderValue={selected => (selected as string[]).join(", ")}
              onChange={e => changeData(e.target.value, index, "requiredClaims", rolesData.data.items)}
              fullWidth
              labelId="demo-simple-select-label"
              label={t(`requiredClaims`)}
              MenuProps={{
                PaperProps: {
                  style: {
                    maxHeight: "300px",
                  },
                },
              }}
            >
              {(rolesData.data.items as Record<string, any>) &&
                (rolesData.data.items as Record<string, any>).map((role: any, index: number) => (
                  <MenuItem key={index} value={role.name}>
                    <Checkbox
                      checked={item.requiredClaims.some((e: any) => {
                        return e === role.id ? true : false;
                      })}
                    />
                    {role.name}
                  </MenuItem>
                ))}
            </Select>
          </FormControl>
          <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">{t(`rolesToNotify`)}</InputLabel>
            <Select
              multiple
              sx={{
                ".MuiPaper-root": { height: "200px" },
              }}
              value={item ? formatValue(item.rolesToNotify, "rolesToNotify") : []}
              renderValue={selected => (selected as string[]).join(", ")}
              onChange={e => changeData(e.target.value, index, "rolesToNotify", rolesData.data.items)}
              fullWidth
              labelId="demo-simple-select-label"
              label={t(`rolesToNotify`)}
              MenuProps={{
                PaperProps: {
                  style: {
                    maxHeight: "300px",
                  },
                },
              }}
            >
              {(rolesData.data.items as Record<string, any>) &&
                (rolesData.data.items as Record<string, any>).map((role: any, index: number) => (
                  <MenuItem key={index} value={role.name}>
                    <Checkbox
                      checked={item.rolesToNotify.some((e: any) => {
                        return e === role.id ? true : false;
                      })}
                    />
                    {role.name}
                  </MenuItem>
                ))}
            </Select>
          </FormControl>
          <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">{t(`toStates`)}</InputLabel>
            <Select
              multiple
              sx={{
                ".MuiPaper-root": { height: "200px" },
              }}
              value={item ? formatValue(item.toStates, "toStates") : []}
              renderValue={selected => (selected as string[]).join(", ")}
              onChange={e => changeData(e.target.value, index, "toStates", status)}
              fullWidth
              labelId="demo-simple-select-label"
              label={t(`toStates`)}
              MenuProps={{
                PaperProps: {
                  style: {
                    maxHeight: "300px",
                  },
                },
              }}
            >
              {(status as Record<string, any>) &&
                (status as Record<string, any>).map((status: any, index: number) =>
                  item.stateName !== status.code ? (
                    <MenuItem key={index} value={status.code}>
                      <Checkbox
                        checked={item.toStates.some((e: any) => {
                          return e === status.id ? true : false;
                        })}
                      />
                      {status.code}
                    </MenuItem>
                  ) : (
                    <></>
                  ),
                )}
            </Select>
          </FormControl>
        </>
      );
    });
  }, [rowData, dispatch, workflowTransitions, status]);

  const showSuccessModal = (title: string, message: string) => {
    const successModal = document.createElement("div");
    document.body.appendChild(successModal);

    ReactDOM.render(React.createElement(SuccessModal, { title, message }), successModal);
  };

  const handleSave = () => {
    dispatch(WorkFlowsActions.updateWorkflowTransitions(rowData)).then(() => {
      dispatch(WorkFlowsActions.getWorkFlowTransitions(paramsRequest()));
      showSuccessModal("Succes", "Tranziții de flux de lucru actualizate cu succes");
    });
  };

  useEffect(() => {
    data.forEach(async (e: any) => {
      if (router.query.id === e.id) {
        const response: any = await dispatch(
          WorkFlowsActions.getWorkflowStatus({ id: router.query.id, entityType: e.entityType }),
        );
        if (response.payload.responsePayload) {
          setStatus(response.payload.responsePayload);
        }
      }
    });
    dispatch(RolesActions.getRoles());
    dispatch(WorkFlowsActions.getWorkFlowTransitions(paramsRequest()));
  }, []);

  useEffect(() => {
    setRowData({
      ...rowData,
      transitions: sortTransitions(workflowTransitions.transitions, workflowTransitions.orderedStateIds),
    });
  }, [workflowTransitions]);

  return (
    <>
      <Container
        className="fadeIn"
        sx={{
          border: "1px solid #00305C",
          maxWidth: "none !important",
          borderRadius: "10px",
          boxShadow: "0px 10px 15px #ccc",
          display: "grid",
          gridTemplateColumns: "1fr",
          gridTemplateRows: "max-content max-content",
          gap: "00px",
          paddingTop: "24px",
          paddingBottom: "24px",
          marginTop: "40px",
        }}
      >
        <Box sx={{ display: "flex", flexDirection: "column", gap: "16px" }}>
          <Box
            sx={{ display: "grid", gridTemplateColumns: "min-content 1fr 1fr 1fr", gap: "30px", alignItems: "center" }}
          >
            {workflowTransitions.transitions ? renderField() : <></>}
          </Box>

          <Button onClick={handleSave} color="primary" sx={{ ...styleButton.buttonConfirm, marginLeft: "auto" }}>
            <SaveOutlined sx={{ marginRight: "5px" }} />
            {t("save")}
          </Button>
        </Box>
      </Container>
    </>
  );
};

export default Transitions;
