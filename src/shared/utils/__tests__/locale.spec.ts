import { translate } from "~/shared";

jest.mock("next-i18next", () => ({
  i18n: {
    t: jest.fn(str => str),
  },
}));

describe("Locale utils", () => {
  it("translate function", () => {
    const key = "test";
    const value = translate(key);
    expect(value).toBe(key);
  });
});
