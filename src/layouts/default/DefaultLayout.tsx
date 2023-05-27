import React from "react";
import { Box, CircularProgress, Container } from "@mui/material";
import { Header, Breadcrumbs, HeadSeo } from "~/components";
import { AuthSelectors } from "~/store";
import { useSelector } from "react-redux";

const DefaultLayout: React.FC = ({ children }) => {
  const Auth = useSelector(AuthSelectors.getRoot);

  return (
    <>
      <React.Fragment>
        <HeadSeo />
        <Box sx={{ display: "flex" }}>
          <Header />
          <Container
            maxWidth={false}
            sx={({ spacing, palette }) => ({
              p: `${spacing(10, 2.5, 0)} !important`,
              minHeight: "100vh",
              backgroundColor: palette.mode === "light" ? palette.grey[100] : palette.grey[900],
            })}
          >
            <Breadcrumbs />
            <Box
              bgcolor="common.white"
              sx={({ spacing }) => ({ m: spacing(0, -2.5), p: 2.5, minHeight: `calc(100vh - ${spacing(13.875)})` })}
            >
              {!Auth.token ? (
                <Box
                  sx={{
                    backgroundColor: "white",
                    width: "100vw",
                    height: "100vh",
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    position: "absolute",
                    top: "0",
                    left: "0",
                    zIndex: "1200",
                  }}
                >
                  <CircularProgress sx={{ width: "70px !important", height: "70px !important" }} />
                </Box>
              ) : null}
              {children}
            </Box>
          </Container>
        </Box>
      </React.Fragment>
    </>
  );
};

export default DefaultLayout;
