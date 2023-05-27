import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { ISubscriptionListStatusPreview, ISubscriptionListStatusCreate } from "~/models";
import { apiCall } from "~/store";

const name = "SUBSCIRPTION_LIST_STATUS";

export const subscriptionListStatusSliceInitialState: ISubscriptionListStatusPreview = {
  data: [],
};

enum ActionType {
  SUBSCIRPTION_LIST_STATUS_REQUEST_SUCCEEDED = "SUBSCIRPTION_LIST_STATUS_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: subscriptionListStatusSliceInitialState,
  reducers: {
    [ActionType.SUBSCIRPTION_LIST_STATUS_REQUEST_SUCCEEDED]: (
      state,
      action: PayloadAction<ISubscriptionListStatusPreview>,
    ) => {
      state.data = action.payload.items as ISubscriptionListStatusPreview;
    },
  },
});

const { SUBSCIRPTION_LIST_STATUS_REQUEST_SUCCEEDED } = slice.actions;

const getSubscriptionListStatus = (paramsPayload: any) => {
  return apiCall(webApi.SubscriptionListStatus.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: SUBSCIRPTION_LIST_STATUS_REQUEST_SUCCEEDED.type,
  });
};
const deleteSubscriptionListStatus = (id: string) => {
  return apiCall(webApi.SubscriptionListStatus.deleteSubscriptionListStatus)({
    args: [id],
  });
};

const updateSubscriptionListStatus = (subscriptionListStatus: ISubscriptionListStatusCreate) => {
  return apiCall(webApi.SubscriptionListStatus.updateSubscriptionListStatus)({
    args: [subscriptionListStatus],
  });
};

export const SubscriptionListStatusActions = {
  getSubscriptionListStatus,
  deleteSubscriptionListStatus,
  updateSubscriptionListStatus,
};

export default slice.reducer;
