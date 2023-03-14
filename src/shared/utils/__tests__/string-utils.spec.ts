import { replaceAtIndex } from "~/shared";

describe("String utils", () => {
  it("replaceAtIndex returns replaced string on passing all params", () => {
    const sourceString = "myString";
    const replacement = "ReplacedString";
    const index = 2;
    const expected = "myReplacedString";
    expect(replaceAtIndex(index, replacement, sourceString)).toBe(expected);
  });
});
