import React from "react";
import { Tooltip, TooltipProps } from "@mui/material";

type ConditionalTooltipProps = TooltipProps & {
  /** Condition that triggers the tooltip rendering. */
  condition: boolean;
};

/**
 * MUI Tooltip wrapper, that will only render the tooltip if a certain condition is met.
 */
const ConditionalTooltip: React.FC<ConditionalTooltipProps> = ({ condition, children, ...props }) => {
  return condition ? <Tooltip {...props}>{children}</Tooltip> : children;
};

export default ConditionalTooltip;
