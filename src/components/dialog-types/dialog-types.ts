import { PromptDialog, PostFormDialog } from "~/components";

const dialogTypes = {
  PromptDialog,
  PostFormDialog,
};

export type DialogType = keyof typeof dialogTypes;

export default dialogTypes;
