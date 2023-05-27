import { createTheme, PaletteMode } from "@mui/material";

const createCustomTheme = (mode: PaletteMode) =>
  createTheme({
    palette: {
      mode,
      primary: {
        main: "#00305C",
        dark: "#101830",
      },
      error: {
        main: "#FF4A4B",
      },
      success: {
        main: "#00C04C",
      },
      warning: {
        main: "#FFBF62",
      },
    },
    mixins: {
      toolbar: {
        minHeight: 80,
      },
    },
    shape: {
      borderRadius: 3,
    },
    typography: {
      title: {
        fontSize: 24,
        lineHeight: "28px",
      },
      h1: {
        fontSize: 22,
        lineHeight: "25px",
        fontWeight: "bold",
      },
      h2: {
        fontSize: 20,
        lineHeight: "24px",
        fontWeight: "bold",
      },
      h3: {
        fontSize: 18,
        lineHeight: "21px",
        fontWeight: "bold",
      },
      h4: {
        fontSize: 16,
        lineHeight: "19px",
        fontWeight: "bold",
      },
      h5: {
        fontSize: 14,
        lineHeight: "16px",
        fontWeight: "bold",
      },
      h6: {
        fontSize: 12,
        lineHeight: "14px",
        fontWeight: "bold",
      },
      body1: {
        fontSize: 16,
        lineHeight: "19px",
      },
      body2: {
        fontSize: 14,
        lineHeight: "16px",
      },
      subtitle1: {
        fontSize: 12,
        lineHeight: "14px",
      },
      subtitle2: {
        fontSize: 10,
        lineHeight: "11px",
      },
      button: {
        fontSize: 16,
        lineHeight: "19px",
        fontWeight: "normal",
      },
    },
    components: {
      MuiAppBar: {
        styleOverrides: {
          root: {
            boxShadow: "0 3px 6px #0000000A",
          },
        },
      },
      MuiButton: {
        defaultProps: {
          disableElevation: true,
          disableRipple: true,
        },
        styleOverrides: {
          root: {
            textTransform: "none",
          },
          sizeMedium: {
            padding: "6.5px 28px",
          },
          sizeLarge: {
            padding: "10.5px 20px",
          },
          outlinedSizeMedium: {
            padding: "5.5px 28px",
          },
          outlinedSizeLarge: {
            padding: "9.5px 20px",
          },
        },
      },
    },
  });

export default createCustomTheme;

declare module "@mui/material/styles" {
  interface TypographyVariants {
    title: React.CSSProperties;
  }

  interface TypographyVariantsOptions {
    title?: React.CSSProperties;
  }
}

declare module "@mui/material/Typography" {
  interface TypographyPropsVariantOverrides {
    title: true;
  }
}
