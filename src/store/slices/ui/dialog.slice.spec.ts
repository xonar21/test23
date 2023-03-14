import { DialogType } from "~/components";
import { dialogReducer, DialogSliceActions, IOpenDialogPayload, dialogSliceInitialState } from "~/store";

describe("Dialog Slice", () => {
  it("should return the initial state", () => {
    expect(dialogReducer(dialogSliceInitialState, { type: "TEST" })).toEqual(dialogSliceInitialState);
  });

  it("should handle OPEN_DIALOG", () => {
    const payload: IOpenDialogPayload = {
      title: "Test",
      dialogType: "TestDialog" as DialogType,
      dialogProps: {},
    };

    expect(dialogReducer(dialogSliceInitialState, { type: DialogSliceActions.OPEN_DIALOG.type, payload })).toEqual({
      ...dialogSliceInitialState,
      ...payload,
      open: true,
    });
  });

  it("should handle CLOSE_DIALOG", () => {
    expect(
      dialogReducer({ ...dialogSliceInitialState, open: true }, { type: DialogSliceActions.CLOSE_DIALOG }),
    ).toEqual(dialogSliceInitialState);
  });
});
