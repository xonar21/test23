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
    margin: theme.spacing(1, 0),
    ...theme.typography.subtitle2,
  },
}));

const Breadcrumbs: React.FC = () => {
  const classes = useStyles();
  const { t } = useTranslation();
  const { pathname } = useRouter();
  const [breadcrumbs, setBreadcrumbs] = useState<IBreadcrumb[]>([]);

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
          <Typography key={index} variant="body2" color="text.primary">
            {t(`routes.${breadcrumb.key.toLowerCase()}`)}
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
