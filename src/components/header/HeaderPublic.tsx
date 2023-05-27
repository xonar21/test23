import React from "react";
import Image from "next/image";
import { AppBar, Toolbar, Box, Typography } from "@mui/material";

import { AuthButton, LocaleSwitch } from "~/components";
import { testIds } from "~/shared";
import { useTranslation } from "next-i18next";

const HeaderPublic: React.FC = () => {
  const { t } = useTranslation(["common"]);
  return (
    <>
      <AppBar
        position="fixed"
        data-testid={testIds.components.header.root}
        sx={({ palette }) => ({
          backgroundColor: palette.primary.main,
          color: palette.primary.main,
          width: "100%",
        })}
      >
        <Toolbar sx={{ p: ({ spacing }) => `${spacing(0, 2.5)} !important`, color: "white" }}>
          <Box sx={{ display: "flex", alignItems: "center" }}>
            <Image src="/svg/cec-logo.svg" alt="CEC Logo" width={35} height={35} />
            <Typography variant="subtitle1" sx={{ ml: 0.625, whiteSpace: "normal", maxWidth: 152 }}>
              {t("brandName")}
            </Typography>
          </Box>

          <Box mx={1.688} sx={{ marginLeft: "auto" }}>
            <LocaleSwitch />
          </Box>
          <AuthButton />
        </Toolbar>
      </AppBar>
    </>
  );
};

export default HeaderPublic;
