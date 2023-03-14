import React from "react";
import { DialogType } from "~/components";
import { GenericDialogProps } from "~/core";

export interface IOpenDialogPayload {
  title: string;
  dialogType: DialogType;
  dialogProps: GenericDialogProps;
}

export interface IPromptConfig {
  content: React.ElementType;
  onConfirm?: (
    setConfirmationDisabled: React.Dispatch<React.SetStateAction<boolean>>,
  ) => void | boolean | Promise<void | boolean>;
  onReject?: () => void;
}

export interface IOpenPromptPayload extends IPromptConfig {
  title: string;
  dialogProps: GenericDialogProps;
}
