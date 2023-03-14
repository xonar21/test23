import React, { useState } from "react";
import { Dialog, DialogTitle } from "@mui/material";
import { DialogTypes } from "~/components";
import { useDialog } from "~/hooks";
import { IPromptConfigContextProps, PromptConfigContext } from "~/context";
import { IPromptConfig } from "~/core";

const DialogProvider: React.FC = ({ children }) => {
  const {
    open,
    dialogType,
    title,
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    dialogProps: { disableCloseOnBackdropClick, ...props },
    closeDialog,
  } = useDialog();
  const [promptConfig, setPromptConfig] = useState<IPromptConfig>();

  const Content = dialogType && DialogTypes[dialogType];

  return (
    <PromptConfigContext.Provider
      value={
        {
          promptConfig,
          setPromptConfig,
        } as IPromptConfigContextProps
      }
    >
      {children}
      <Dialog open={open} onClose={closeDialog} scroll="paper" {...props}>
        <DialogTitle>{title}</DialogTitle>
        {Content && <Content />}
      </Dialog>
    </PromptConfigContext.Provider>
  );
};

export default DialogProvider;
