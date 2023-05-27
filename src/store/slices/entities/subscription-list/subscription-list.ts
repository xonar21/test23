import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { ISubscriptionListCreate, ISubscriptionListsPreview } from "~/models";
import { apiCall } from "~/store";

const name = "SUBSCIRPTION_LIST";

export const subscriptionListSliceInitialState: ISubscriptionListsPreview = {
  data: [],
};

enum ActionType {
  SUBSCIRPTION_LIST_REQUEST_SUCCEEDED = "SUBSCIRPTION_LIST_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: subscriptionListSliceInitialState,
  reducers: {
    [ActionType.SUBSCIRPTION_LIST_REQUEST_SUCCEEDED]: (state, action: PayloadAction<ISubscriptionListsPreview>) => {
      state.data = action.payload.items as ISubscriptionListsPreview;
    },
  },
});

const { SUBSCIRPTION_LIST_REQUEST_SUCCEEDED } = slice.actions;

const getSubscriptionList = (paramsPayload: any) => {
  return apiCall(webApi.SubscriptionList.getList)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: SUBSCIRPTION_LIST_REQUEST_SUCCEEDED.type,
  });
};

const getSubscriptionListForSigning = (paramsPayload: any) => {
  return apiCall(webApi.SubscriptionList.getListForSigning)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: SUBSCIRPTION_LIST_REQUEST_SUCCEEDED.type,
  });
};

const getSubscriptionListForSigningMy = (paramsPayload: any) => {
  return apiCall(webApi.SubscriptionList.getListForSigningMy)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: SUBSCIRPTION_LIST_REQUEST_SUCCEEDED.type,
  });
};

const getSubscriptionListForSigningActive = (paramsPayload: any) => {
  return apiCall(webApi.SubscriptionList.getListForSigningActive)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: SUBSCIRPTION_LIST_REQUEST_SUCCEEDED.type,
  });
};

const getSubscriptionListForSigningArchived = (paramsPayload: any) => {
  return apiCall(webApi.SubscriptionList.getListForSigningArhived)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    success: SUBSCIRPTION_LIST_REQUEST_SUCCEEDED.type,
  });
};

const deleteSubscriptionList = (id: string) => {
  return apiCall(webApi.SubscriptionList.deleteSubscriptionList)({
    args: [id],
  });
};

const updateSubscriptionList = (subscriptionList: ISubscriptionListCreate) => {
  return apiCall(webApi.SubscriptionList.updateSubscriptionList)({
    args: [subscriptionList],
  });
};

const subscriptionListDeactivate = (subscriptionList: any) => {
  return apiCall(webApi.SubscriptionList.subscriptionListDeactivate)({
    args: [subscriptionList],
  });
};

const subscriptionListActivate = (subscriptionList: any) => {
  return apiCall(webApi.SubscriptionList.subscriptionListActivate)({
    args: [subscriptionList],
  });
};

const createSubscriptionList = (subscriptionList: ISubscriptionListCreate) => {
  return apiCall(webApi.SubscriptionList.createSubscriptionList)({
    args: [subscriptionList],
  });
};

export const SubscriptionListActions = {
  getSubscriptionList,
  deleteSubscriptionList,
  updateSubscriptionList,
  createSubscriptionList,
  getSubscriptionListForSigning,
  getSubscriptionListForSigningMy,
  getSubscriptionListForSigningActive,
  getSubscriptionListForSigningArchived,
  subscriptionListDeactivate,
  subscriptionListActivate,
};

export default slice.reducer;
