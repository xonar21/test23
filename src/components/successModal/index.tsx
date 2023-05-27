import React, { useEffect, useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogContentText, Typography } from "@mui/material";
import { DoneAllOutlined } from "@mui/icons-material";

interface SuccessModalProps {
  title?: string;
  message?: any | string;
}

const SuccessModal: React.FC<SuccessModalProps> = ({ title, message }) => {
  const [successModal, setSuccessModal] = useState(false);
  const [timer, setTimer] = useState(5);
  const [customTitle, setCustomTitle] = useState("");
  const [customMessage, setCustomMessage] = useState("");

  const setModal = (bool: boolean) => {
    setSuccessModal(bool);
  };

  const handleSuccess = () => {
    setModal(true);

    const countdown = setInterval(() => {
      setTimer(prevTimer => prevTimer - 1);
    }, 1000);

    setTimeout(() => {
      clearInterval(countdown);
      setModal(false);
    }, 5000);
  };

  useEffect(() => {
    if (message) {
      handleSuccess();
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
        open={successModal}
        className="turn"
        onClose={() => setModal(false)}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        sx={{ ".MuiPaper-root": { padding: "40px" } }}
      >
        <DialogTitle
          id="alert-dialog-title"
          sx={{ display: "flex", padding: "0", alignItems: "end", marginBottom: "20px" }}
        >
          <DoneAllOutlined sx={{ color: "#027333", fontSize: "45px", marginRight: "13px" }} />
          <Typography sx={{ fontSize: "24px", fontWeight: "bold", color: "#00305C" }}>{customTitle}</Typography>
        </DialogTitle>
        <DialogContent sx={{ padding: "0" }}>
          <DialogContentText id="alert-dialog-description" sx={{ color: "#00305C" }}>
            {customMessage} {timer}
          </DialogContentText>
        </DialogContent>
      </Dialog>
    </>
  );
};

export default SuccessModal;
