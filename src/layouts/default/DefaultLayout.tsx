import React from "react";
import { Box, Container } from "@mui/material";
import { AppHeader, Breadcrumbs, HeadSeo } from "~/components";

const DefaultLayout: React.FC = ({ children }) => {
  return (
    <React.Fragment>
      <HeadSeo />
      <Box sx={{ display: "flex" }}>
        <AppHeader />
        <Container
          maxWidth={false}
          sx={{
            pt: 8,
            minHeight: "100vh",
            backgroundColor: ({ palette }) => (palette.mode === "light" ? palette.grey[100] : palette.grey[900]),
          }}
        >
          <Breadcrumbs />
          {children}
        </Container>
      </Box>
    </React.Fragment>
  );
};

export default DefaultLayout;
