import React, { useEffect, useState } from "react";
import { useRouter } from "next/router";
import NextLink from "next/link";
import { Breadcrumbs as MuiBreadcrumbs, Link, Theme, Typography } from "@mui/material";
import { makeStyles } from "@mui/styles";
import { useTranslation } from "next-i18next";
import { IApplicationRoute, IApplicationRoutes, routes } from "~/shared";

interface IBreadcrumb extends IApplicationRoute {
  key: string;
}

const useStyles = makeStyles((theme: Theme) => ({
  root: {
    margin: theme.spacing(1.25, 0),
    color: theme.palette.primary.main,
    ...theme.typography.subtitle2,
    "& .MuiBreadcrumbs-separator": {
      margin: theme.spacing(0, 0.625),
    },
  },
}));

const Breadcrumbs: React.FC = () => {
  const classes = useStyles();
  const { t } = useTranslation();
  const { pathname } = useRouter();
  const [breadcrumbs, setBreadcrumbs] = useState<IBreadcrumb[]>([]);

  const lowerCaseFirstLetter = (str: string): string => {
    if (str.length === 0) {
      return str;
    }
    return str.charAt(0).toLowerCase() + str.slice(1);
  };

  useEffect(() => {
    const values: IBreadcrumb[] = Object.keys(routes)
      .map(key => ({
        key,
        ...(routes[key as keyof IApplicationRoutes] as IApplicationRoute),
      }))
      .filter(({ path, avoidBreadcrumbRender }) => pathname.includes(path) && !avoidBreadcrumbRender);

    setBreadcrumbs(values);
  }, [pathname]);

  return breadcrumbs.length > 1 ? (
    <MuiBreadcrumbs className={classes.root} aria-label="breadcrumbs">
      {breadcrumbs.map((breadcrumb: IBreadcrumb, index) =>
        index === breadcrumbs.length - 1 ? (
          <Typography key={index} variant="subtitle2" fontWeight="bold">
            {t(`routes.${lowerCaseFirstLetter(breadcrumb.key)}`)}
            {console.log(breadcrumb.key, t(`routes.${lowerCaseFirstLetter(breadcrumb.key)}`))}
          </Typography>
        ) : (
          <NextLink key={index} href={breadcrumb.path} passHref>
            <Link underline="hover" color="inherit">
              {t(`routes.${breadcrumb.key.toLowerCase()}`)}
            </Link>
          </NextLink>
        ),
      )}
    </MuiBreadcrumbs>
  ) : (
    <></>
  );
};

export default Breadcrumbs;
