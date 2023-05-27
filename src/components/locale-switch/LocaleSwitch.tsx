import React, { useState } from "react";
import { useRouter } from "next/router";
import { useTranslation } from "next-i18next";
import { Button, Menu, MenuItem, Typography } from "@mui/material";
import Image from "next/image";
import { Nullable } from "~/core";
import { Locale, testIds } from "~/shared";

type LocaleSwitchProps = {
  variant?: "primary" | "white";
};

const LocaleSwitch: React.FC<LocaleSwitchProps> = ({ variant = "primary" }) => {
  const { t, i18n } = useTranslation();
  const router = useRouter();
  const [anchorEl, setAnchorEl] = useState<Nullable<HTMLButtonElement>>(null);
  const open = Boolean(anchorEl);

  const handleLocaleChange = (locale: Locale) => {
    router.push(router.pathname, router.pathname, { locale });
    setAnchorEl(null);
  };

  return (
    <>
      <Button
        aria-expanded={open}
        aria-haspopup="true"
        color="inherit"
        onClick={e => setAnchorEl(e.currentTarget)}
        data-testid={testIds.components.header.buttons.localeMenu}
        sx={{ p: 0.813 }}
      >
        <Typography sx={{ textTransform: "capitalize", mr: 0.625 }}>{i18n.language}</Typography>
        <Image src={`/svg/locale-${i18n.language}-${variant}-border.svg`} alt={i18n.language} width={20} height={20} />
      </Button>
      <Menu anchorEl={anchorEl} open={open} onClose={() => setAnchorEl(null)}>
        {Object.values(Locale).map(locale => (
          <MenuItem
            key={locale}
            onClick={() => handleLocaleChange(locale)}
            data-testid={testIds.components.header.localeItem}
          >
            <Typography variant="subtitle1" color="primary" fontWeight={locale === i18n.language ? "bold" : "inherit"}>
              {t(`locales.${locale}`)}
            </Typography>
          </MenuItem>
        ))}
      </Menu>
    </>
  );
};

export default LocaleSwitch;
