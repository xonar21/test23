import React from "react";
import Head from "next/head";
import { useTranslation } from "next-i18next";

interface IHeadSeoProps {
  title?: string;
  description?: string;
}

const HeadSeo: React.FC<IHeadSeoProps> = ({ title, description, children }) => {
  const { t } = useTranslation(["common"]);

  return (
    <Head>
      <title>{title || t("brandName")}</title>
      <meta name="description" content={description || t("description")} />
      {children}
    </Head>
  );
};

export default HeadSeo;
