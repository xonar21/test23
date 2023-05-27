import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IRoleCreate, IRolePreview } from "~/models";
import { apiCall } from "~/store";

const name = "REGIONS";

export const rolesSliceInitialState: IRolePreview = {
  data: [],
};

enum ActionType {
  ROLES_REQUEST_SUCCEEDED = "ROLES_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: rolesSliceInitialState,
  reducers: {
    [ActionType.ROLES_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IRolePreview>) => {
      state.data = action.payload as IRolePreview;
    },
  },
});

const { ROLES_REQUEST_SUCCEEDED } = slice.actions;

const getRoles = (paramsPayload?: any) => {
  return apiCall(webApi.Roles.getList)({
    args: [
      paramsPayload
        ? {
            PageNumber: paramsPayload.number,
            PageSize: paramsPayload.size,
            Filters: paramsPayload.filters,
            SortField: paramsPayload.sortField,
            SortOrder: paramsPayload.sortOrder,
          }
        : {},
    ],
    success: ROLES_REQUEST_SUCCEEDED.type,
  });
};
const getRoleId = (id: string) => {
  return apiCall(webApi.Roles.getRoleId)({
    args: [{ id: id }],
    success: ROLES_REQUEST_SUCCEEDED.type,
  });
};
const deleteRoles = (id: string) => {
  return apiCall(webApi.Roles.deleteRoles)({
    args: [id],
  });
};

const updateRoles = (role: IRolePreview) => {
  return apiCall(webApi.Roles.updateRoles)({
    args: [role],
  });
};

const createRole = (role: IRoleCreate) => {
  return apiCall(webApi.Roles.createRoles)({
    args: [role],
  });
};

export const RolesActions = {
  getRoles,
  createRole,
  deleteRoles,
  updateRoles,
  getRoleId,
};

export default slice.reducer;
