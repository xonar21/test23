import React from "react";
import { Dialog, DialogContent, DialogActions, Button, Box, DialogTitle, IconButton, Typography } from "@mui/material";
import { styleButton } from "../../styles/defaultStyleButtonForDialog";
import { useTranslation } from "next-i18next";
import { CloseOutlined, InfoOutlined, SaveOutlined } from "@mui/icons-material";

interface DefaultDialogActionProps {
  openModal: any;
  handleClose: any;
  dynamicTitle: any;
  handleSave?: any;
  content: React.ReactNode;
  styledContent?: any;
  loading?: boolean;
  customActions?: any;
}

const DialogImport: React.FC<DefaultDialogActionProps> = ({
  openModal,
  handleClose,
  dynamicTitle,
  handleSave,
  content,
  styledContent,
  loading,
  customActions,
}) => {
  const { t } = useTranslation();
  return (
    <Dialog
      PaperProps={{ sx: { backgroundColor: "white" } }}
      open={openModal}
      onClose={handleClose}
      maxWidth="sm"
      fullWidth
    >
      <DialogTitle sx={{ color: "rgba(0, 48, 92, 1)", position: "relative", padding: "20px 40px 10px" }}>
        <Box>
          <Box>
            {dynamicTitle ? dynamicTitle() : ""}
            <IconButton
              edge="end"
              color="inherit"
              onClick={handleClose}
              aria-label="close"
              sx={{ position: "absolute", right: "40px", top: "7px" }}
            >
              <CloseOutlined sx={{ color: "black" }} />
            </IconButton>
          </Box>
          <Box
            sx={{ display: dynamicTitle() === t("import") ? "flex" : "none", alignItems: "center", marginTop: "7px" }}
          >
            <InfoOutlined sx={{ fontSize: "20px", color: "#D4D7D9", marginRight: "7px" }} />
            <Typography sx={{ fontSize: "12px", lineHeight: "14px", fontWeight: "normal", color: "#D4D7D9" }}>
              Selectați scrutin pentru importare:
            </Typography>
          </Box>
        </Box>
      </DialogTitle>

      <DialogContent
        sx={{
          ...styledContent,
          paddingTop: "10px !important",
          maxHeight: "560px",
          overflowY: "auto",
          padding: "10px 40px 0px",
          "&::-webkit-scrollbar": {
            width: "4px",
          },
          "&::-webkit-scrollbar-thumb": {
            backgroundColor: "rgba(0, 48, 92, 1)",
            borderRadius: "4px",
          },
          "&::-webkit-scrollbar-track": {
            backgroundColor: "rgba(230, 230, 230, 1)",
          },
        }}
      >
        {content}
      </DialogContent>
      {loading ? (
        <></>
      ) : customActions ? (
        customActions
      ) : (
        <DialogActions sx={{ padding: "10px 40px 20px" }}>
          <Button onClick={handleClose} color="secondary" sx={styleButton.buttonExit}>
            <CloseOutlined sx={{ marginRight: "5px" }} />
            <Typography sx={{ fontSize: "16px", lineHeight: "19px", fontWeight: "normal" }}>{t("close")}</Typography>
          </Button>
          <Box sx={{ flexGrow: 1 }} />
          <Button onClick={() => handleSave()} color="primary" sx={styleButton.buttonConfirm}>
            <SaveOutlined sx={{ marginRight: "5px" }} />
            <Typography sx={{ fontSize: "16px", lineHeight: "19px", fontWeight: "normal" }}>{t("save")}</Typography>
          </Button>
        </DialogActions>
      )}
    </Dialog>
  );
};

export default DialogImport;
