import React, { useCallback, useEffect, useMemo, useState } from "react";
import { PaletteMode, ThemeProvider as MuiThemeProvider, useMediaQuery } from "@mui/material";
import { createCustomTheme } from "~/styles";
import { ThemeModeContext } from "~/context";
import { LocalStorageKeys } from "~/shared";

const ThemeProvider: React.FC = ({ children }) => {
  const prefersDarkMode = useMediaQuery("(prefers-color-scheme: dark)");
  const [mode, setMode] = useState<PaletteMode>("light");

  useEffect(() => {
    const localStorageValue = localStorage.getItem(LocalStorageKeys.ThemeMode) as PaletteMode;
    setMode(localStorageValue ? localStorageValue : prefersDarkMode ? "dark" : "light");
  }, [prefersDarkMode]);

  const toggleThemeMode = useCallback(() => {
    setMode(previous => {
      const value = previous === "light" ? "dark" : "light";
      localStorage.setItem(LocalStorageKeys.ThemeMode, value);
      return value;
    });
  }, []);

  const theme = useMemo(() => createCustomTheme(mode), [mode]);

  return (
    <ThemeModeContext.Provider value={{ mode, toggleThemeMode }}>
      <MuiThemeProvider theme={theme}>{children}</MuiThemeProvider>
    </ThemeModeContext.Provider>
  );
};

export default ThemeProvider;
