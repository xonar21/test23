/* eslint-disable @typescript-eslint/no-explicit-any */
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IAxiosErrorPayload, IExtendedApiPayload, Nullable } from "~/core";
import { apiCall, hydrate } from "~/store/actions";

const name = "SELECT_LISTS";

interface ISelectLists {
  politicalParties: {
    items: any[];
  };
  genders: {
    items: any[];
  };
  elections: {
    items: any[];
  };
  circumscriptions: {
    items: any[];
  };
  electionFunction: {
    items: any[];
  };
  electionStatus: {
    items: any[];
  };
  subscriptionListStatus: {
    items: any[];
  };
}

const initialSelectLists: ISelectLists = {
  politicalParties: {
    items: [],
  },
  genders: {
    items: [],
  },
  elections: {
    items: [],
  },
  circumscriptions: {
    items: [],
  },
  electionFunction: {
    items: [],
  },
  electionStatus: {
    items: [],
  },
  subscriptionListStatus: {
    items: [],
  },
};

export type SelectListKey = keyof typeof initialSelectLists;

export type SelectListAction = keyof typeof SelectListsActions;

export type SelectListSelector = keyof typeof selectListsSliceInitialState;

type SelectListsSliceState = ISelectLists & {
  loading: Record<SelectListKey, boolean>;
  error: Record<SelectListKey, Nullable<IAxiosErrorPayload>>;
  items?: any;
  totalPages?: any;
};

export const selectListsSliceInitialState: SelectListsSliceState = {
  ...initialSelectLists,
  loading: {
    politicalParties: false,
    genders: false,
    elections: false,
    circumscriptions: false,
    electionFunction: false,
    electionStatus: false,
    subscriptionListStatus: false,
  },
  error: {
    politicalParties: null,
    genders: null,
    elections: null,
    circumscriptions: null,
    electionFunction: null,
    electionStatus: null,
    subscriptionListStatus: null,
  },
};

export enum ActionType {
  LIST_REQUEST_STARTED = "LIST_REQUEST_STARTED",
  LIST_REQUEST_SUCCEEDED = "LIST_REQUEST_SUCCEEDED",
  LIST_REQUEST_FAILED = "LIST_REQUEST_FAILED",
  LIST_REQUEST_ENDED = "LIST_REQUEST_ENDED",
}

const slice = createSlice({
  name,
  initialState: selectListsSliceInitialState,
  reducers: {
    [ActionType.LIST_REQUEST_STARTED]: (state, action: PayloadAction<SelectListKey>) => {
      state.loading[action.payload] = true;
    },
    [ActionType.LIST_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IExtendedApiPayload<any, SelectListKey>>) => {
      const { responsePayload, optionalPayload } = action.payload;
      const formatItems = responsePayload.items.map((e: any) => {
        return { ...e, key: e.key.toLowerCase() };
      });
      state[optionalPayload] = { ...responsePayload, items: formatItems };
    },
    [ActionType.LIST_REQUEST_FAILED]: (
      state,
      action: PayloadAction<IExtendedApiPayload<IAxiosErrorPayload, SelectListKey>>,
    ) => {
      const { responsePayload, optionalPayload } = action.payload;
      state.error[optionalPayload] = responsePayload;
    },
    [ActionType.LIST_REQUEST_ENDED]: (state, action: PayloadAction<SelectListKey>) => {
      state.loading[action.payload] = false;
    },
  },
  extraReducers: builder => {
    builder.addCase(hydrate, (state, action) => {
      return {
        ...state,
        ...action.payload[name],
      };
    });
  },
});

const { LIST_REQUEST_STARTED, LIST_REQUEST_SUCCEEDED, LIST_REQUEST_FAILED, LIST_REQUEST_ENDED } = slice.actions;

const getSelectList = (api: (args: any) => Promise<unknown>, paramsPayload: any, key: SelectListKey) => {
  return apiCall(api)({
    args: [
      {
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    start: LIST_REQUEST_STARTED.type,
    startPayload: key,
    success: LIST_REQUEST_SUCCEEDED.type,
    successPayload: key,
    failure: LIST_REQUEST_FAILED.type,
    failurePayload: key,
    end: LIST_REQUEST_ENDED.type,
    endPayload: key,
  });
};

const getSubSelectList = (api: (args: any) => Promise<unknown>, paramsPayload: any, key: SelectListKey) => {
  return apiCall(api)({
    args: [
      {
        id: paramsPayload.id,
        PageNumber: paramsPayload.number,
        PageSize: paramsPayload.size,
        Filters: paramsPayload.filters,
        SortField: paramsPayload.sortField,
        SortOrder: paramsPayload.sortOrder,
      },
    ],
    start: LIST_REQUEST_STARTED.type,
    startPayload: key,
    success: LIST_REQUEST_SUCCEEDED.type,
    successPayload: key,
    failure: LIST_REQUEST_FAILED.type,
    failurePayload: key,
    end: LIST_REQUEST_ENDED.type,
    endPayload: key,
  });
};

const getPoliticalParties = (paramsPayload: any) => {
  return getSelectList(webApi.PoliticalParties.getSelectList, paramsPayload, "politicalParties");
};

const getGenders = (paramsPayload: any) => {
  return getSelectList(webApi.Genders.getSelectList, paramsPayload, "genders");
};

const getElections = (paramsPayload: any) => {
  return getSelectList(webApi.Elections.getSelectList, paramsPayload, "elections");
};

const getElectionFunction = (paramsPayload: any) => {
  return getSelectList(webApi.ElectionFunction.getSelectList, paramsPayload, "electionFunction");
};

const getElectionStatus = (paramsPayload: any) => {
  return getSelectList(webApi.ElectionStatus.getSelectList, paramsPayload, "electionStatus");
};

const getSubscriptionListStatus = (paramsPayload: any) => {
  return getSelectList(webApi.SubscriptionListStatus.getSelectList, paramsPayload, "subscriptionListStatus");
};

const getCircumscriptions = (paramsPayload: any) => {
  return getSubSelectList(webApi.Elections.getSubSelectList, paramsPayload, "circumscriptions");
};

export const SelectListsActions = {
  ...slice.actions,
  getPoliticalParties,
  getGenders,
  getElections,
  getCircumscriptions,
  getElectionFunction,
  getElectionStatus,
  getSubscriptionListStatus,
};

export default slice.reducer;
