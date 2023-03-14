import React from "react";
import { Button } from "@mui/material";
import { useTranslation } from "next-i18next";
import { useDispatch, useSelector } from "react-redux";
import { CookieKeys, getRoutePath, routes, testIds } from "~/shared";
import { AuthActions, AuthSelectors } from "~/store";
import { useRouter } from "next/router";
import { useCookie } from "react-use";

const AuthButton = () => {
  const dispatch = useDispatch();
  const { t } = useTranslation(["common"]);
  const { push } = useRouter();
  const [, , deleteToken] = useCookie(CookieKeys.AuthToken);
  const { isAuthenticated } = useSelector(AuthSelectors.getRoot);
  const label = t(isAuthenticated ? "auth.logout" : "auth.login");

  const handleOnClick = () => {
    if (isAuthenticated) {
      dispatch(AuthActions.AUTH_STATE_CHANGED({ token: null }));
      deleteToken();
      push(getRoutePath(routes.Home));
    } else {
      push(getRoutePath(routes.Login));
    }
  };

  return (
    <Button onClick={handleOnClick} color="inherit" data-testid={testIds.components.header.buttons.authButton}>
      {label}
    </Button>
  );
};

export default AuthButton;
