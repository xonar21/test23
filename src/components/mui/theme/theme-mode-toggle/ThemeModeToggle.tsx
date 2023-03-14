import React, { useContext } from "react";
import { IconButton } from "@mui/material";
import { Brightness3 as MoonIcon, WbSunny as SunIcon } from "@mui/icons-material";
import { ThemeModeContext } from "~/context";
import { testIds } from "~/shared";

const ThemeModeToggle = () => {
  const { mode, toggleThemeMode } = useContext(ThemeModeContext);

  return (
    <IconButton
      size="large"
      color="inherit"
      aria-label="dark-mode-toggle"
      onClick={toggleThemeMode}
      data-testid={testIds.components.header.buttons.themeToggler}
    >
      {mode === "light" ? <MoonIcon /> : <SunIcon />}
    </IconButton>
  );
};

export default ThemeModeToggle;
