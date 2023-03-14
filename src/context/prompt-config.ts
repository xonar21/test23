/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { createContext } from "react";
import { IPromptConfig } from "~/core";

export interface IPromptConfigContextProps {
  promptConfig: IPromptConfig;
  setPromptConfig: React.Dispatch<React.SetStateAction<IPromptConfig>>;
}

export default createContext<IPromptConfigContextProps>({
  promptConfig: null as any,
  setPromptConfig: null as any,
});
