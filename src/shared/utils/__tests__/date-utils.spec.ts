import { diffByMinutes } from "~/shared";

describe("Date utils", () => {
  it("diffByMinutes returns correct value", () => {
    const date1 = new Date(2020, 10, 20, 20, 0, 0);
    const date2 = new Date(2020, 10, 20, 20, 30, 0);
    const expected = 30;

    expect(diffByMinutes(date1, date2)).toBe(expected);
  });
});
