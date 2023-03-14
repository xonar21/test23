import React, { useCallback, useState } from "react";
import { AppBar, Button, IconButton, Menu, MenuItem, Toolbar, Typography } from "@mui/material";
import { Menu as MenuIcon, Close as CloseIcon, Home as HomeIcon, Article as ArticleIcon } from "@mui/icons-material";
import { AuthButton, NavigationDrawer, ThemeModeToggle } from "~/components";
import { Locale, routes, testIds } from "~/shared";
import { INavigationDrawerLink, Nullable } from "~/core";
import { useRouter } from "next/router";
import { useTranslation } from "next-i18next";

const Header: React.FC = () => {
  const { t } = useTranslation(["common"]);
  const router = useRouter();
  const [open, setOpen] = useState(false);
  const [localesMenuAnchor, setLocalesMenuAnchor] = useState<Nullable<HTMLButtonElement>>(null);
  const localesMenuOpen = Boolean(localesMenuAnchor);

  const links: INavigationDrawerLink[] = [
    {
      icon: HomeIcon,
      title: t("routes.home"),
      route: routes.Home,
    },
    {
      icon: ArticleIcon,
      title: t("routes.posts"),
      route: routes.Posts,
    },
  ];

  const toggleDrawer = useCallback(() => {
    setOpen(previous => !previous);
  }, []);

  const handleLocaleChange = (locale: Locale) => {
    router.push(router.pathname, router.pathname, { locale });
    setLocalesMenuAnchor(null);
  };

  return (
    <>
      <AppBar position="fixed" data-testid={testIds.components.header.root}>
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
            onClick={toggleDrawer}
            data-testid={testIds.components.header.buttons.drawerToggler}
          >
            {open ? <CloseIcon /> : <MenuIcon />}
          </IconButton>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            {t("brandName")}
          </Typography>
          <ThemeModeToggle />
          <Button
            aria-haspopup="true"
            onClick={event => setLocalesMenuAnchor(event.currentTarget)}
            color="inherit"
            sx={{ mx: 2 }}
            data-testid={testIds.components.header.buttons.localeMenu}
          >
            {t("language")}
          </Button>
          <Menu
            anchorEl={localesMenuAnchor}
            open={localesMenuOpen}
            onClose={/* istanbul ignore next */ () => setLocalesMenuAnchor(null)}
          >
            {Object.values(Locale).map(locale => (
              <MenuItem
                key={locale}
                onClick={() => handleLocaleChange(locale)}
                data-testid={testIds.components.header.localeItem}
              >
                {t(`locales.${locale}`)}
              </MenuItem>
            ))}
          </Menu>
          <AuthButton />
        </Toolbar>
      </AppBar>
      <NavigationDrawer
        variant="permanent"
        links={links}
        open={open}
        aria-expanded={open}
        data-testid={testIds.components.navigationDrawer.root}
      />
    </>
  );
};

export default Header;
