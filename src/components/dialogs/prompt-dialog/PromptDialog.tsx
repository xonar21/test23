import React, { useState } from "react";
import { useTranslation } from "next-i18next";
import { Button, DialogActions, DialogContent } from "@mui/material";
import { useDialog } from "~/hooks";
import { testIds } from "~/shared";
import { LoadingButton } from "~/components";

const PromptDialog = () => {
  const { t } = useTranslation();
  const { promptConfig, closeDialog } = useDialog();
  const [loading, setLoading] = useState(false);

  const handleOnConfirm = async () => {
    const succeeded = await promptConfig.onConfirm?.(setLoading);
    setLoading(false);
    if (succeeded !== false) closeDialog();
  };

  const handleOnReject = async () => {
    await promptConfig.onReject?.();
    closeDialog();
  };

  return (
    <React.Fragment>
      <DialogContent dividers>
        <promptConfig.content />
      </DialogContent>
      <DialogActions>
        <Button data-testid={testIds.components.dialogs.promptDialog.rejectButton} onClick={handleOnReject}>
          {t("common:cancel")}
        </Button>
        <LoadingButton
          data-testid={testIds.components.dialogs.promptDialog.confirmButton}
          variant="contained"
          loading={loading}
          onClick={handleOnConfirm}
        >
          {t("common:confirm")}
        </LoadingButton>
      </DialogActions>
    </React.Fragment>
  );
};

export default PromptDialog;
