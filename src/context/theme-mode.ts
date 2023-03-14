import { PaletteMode } from "@mui/material";
import { createContext } from "react";

interface IThemeModeContextProps {
  mode: PaletteMode;
  toggleThemeMode: () => void;
}

export default createContext<IThemeModeContextProps>({
  mode: "light",
  toggleThemeMode: /* istanbul ignore next */ () => {
    return;
  },
});
