import { createTheme, PaletteMode } from "@mui/material";

const createCustomTheme = (mode: PaletteMode) =>
  createTheme({
    palette: {
      mode,
    },
  });

export default createCustomTheme;
