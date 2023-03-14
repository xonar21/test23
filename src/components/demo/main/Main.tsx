import React from "react";
import { Box, Typography } from "@mui/material";
import { useTranslation } from "next-i18next";

const Main: React.FC = () => {
  const { t } = useTranslation(["common", "home"]);

  return (
    <Box sx={{ textAlign: "center", bgcolor: "background.default" }} py={5} mb={3} mx={-3}>
      <Typography variant="h2" color="text.primary" fontWeight="bold">
        {t("brandName")}
      </Typography>
      <Typography variant="subtitle1" color="text.primary" variantMapping={{ subtitle1: "h3" }}>
        {t("home:subtitle")}
      </Typography>
    </Box>
  );
};

export default Main;
