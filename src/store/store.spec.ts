import { store } from "~/store";

describe("Redux Store", () => {
  it("should create store with entities reducer", () => {
    const currentState = store.getState();
    expect(currentState).toHaveProperty("ENTITIES");
  });
});
