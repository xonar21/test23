import React from "react";
import { GetServerSideProps } from "next";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import i18nConfig from "@i18n";
import { NextPageWithLayout } from "~/core";
import { PublicLayout } from "~/layouts";
import { Typography } from "@mui/material";
import { useTranslation } from "next-i18next";

const Public: NextPageWithLayout = () => {
  const { t } = useTranslation(["common"]);
  return (
    <>
      {["welcomeAbout.p1", "welcomeAbout.p2", "welcomeAbout.p3", "welcomeAbout.p4"].map(path => (
        <Typography
          variant="body1"
          sx={({ palette }) => ({
            color: palette.primary.main,
            marginBottom: "20px",
          })}
          key={path}
        >
          {t(path)}
        </Typography>
      ))}
    </>
  );
};

Public.layout = PublicLayout;

export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};

export default Public;
