import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { DialogType } from "~/components";
import { GenericDialogProps, IOpenDialogPayload, Nullable } from "~/core";
import { WritableDraft } from "immer/dist/internal";

export interface IDialogSliceState {
  open: boolean;
  title: string;
  dialogType: Nullable<DialogType>;
  dialogProps: GenericDialogProps;
}

enum DialogSliceActionType {
  OPEN_DIALOG = "OPEN_DIALOG",
  CLOSE_DIALOG = "CLOSE_DIALOG",
}

const name = "DIALOG";

export const dialogSliceInitialState: IDialogSliceState = {
  open: false,
  title: "",
  dialogType: null,
  dialogProps: {},
};

const slice = createSlice({
  name,
  initialState: dialogSliceInitialState,
  reducers: {
    [DialogSliceActionType.OPEN_DIALOG]: (state, action: PayloadAction<IOpenDialogPayload>) => {
      state.title = action.payload.title;
      state.dialogType = action.payload.dialogType;
      state.dialogProps = action.payload.dialogProps as WritableDraft<GenericDialogProps>;
      state.open = true;
    },
    [DialogSliceActionType.CLOSE_DIALOG]: state => {
      state.open = false;
    },
  },
});

export const DialogSliceActions = slice.actions;

export default slice.reducer;
