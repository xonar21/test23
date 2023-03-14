import { LogService } from "~/services";

describe("Log Service", () => {
  const service = new LogService();
  const consoleLogSpy = jest.spyOn(console, "log");

  it("logs message", () => {
    service.log("Logger test message.");
    expect(consoleLogSpy).toBeCalledTimes(1);
  });
});
