import React, { useCallback, useMemo, useState } from "react";
import { PaletteMode, ThemeProvider as MuiThemeProvider } from "@mui/material";
import { createCustomTheme } from "~/styles";
import { ThemeModeContext } from "~/context";
import { LocalStorageKeys } from "~/shared";

const ThemeProvider: React.FC = ({ children }) => {
  const [mode, setMode] = useState<PaletteMode>("light");

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
