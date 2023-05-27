import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { webApi } from "~/api";
import { IRegionTypePreview, IRegionTypesPreview } from "~/models";
import { apiCall } from "~/store";

const name = "REGION_TYPES";

export const regionTypesSliceInitialState: IRegionTypesPreview = {
  items: null,
};

enum ActionType {
  REGION_TYPES_REQUEST_SUCCEEDED = "REGION_TYPES_REQUEST_SUCCEEDED",
}

const slice = createSlice({
  name,
  initialState: regionTypesSliceInitialState,
  reducers: {
    [ActionType.REGION_TYPES_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IRegionTypesPreview>) => {
      state.items = action.payload.items as IRegionTypesPreview;
    },
  },
});

const { REGION_TYPES_REQUEST_SUCCEEDED } = slice.actions;

const getRegionTypes = (paramsPayload?: any) => {
  return apiCall(webApi.RegionTypes.getList)({
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
    success: REGION_TYPES_REQUEST_SUCCEEDED.type,
  });
};

const updateRegionTypes = (region: IRegionTypePreview) => {
  return apiCall(webApi.RegionTypes.updateRegionTypes)({
    args: [region],
  });
};

export const RegionTypesActions = {
  ...slice.actions,
  getRegionTypes,
  updateRegionTypes,
};

export default slice.reducer;
