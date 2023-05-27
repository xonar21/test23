import React, { useEffect, useState } from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  Typography,
  Button,
  DialogActions,
} from "@mui/material";
import { InfoOutlined } from "@mui/icons-material";

interface WarningModalProps {
  title?: string;
  message?: any | string;
  onConfirm?: any;
}

const WarningModal: React.FC<WarningModalProps> = ({ title, message, onConfirm }) => {
  const [warningModal, setWarningModal] = useState(false);
  const [timer, setTimer] = useState(5);
  const [customTitle, setCustomTitle] = useState("");
  const [customMessage, setCustomMessage] = useState("");

  const setModal = (bool: boolean) => {
    setWarningModal(bool);
  };

  // const handleSuccess = () => {
  //   setModal(true);

  //   const countdown = setInterval(() => {
  //     setTimer(prevTimer => prevTimer - 1);
  //   }, 1000);

  //   setTimeout(() => {
  //     clearInterval(countdown);
  //     setModal(false);
  //   }, 5000);
  // };

  useEffect(() => {
    if (message) {
      // handleSuccess();
      setModal(true);
    }
  }, [message]);

  useEffect(() => {
    if (title) {
      setCustomMessage(message);
      setCustomTitle(title);
    }
  }, [title]);

  return (
    <>
      <Dialog
        open={warningModal}
        className="turn"
        onClose={() => setModal(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        sx={{ ".MuiPaper-root": { padding: "40px", paddingBottom: "20px" } }}
      >
        <DialogTitle
          id="alert-dialog-title"
          sx={{ display: "flex", padding: "0", alignItems: "end", marginBottom: "20px" }}
        >
          <InfoOutlined sx={{ color: "#FFBD4C", fontSize: "45px", marginRight: "13px" }} />
          <Typography sx={{ fontSize: "24px", fontWeight: "bold", color: "#00305C" }}>{customTitle}</Typography>
        </DialogTitle>
        <DialogContent sx={{ padding: "0" }}>
          <DialogContentText id="alert-dialog-description" sx={{ color: "#00305C" }}>
            {customMessage}
          </DialogContentText>
        </DialogContent>
        <DialogActions sx={{ padding: "0", marginTop: "40px" }}>
          <Button
            onClick={() => setModal(false)}
            sx={{
              marginRight: "auto",
              width: "120px",
              height: "32px",
              color: "#00305C",
              borderRadius: "3px",
              border: "1px solid #00305E",
              textTransform: "none",
              fontSize: "16px",
              lineHeight: "19px",
              fontWeight: "normal",
            }}
          >
            Anuleaza
          </Button>
          <Button
            onClick={() => {
              setModal(false);
              return onConfirm();
            }}
            color="primary"
            autoFocus
            sx={{
              width: "120px",
              height: "32px",
              background: "#00305C",
              color: "white",
              borderRadius: "3px",
              border: "1px solid #00305E",
              textTransform: "none",
              fontSize: "16px",
              lineHeight: "19px",
              fontWeight: "normal",
              "&:hover": {
                background: "white",
                color: "#00305C",
              },
            }}
          >
            Confirma
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default WarningModal;
