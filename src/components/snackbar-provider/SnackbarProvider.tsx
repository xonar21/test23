import React, { createRef } from "react";
import { SnackbarKey, SnackbarProvider as NotistackSnackbarProvider } from "notistack";
import { IconButton } from "@mui/material";
import { Close as CloseIcon } from "@mui/icons-material";

const SnackbarProvider: React.FC = ({ children }) => {
  const notistackRef = createRef<NotistackSnackbarProvider>();

  /* istanbul ignore next */
  const handleSnackbarDismiss = (key: SnackbarKey) => {
    notistackRef.current?.closeSnackbar(key);
  };

  return (
    <NotistackSnackbarProvider
      ref={notistackRef}
      action={
        /* istanbul ignore next */ (key: SnackbarKey) => (
          <IconButton color="inherit" size="small" onClick={() => handleSnackbarDismiss(key)}>
            <CloseIcon />
          </IconButton>
        )
      }
      anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
      maxSnack={3}
    >
      {children}
    </NotistackSnackbarProvider>
  );
};

export default SnackbarProvider;
