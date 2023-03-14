/* eslint-disable @typescript-eslint/no-explicit-any */
import { AuthSelectors, authSliceInitialState } from "~/store";

describe("Auth selectors", () => {
  it("getRoot", () => {
    const root = AuthSelectors.getRoot({ AUTH: authSliceInitialState } as any);
    expect(root).toEqual(authSliceInitialState);
  });
});
