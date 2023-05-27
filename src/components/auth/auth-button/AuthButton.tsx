import React from "react";
import { Box, IconButton, Tooltip, Typography } from "@mui/material";
import { LoginOutlined, LogoutOutlined } from "@mui/icons-material";
import { useTranslation } from "next-i18next";
import { useDispatch, useSelector } from "react-redux";
import { CookieKeys, getRoutePath, routes, testIds } from "~/shared";
import { AuthActions, AuthSelectors, UserSelectors } from "~/store";
import { useRouter } from "next/router";
import { useCookie } from "react-use";

const AuthButton = () => {
  const dispatch = useDispatch();
  const { t } = useTranslation(["common"]);
  const { push } = useRouter();
  const [, , deleteToken] = useCookie(CookieKeys.AuthToken);
  const [, , deleteTokenRefresh] = useCookie(CookieKeys.AuthRefreshToken);
  const [, , deleteSaiseToken] = useCookie(CookieKeys.SaiseToken);
  const [, , deleteEmail] = useCookie(CookieKeys.ExternalUserEmail);
  const [, , deleteIdnp] = useCookie(CookieKeys.ExternalUserIdnp);
  const { isAuthenticated, SaiseToken, token, refreshToken } = useSelector(AuthSelectors.getRoot);
  const { data } = useSelector(UserSelectors.getRoot);
  const label = t(isAuthenticated ? "auth.logout" : "auth.login");

  function deleteCookie(name: any, domain: string) {
    document.cookie = `${name}=; domain=${domain}; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC`;
  }
  const handleOnClick = async () => {
    if (isAuthenticated) {
      await dispatch(AuthActions.invalidate({ token: token as string, refreshToken: refreshToken as string }));
      await dispatch(AuthActions.AUTH_STATE_CHANGED({ token: null, refreshToken: null }));
      deleteToken();
      deleteEmail();
      deleteIdnp();
      deleteSaiseToken();
      deleteTokenRefresh();
      const interDate = 0;
      dispatch(AuthActions.TIME_REFRESH_SET({ interDate }));
      deleteCookie(CookieKeys.SaiseToken, ".dev.indrivo.com");
      if (SaiseToken) {
        push("https://siaadmin.dev.indrivo.com/");
      } else {
        push(getRoutePath(routes.Public));
      }
    } else {
      push(getRoutePath(routes.Login));
    }
  };

  return (
    <>
      {isAuthenticated && (
        <Box mr={1.5}>
          <Typography variant="h4">{data?.userName}</Typography>
          <Typography variant="subtitle1" sx={{ opacity: 0.3 }}>
            {t("authorizedUser")}
          </Typography>
        </Box>
      )}
      <Tooltip title={label} placement="bottom">
        <IconButton onClick={handleOnClick} color="inherit" data-testid={testIds.components.header.buttons.authButton}>
          {isAuthenticated ? <LogoutOutlined /> : <LoginOutlined />}
        </IconButton>
      </Tooltip>
    </>
  );
};

export default AuthButton;
