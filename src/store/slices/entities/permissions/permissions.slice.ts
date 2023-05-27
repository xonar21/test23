import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IPermissionPreview } from "~/models";
import { apiCall } from "~/store";

const name = "REGIONS";

export const permissionsSliceInitialState: IPermissionPreview = {
  data: [],
};

enum ActionType {
  PERMISSIONS_REQUEST_SUCCEEDED = "PERMISSIONS_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: permissionsSliceInitialState,
  reducers: {
    [ActionType.PERMISSIONS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IPermissionPreview>) => {
      state.data = action.payload as IPermissionPreview;
    },
  },
});

const { PERMISSIONS_REQUEST_SUCCEEDED } = slice.actions;

const getPermissions = () => {
  return apiCall(webApi.Permissions.getList)({
    args: [{ page: 0 }],
    success: PERMISSIONS_REQUEST_SUCCEEDED.type,
  });
};

export const PermissionsActions = {
  getPermissions,
};

export default slice.reducer;
