import React from "react";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";

interface ErrorDialogProps {
  open: boolean;
  onClose: () => void;
  errorText: string;
}

const ErrorDialog: React.FC<ErrorDialogProps> = ({ open, onClose, errorText }) => {
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>{"Ошибка"}</DialogTitle>
      <DialogContent>
        <DialogContentText>{errorText}</DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary">
          Закрыть
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default ErrorDialog;
