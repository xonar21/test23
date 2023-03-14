import {
  ActionCreatorWithoutPayload,
  ActionCreatorWithPayload,
  createSlice,
  Draft,
  PayloadAction,
  SliceCaseReducers,
  ValidateSliceCaseReducers,
} from "@reduxjs/toolkit";
import { GridSelectionModel } from "@mui/x-data-grid";
import { IAxiosErrorPayload, Nullable } from "~/core";
import { IPageRequest, IResponse } from "~/models";
import { apiCall, RootState, hydrate } from "~/store";

export interface IPaginationSliceState<T> extends IResponse<T> {
  loading: boolean;
  error: Nullable<IAxiosErrorPayload>;
  selectedRows: GridSelectionModel;
}

enum ActionType {
  LIST_REQUEST_STARTED = "LIST_REQUEST_STARTED",
  LIST_REQUEST_SUCCEEDED = "LIST_REQUEST_SUCCEEDED",
  LIST_REQUEST_FAILED = "LIST_REQUEST_FAILED",
  LIST_REQUEST_ENDED = "LIST_REQUEST_ENDED",
  PAGE_SIZE_CHANGED = "PAGE_SIZE_CHANGED",
  PAGE_CHANGED = "PAGE_CHANGED",
  SELECTED_ROWS_CHANGED = "SELECTED_ROWS_CHANGED",
}

export const getInitialPaginationSliceState = <TEntity>(): IPaginationSliceState<TEntity> => {
  return {
    data: [],
    page: 0,
    limit: 10,
    total: 0,
    loading: false,
    error: null,
    selectedRows: [],
  };
};

export const createPaginationSlice = <
  TState,
  TEntity,
  Reducers extends SliceCaseReducers<IPaginationSliceState<TEntity> & TState>,
>({
  name,
  initialState,
  reducers,
  api,
}: {
  name: string;
  initialState: IPaginationSliceState<TEntity> & TState;
  reducers: ValidateSliceCaseReducers<IPaginationSliceState<TEntity> & TState, Reducers>;
  api: (params: IPageRequest) => Promise<IResponse<TEntity>>;
}) => {
  const slice = createSlice({
    name,
    initialState: {
      ...getInitialPaginationSliceState<TEntity>(),
      ...initialState,
    },
    reducers: {
      [ActionType.LIST_REQUEST_STARTED]: state => {
        state.loading = true;
      },
      [ActionType.LIST_REQUEST_SUCCEEDED]: (state, action: PayloadAction<IResponse<TEntity>>) => {
        state.data = action.payload.data as Draft<TEntity[]>;
        state.page = action.payload.page;
        state.limit = action.payload.limit;
        state.total = action.payload.total;
      },
      [ActionType.LIST_REQUEST_FAILED]: (state, action: PayloadAction<IAxiosErrorPayload>) => {
        state.error = action.payload;
      },
      [ActionType.LIST_REQUEST_ENDED]: state => {
        state.loading = false;
      },
      [ActionType.PAGE_SIZE_CHANGED]: (state, action: PayloadAction<number>) => {
        state.limit = action.payload;
      },
      [ActionType.PAGE_CHANGED]: (state, action: PayloadAction<number>) => {
        state.page = action.payload;
      },
      [ActionType.SELECTED_ROWS_CHANGED]: (state, action: PayloadAction<GridSelectionModel>) => {
        state.selectedRows = action.payload;
      },
      ...reducers,
    },
    extraReducers: builder => {
      builder.addCase(hydrate, (state, action) => {
        return {
          ...state,
          ...action.payload.ENTITIES[name as keyof RootState["ENTITIES"]],
        };
      });
    },
  });

  const { LIST_REQUEST_STARTED, LIST_REQUEST_SUCCEEDED, LIST_REQUEST_FAILED, LIST_REQUEST_ENDED } = slice.actions;

  const getList = (params: IPageRequest) => {
    return apiCall(api)({
      args: [params],
      start: (LIST_REQUEST_STARTED as ActionCreatorWithoutPayload).type,
      success: (LIST_REQUEST_SUCCEEDED as ActionCreatorWithPayload<IResponse<TEntity>>).type,
      failure: (LIST_REQUEST_FAILED as ActionCreatorWithPayload<IAxiosErrorPayload>).type,
      end: (LIST_REQUEST_ENDED as ActionCreatorWithoutPayload).type,
    });
  };

  return { slice, paginationActions: { getList } };
};

export const createPaginationSliceSelectors = <TEntity>(root: (state: RootState) => IPaginationSliceState<TEntity>) => {
  return {
    root,
  };
};

/* istanbul ignore next */
class CreatePaginationSliceSelectorsWrapper<TEntity> {
  create(root: (state: RootState) => IPaginationSliceState<TEntity>) {
    return createPaginationSliceSelectors<TEntity>(root);
  }
}

export type PaginationActions = ReturnType<typeof createPaginationSlice>["paginationActions"] &
  ReturnType<typeof createPaginationSlice>["slice"]["actions"];
export type PaginationSelectors<TEntity> = ReturnType<CreatePaginationSliceSelectorsWrapper<TEntity>["create"]>;
