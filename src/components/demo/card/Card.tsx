import React from "react";
import { Button, Card as MuiCard, CardActions, CardContent, Divider, Typography } from "@mui/material";
import { useTranslation } from "next-i18next";

interface IProps {
  title: string;
  url: string;
}

const Card: React.FC<IProps> = ({ title, url, children }) => {
  const { t } = useTranslation();

  return (
    <MuiCard sx={{ height: "100%", display: "flex", flexDirection: "column", justifyContent: "space-between" }}>
      <CardContent>
        <Typography variant="h6" color="text.secondary" variantMapping={{ h6: "h4" }}>
          {title}
        </Typography>
        <Divider sx={{ my: 1 }} />
        <Typography color="text.secondary">{children}</Typography>
      </CardContent>
      <CardActions>
        <Button size="small" href={url} target="_blank">
          {t("readDocs")}
        </Button>
      </CardActions>
    </MuiCard>
  );
};

export default Card;
