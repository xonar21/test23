import { createContext } from "react";
import { IServiceCollection } from "~/core";
import { LogService } from "~/services";

export abstract class ServiceCollectionBuilder {
  public static initialize(): IServiceCollection {
    return {
      logService: new LogService(),
    };
  }
}

export default createContext<IServiceCollection>(ServiceCollectionBuilder.initialize());
