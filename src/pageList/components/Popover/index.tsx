import React from "react";
import { Popover } from "@mui/material";
interface PopoverModalProps {
  anchorEl: any;
  handleClose: any;
  content: React.ReactNode;
}

const PopoverModal: React.FC<PopoverModalProps> = ({ anchorEl, handleClose, content }) => {
  return (
    <>
      <Popover
        open={Boolean(anchorEl)}
        anchorEl={anchorEl}
        onClose={handleClose}
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "center",
        }}
        transformOrigin={{
          vertical: "top",
          horizontal: "right",
        }}
        PaperProps={{
          style: {
            background: "transparent url(/images/Union.svg) no-repeat -30px -27px",
            boxShadow: "rgba(0, 0, 0, 0.1) 0px 12px 13px 5px",
          },
        }}
        elevation={1}
      >
        {content}
      </Popover>

      <style>
        {`
          .MuiPopperUnstyled-root {
            z-index: 9999;
          }
        `}
      </style>
    </>
  );
};

export default PopoverModal;
