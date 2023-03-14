import React from "react";
import { GridActionsCellItemProps, GridActionsCellItem as MuiGridActionsCellItem } from "@mui/x-data-grid";

const GridActionsCellItem: React.FC<GridActionsCellItemProps> = props => {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  return <MuiGridActionsCellItem {...(props as any)} />;
};

export default GridActionsCellItem;
