import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { INotificationEventsPreview } from "~/models";
import { apiCall } from "~/store";

const name = "NOTIFICATION_EVENT";

export const notificationEventSliceInitialState: INotificationEventsPreview = {
  data: [],
};

enum ActionType {
  NOTIFICATION_EVENT_REQUEST_SUCCEEDED = "NOTIFICATION_EVENT_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: notificationEventSliceInitialState,
  reducers: {
    [ActionType.NOTIFICATION_EVENT_REQUEST_SUCCEEDED]: (state, action: PayloadAction<INotificationEventsPreview>) => {
      state.data = action.payload as INotificationEventsPreview;
    },
  },
});

const { NOTIFICATION_EVENT_REQUEST_SUCCEEDED } = slice.actions;

const getNotificationEvent = () => {
  return apiCall(webApi.NotificationEvent.getList)({
    args: [{}],
    success: NOTIFICATION_EVENT_REQUEST_SUCCEEDED.type,
  });
};

export const NotificationEventActions = {
  getNotificationEvent,
};

export default slice.reducer;
