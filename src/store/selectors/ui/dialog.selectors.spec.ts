/* eslint-disable @typescript-eslint/no-explicit-any */
import { DialogSelectors, dialogSliceInitialState } from "~/store";

describe("Posts selectors", () => {
  it("getRoot", () => {
    const root = DialogSelectors.getRoot({ UI: { DIALOG: dialogSliceInitialState } } as any);
    expect(root).toEqual(dialogSliceInitialState);
  });
});
