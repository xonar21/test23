import { Box, Typography } from "@mui/material";
import { useTranslation } from "next-i18next";
import React from "react";
import { HeaderPublic } from "~/components";

const PublicLayout: React.FC = ({ children }) => {
  const { t } = useTranslation(["common"]);
  return (
    <React.Fragment>
      <HeaderPublic />
      <Box sx={{ paddingTop: "85px", height: "100vh", background: "rgba(233, 235, 236, 1)" }}>
        <Box sx={{ height: "130px", display: "flex", justifyContent: "center", alignItems: "center" }}>
          <Typography
            variant="h1"
            sx={({ palette }) => ({
              color: palette.primary.main,
              fontSize: "32px",
            })}
          >
            {t("welcomeTitle")}
          </Typography>
        </Box>
        <Box
          sx={{
            width: "90%",
            minHeight: "600px",
            margin: "auto",
            boxShadow: "0px 0px 20px rgba(0,0,0,0.3)",
            background: "white",
            borderRadius: "15px",
            padding: "40px",
          }}
        >
          {children}
        </Box>
      </Box>
    </React.Fragment>
  );
};

export default PublicLayout;
