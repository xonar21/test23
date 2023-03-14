import { isArrayNullOrEmpty } from "~/shared";

describe("Array utils", () => {
  it("isArrayNullOrEmpty returns true on passing empty array", () => {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const emptyArray: any[] = [];
    expect(isArrayNullOrEmpty(emptyArray)).toBeTruthy();
  });

  it("isArrayNullOrEmpty returns false on passing array with values", () => {
    const array = [1, "hello", { id: 1 }];
    expect(isArrayNullOrEmpty(array)).toBeFalsy();
  });
});
