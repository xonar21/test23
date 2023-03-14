/* eslint-disable @typescript-eslint/no-explicit-any */
import { hydrate } from "./next-redux-wrapper.actions";

describe("Next Redux Wrapper Actions", () => {
  it("Hydrate should return correct type", () => {
    const action = hydrate({} as any);
    expect(action.type).toBe(hydrate.type);
  });
});
