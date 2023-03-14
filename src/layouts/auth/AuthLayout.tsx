import React from "react";
import { Box, Card } from "@mui/material";
import { HeadSeo, ThemeModeToggle } from "~/components";

const AuthLayout: React.FC = ({ children }) => {
  return (
    <React.Fragment>
      <HeadSeo />
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          minHeight: "100vh",
          backgroundColor: ({ palette }) => (palette.mode === "light" ? palette.grey[100] : palette.grey[900]),
        }}
      >
        <Card
          elevation={24}
          sx={{
            padding: ({ spacing }) => spacing(3),
            minWidth: 500,
            position: "relative",
          }}
        >
          <Box
            sx={{
              position: "absolute",
              top: ({ spacing }) => spacing(2),
              right: ({ spacing }) => spacing(2),
            }}
          >
            <ThemeModeToggle />
          </Box>
          {children}
        </Card>
      </Box>
    </React.Fragment>
  );
};

export default AuthLayout;
