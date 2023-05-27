import React, { useState } from "react";
import { TextField, Typography } from "@mui/material";
import { useDispatch } from "react-redux";
import { useTranslation } from "next-i18next";
import { VoterActions } from "~/store";
import LoadingButton from "@mui/lab/LoadingButton";
import ReactDOM from "react-dom";
import SuccessModal from "../successModal";

const SubscribeToNotification: React.FC = () => {
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);
  const dispatch = useDispatch();
  const { t } = useTranslation();

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(event.target.value);
  };

  const showSuccessModal = (title: string, message: string) => {
    const successModal = document.createElement("div");
    document.body.appendChild(successModal);

    ReactDOM.render(React.createElement(SuccessModal, { title, message }), successModal);
  };

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    setLoading(true);
    try {
      const response = await dispatch(VoterActions.postEmailAttach({ email }));
      if (response.succeeded) {
        showSuccessModal("Succes", "Mesaj trimis cu succes");
      }
    } catch (error) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <TextField
        label={t("email")}
        variant="outlined"
        type="email"
        value={email}
        onChange={handleChange}
        required
        size="small"
        InputLabelProps={{ sx: { fontSize: "14px" } }}
        inputProps={{ sx: { fontSize: "14px", lineHeight: "16px", fontWeight: "normal" } }}
        sx={{ height: "32px", ".MuiInputBase-root": { height: "32px" }, marginRight: "20px" }}
      />
      <LoadingButton
        type="submit"
        variant="contained"
        color="primary"
        loading={loading}
        sx={{
          height: "32px",
          border: loading ? "1px solid transparent" : "1px solid #00305C",
          "&:hover": { backgroundColor: "white", color: "#00305C" },
        }}
      >
        <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "normal" }}>{t("confirm")}</Typography>
      </LoadingButton>
    </form>
  );
};

export default SubscribeToNotification;
