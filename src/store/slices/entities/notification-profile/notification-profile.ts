import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { INotificationProfilesPreview, INotificationProfileUpdate } from "~/models";
import { apiCall } from "~/store";

const name = "NOTIFICATION_PROFILE";

export const notificationProfileSliceInitialState: INotificationProfilesPreview = {
  data: [],
};

enum ActionType {
  NOTIFICATION_PROFILE_REQUEST_SUCCEEDED = "NOTIFICATION_PROFILE_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: notificationProfileSliceInitialState,
  reducers: {
    [ActionType.NOTIFICATION_PROFILE_REQUEST_SUCCEEDED]: (
      state,
      action: PayloadAction<INotificationProfilesPreview>,
    ) => {
      state.data = action.payload as INotificationProfilesPreview;
    },
  },
});

const { NOTIFICATION_PROFILE_REQUEST_SUCCEEDED } = slice.actions;

const getNotificationProfile = () => {
  return apiCall(webApi.NotificationProfile.getListOwn)({
    args: [{}],
    success: NOTIFICATION_PROFILE_REQUEST_SUCCEEDED.type,
  });
};

const updateNotificationProfile = (notificationProfile: INotificationProfileUpdate) => {
  return apiCall(webApi.NotificationProfile.updateNotificationProfile)({
    args: [notificationProfile],
  });
};

export const NotificationProfileActions = {
  getNotificationProfile,
  updateNotificationProfile,
};

export default slice.reducer;
