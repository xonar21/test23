import React, { useEffect } from "react";
import { useDispatch } from "react-redux";
import { useRouter } from "next/router";
import { useCookie } from "react-use";
import { Box, CircularProgress } from "@mui/material";
import { ISaiseModel } from "~/models";
import { CookieKeys, getRoutePath, routes } from "~/shared";
import { AuthActions } from "~/store";

const RedirectLayout: React.FC = () => {
  const router = useRouter();
  const dispatch = useDispatch();
  const saiseToken = router.query[CookieKeys.SaiseToken];
  const [, updateAuthRefreshCookie] = useCookie(CookieKeys.AuthRefreshToken);
  const [, updateAuthCookie] = useCookie(CookieKeys.AuthToken);
  const [, updateSaiseCookie] = useCookie(CookieKeys.SaiseToken);

  function deleteCookie(name: any, domain: string) {
    document.cookie = `${name}=; domain=${domain}; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC`;
  }

  useEffect(() => {
    if (typeof saiseToken === "string") {
      const saiseTokenModel: ISaiseModel = { token: saiseToken };

      dispatch(AuthActions.loginSaise(saiseTokenModel)).then((res: any) => {
        if (res.succeeded === false) {
          deleteCookie(CookieKeys.SaiseToken, ".dev.indrivo.com");

          return;
        }
        console.log(res);
        updateAuthRefreshCookie(res.payload.refreshToken);
        updateAuthCookie(res.payload.token);
        updateSaiseCookie(saiseTokenModel.token);
        window.location.href = getRoutePath(routes.Elections);
      });
    }
  }, [saiseToken, dispatch]);

  return (
    <>
      <Box
        sx={{
          width: "100%",
          height: "100vh",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <CircularProgress sx={{ width: "70px !important", height: "70px !important" }} />
      </Box>
    </>
  );
};

export default RedirectLayout;
