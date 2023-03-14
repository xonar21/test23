import { useContext } from "react";
import { useDispatch, useSelector } from "react-redux";
import { DialogType } from "~/components";
import { PromptConfigContext } from "~/context";
import { IOpenDialogPayload, IOpenPromptPayload } from "~/core";
import { DialogSelectors, DialogSliceActions } from "~/store";

const PROMPT_DIALOG_KEY = "PromptDialog" as const;
type PromptDialog = typeof PROMPT_DIALOG_KEY;

interface IOpenDialogConfig extends IOpenDialogPayload {
  dialogType: Exclude<DialogType, PromptDialog>;
}

const useDialog = () => {
  const dispatch = useDispatch();
  const { promptConfig, setPromptConfig } = useContext(PromptConfigContext);
  const { open, dialogType, title, dialogProps } = useSelector(DialogSelectors.getRoot);

  const openDialog = (config: IOpenDialogConfig) => {
    dispatch(DialogSliceActions.OPEN_DIALOG(config));
  };

  /* istanbul ignore next */
  const openPrompt = (config: IOpenPromptPayload) => {
    const { title, dialogProps, ...promptConfig } = config;

    setPromptConfig(promptConfig);

    dispatch(
      DialogSliceActions.OPEN_DIALOG({
        dialogType: PROMPT_DIALOG_KEY,
        dialogProps,
        title,
      }),
    );
  };

  const closeDialog = (_?: unknown, reason?: "backdropClick" | "escapeKeyDown") => {
    if (dialogProps.disableCloseOnBackdropClick && reason === "backdropClick") return;
    dispatch(DialogSliceActions.CLOSE_DIALOG());
  };

  return { openDialog, openPrompt, closeDialog, open, dialogType, title, dialogProps, promptConfig };
};

export default useDialog;
