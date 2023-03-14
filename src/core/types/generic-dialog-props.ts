import { DialogProps } from "@mui/material";

export type GenericDialogProps = Omit<DialogProps, "open"> & {
  disableCloseOnBackdropClick?: boolean;
};
