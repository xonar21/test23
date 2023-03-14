import React from "react";
import { Button, ButtonProps, CircularProgress, Box } from "@mui/material";

interface ILoadingButtonProps extends ButtonProps {
  loading?: boolean;
}

const ButtonToCircularProgressSizeMap = {
  small: 22.75,
  medium: 24,
  large: 26.25,
};

const LoadingButton: React.FC<ILoadingButtonProps> = ({ loading, disabled, children, ...props }) => {
  return (
    <Button disabled={disabled || loading} {...props}>
      {loading && (
        <CircularProgress
          color="inherit"
          size={ButtonToCircularProgressSizeMap[props.size || "medium"]}
          sx={{ position: "absolute" }}
        />
      )}
      <Box sx={{ visibility: loading ? "hidden" : "initial" }}>{children}</Box>
    </Button>
  );
};

export default LoadingButton;
