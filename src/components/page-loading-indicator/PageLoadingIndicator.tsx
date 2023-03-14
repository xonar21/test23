import React, { useCallback, useEffect, useState } from "react";
import { Router } from "next/router";
import { Box, LinearProgress } from "@mui/material";

const PageLoadingIndicator: React.FC = ({ children }) => {
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    Router.events.on("routeChangeStart", handleStart);
    Router.events.on("routeChangeComplete", handleEnd);
    Router.events.on("routeChangeError", handleEnd);

    return () => {
      Router.events.off("routeChangeStart", handleStart);
      Router.events.off("routeChangeComplete", handleEnd);
      Router.events.off("routeChangeError", handleEnd);
    };
  }, []);

  const handleStart = useCallback(() => {
    setLoading(true);
  }, []);

  const handleEnd = useCallback(() => {
    setLoading(false);
  }, []);

  return (
    <React.Fragment>
      <Box
        sx={{
          position: "absolute",
          width: "100%",
          transition: ".2s linear",
          zIndex: theme => theme.zIndex.appBar + 1,
          opacity: loading ? 1 : 0,
        }}
      >
        <LinearProgress variant="indeterminate" />
      </Box>
      {children}
    </React.Fragment>
  );
};

export default PageLoadingIndicator;
