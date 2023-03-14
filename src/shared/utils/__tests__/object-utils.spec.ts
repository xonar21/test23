import { getKeyByValue, getValueByPath, getValuePath } from "~/shared";

describe("Object utils", () => {
  it("getKeyByValue returns a valid key on passing existing object value", () => {
    const obj = {
      id: 1,
      title: "Test",
      nested: {
        prop1: "Test 2",
      },
    };
    const searchValue = {
      prop1: "Test 2",
    };
    const expected = "nested";
    expect(getKeyByValue(obj, searchValue)).toBe(expected);
  });

  it("getValuePath returns a valid path array on passing existing object value", () => {
    const obj = {
      id: 1,
      title: "Test 1",
      nested: {
        prop1: 15,
        prop2: {
          prop3: "Test 2",
        },
      },
    };
    const searchValue = obj.nested.prop2;
    const expected = ["nested", "prop2"];
    expect(getValuePath(obj, searchValue)).toStrictEqual(expected);
  });

  it("getValuePath returns an empty array on passing unexisting object value", () => {
    const obj = {
      id: 1,
      title: "Test 1",
      nested: {
        prop1: 15,
        prop2: {
          prop3: "Test 2",
        },
      },
    };
    const searchValue = "Other";
    const expected: Array<string> = [];
    expect(getValuePath(obj, searchValue)).toStrictEqual(expected);
  });

  it("getValueByPath returns the correct value on passing a valid path", () => {
    const obj = {
      id: 1,
      title: "Test 1",
      nested: {
        prop1: 15,
        prop2: {
          prop3: "Test 2",
        },
      },
    };
    const stringPath = "nested.prop2.prop3";
    const arrayPath = ["nested", "prop2", "prop3"];
    const expected = "Test 2";
    expect(getValueByPath(obj, stringPath)).toBe(expected);
    expect(getValueByPath(obj, arrayPath)).toBe(expected);
  });
});
