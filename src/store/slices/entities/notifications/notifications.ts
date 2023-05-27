import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { INotificationsPreview } from "~/models";
import { apiCall } from "~/store";

const name = "NOTIFICATIONS";

export const notificationsSliceInitialState: INotificationsPreview = {
  data: [],
  count: 0,
};

enum ActionType {
  NOTIFICATIONS_REQUEST_SUCCEEDED = "NOTIFICATIONS_REQUEST_SUCCEEDED",
  NOTIFICATIONS_COUNT_REQUEST_SUCCEEDED = "NOTIFICATIONS_COUNT_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: notificationsSliceInitialState,
  reducers: {
    [ActionType.NOTIFICATIONS_REQUEST_SUCCEEDED]: (state, action: PayloadAction<INotificationsPreview>) => {
      state.data = action.payload.items as INotificationsPreview;
    },
    [ActionType.NOTIFICATIONS_COUNT_REQUEST_SUCCEEDED]: (state, action: PayloadAction<INotificationsPreview>) => {
      state.count = action.payload as INotificationsPreview;
    },
  },
});

const { NOTIFICATIONS_REQUEST_SUCCEEDED, NOTIFICATIONS_COUNT_REQUEST_SUCCEEDED } = slice.actions;

const getNotifications = (paramsPayload: any) => {
  return apiCall(webApi.Notifications.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: NOTIFICATIONS_REQUEST_SUCCEEDED.type,
  });
};

const getNotificationsCount = () => {
  return apiCall(webApi.Notifications.getCount)({
    args: [{}],
    success: NOTIFICATIONS_COUNT_REQUEST_SUCCEEDED.type,
  });
};

const updatetNotifications = (notifiactions: any) => {
  return apiCall(webApi.Notifications.updateNotificationsRead)({
    args: [notifiactions],
  });
};

export const NotificationsActions = {
  getNotifications,
  getNotificationsCount,
  updatetNotifications,
};

export default slice.reducer;
