/* eslint-disable @typescript-eslint/no-explicit-any */
import { apiCallInitAction, apiCallSuccessAction, apiCallFailureAction, apiCall } from "~/store";

describe("Redux Api Actions", () => {
  it("Call Init should return correct type", () => {
    const action = apiCallInitAction({} as any);
    expect(action.type).toBe(apiCallInitAction.type);
  });

  it("Call Success should return correct type", () => {
    const action = apiCallSuccessAction({} as any);
    expect(action.type).toBe(apiCallSuccessAction.type);
  });

  it("Call Failure should return correct type", () => {
    const action = apiCallFailureAction({} as any);
    expect(action.type).toBe(apiCallFailureAction.type);
  });

  it("Api Call creates an Call Init action", () => {
    const mockedApi = () => new Promise(jest.fn());
    const action = apiCall(mockedApi)({ args: [null] });
    expect(action.type).toBe(apiCallInitAction.type);
  });
});
