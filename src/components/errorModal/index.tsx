import React, { useEffect, useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogContentText, Typography } from "@mui/material";
import { ReportProblemOutlined } from "@mui/icons-material";
import { CookieKeys } from "~/shared";
import { useCookie } from "react-use";

interface ErrorModalProps {
  title?: string;
  message?: any | string;
  error?: any;
}

const ErrorModal: React.FC<ErrorModalProps> = ({ title, message, error }) => {
  const [saiseToken] = useCookie(CookieKeys.SaiseToken);
  const [, , deleteToken] = useCookie(CookieKeys.AuthToken);
  const [, , deleteTokenRefresh] = useCookie(CookieKeys.AuthRefreshToken);
  const [, , deleteSaiseToken] = useCookie(CookieKeys.SaiseToken);
  const [, , deleteEmail] = useCookie(CookieKeys.ExternalUserEmail);
  const [, , deleteIdnp] = useCookie(CookieKeys.ExternalUserIdnp);
  const [errorModal, setErrorModal] = useState(false);
  const [timer, setTimer] = useState(5);
  const [customTitle, setCustomTitle] = useState("");
  const [customMessage, setCustomMessage] = useState("");

  function deleteCookie(name: any, domain: string) {
    document.cookie = `${name}=; domain=${domain}; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC`;
  }

  const setModal = (bool: boolean) => {
    if (error) {
      if (error.response.status === 401) {
        if (saiseToken) {
          deleteToken();
          deleteTokenRefresh();
          window.location.href = "https://siaadmin.dev.indrivo.com/";
          deleteCookie(CookieKeys.SaiseToken, ".dev.indrivo.com");
          deleteSaiseToken();
          return;
        }
        deleteToken();
        deleteEmail();
        deleteIdnp();
        deleteTokenRefresh();
        window.location.href = "/public";
      }
      if (error.config.url.includes("refresh") && error.response.status === 400) {
        setErrorModal(false);
        return;
      }
    }
    if (error && !bool) {
      if (error.config.url.includes("login/saiseadmin")) {
        window.location.href = "https://siaadmin.dev.indrivo.com/";
      }
      if (error.response.status === 404) {
        // window.history.back();
      }
    }
    setErrorModal(bool);
  };

  const handleError = () => {
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
      handleError();
    }
  }, [message]);

  useEffect(() => {
    if (title) {
      if (error) {
        if (error.config.url.includes("login/saiseadmin")) {
          setCustomTitle("Eroare de autorizare");
          setCustomMessage("S-a produs o eroare de conectare prin Saise, veți fi redirecționat prin");
          return;
        }
      }
      setCustomMessage(message);
      setCustomTitle(title);
    }
  }, [title]);

  return (
    <>
      <Dialog
        open={errorModal}
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
          <ReportProblemOutlined sx={{ color: "#D91414", fontSize: "45px", marginRight: "13px" }} />
          <Typography sx={{ fontSize: "24px", fontWeight: "bold", color: "#00305C" }}>{customTitle}</Typography>
        </DialogTitle>
        <DialogContent sx={{ padding: "0" }}>
          <DialogContentText id="alert-dialog-description" sx={{ color: "#00305C" }}>
            {customMessage} {error ? (error.response.status === 401 ? "" : timer) : timer}
          </DialogContentText>
        </DialogContent>
      </Dialog>
    </>
  );
};

export default ErrorModal;
