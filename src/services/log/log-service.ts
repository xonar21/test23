import { ILogService } from "~/core";

export class LogService implements ILogService {
  public log(message: string): void {
    console.log(message);
  }
}
