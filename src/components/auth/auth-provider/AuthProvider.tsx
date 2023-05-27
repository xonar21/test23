import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useCookie } from "react-use";
import { CookieKeys } from "~/shared";
import { AuthActions, AuthSelectors, UserActions } from "~/store";
import jwt, { JwtPayload } from "jsonwebtoken";
import { IRefreshModel, IResponse, ISaiseModel } from "~/models";
import { useRouter } from "next/router";

const AuthProvider: React.FC = ({ children }) => {
  const router = useRouter();
  const dispatch = useDispatch();
  const [token] = useCookie(CookieKeys.AuthToken);
  const [, updateToken] = useCookie(CookieKeys.AuthToken);
  const [SaiseToken] = useCookie(CookieKeys.SaiseToken);
  const [refreshToken] = useCookie(CookieKeys.AuthRefreshToken);
  const [, updateRefreshToken] = useCookie(CookieKeys.AuthRefreshToken);
  const decoded = jwt.decode(token as string);
  const [time, setTime] = useState(false);
  const [timeoutId, setTimeoutId]: any = useState(0);
  const Auth = useSelector(AuthSelectors.getRoot);

  useEffect(() => {
    if (decoded && typeof decoded !== "string" && "id" in decoded) {
      const decodedJwt = decoded as JwtPayload;
      if (decodedJwt.exp) {
        const exp = decodedJwt.exp;
        const expirationDate: any = new Date(exp * 1000);
        const today: any = new Date();
        const interDate = expirationDate - today;
        dispatch(AuthActions.TIME_REFRESH_SET({ interDate }));
        dispatch(UserActions.getUserById(decodedJwt.id as string));
        setTime(true);
      }
    }

    initialize();
  }, [token]);

  function isIResponse(obj: any, field: string): obj is IResponse<IRefreshModel> {
    return field in obj;
  }

  useEffect(() => {
    if (!Auth.isAuthenticated) {
      clearTimeout(timeoutId);
    }
  }, [Auth.isAuthenticated]);

  useEffect(() => {
    if (time && Auth.timeToRefresh > 0) {
      const timeout = setTimeout(async () => {
        if (Auth.token && Auth.refreshToken) {
          console.log(router);
          const response = await dispatch(AuthActions.refresh({ token: Auth.token, refreshToken: Auth.refreshToken }));
          console.log(response);
          if (response.succeeded) {
            if (isIResponse(response.payload, "token") && isIResponse(response.payload, "refreshToken")) {
              updateToken(response.payload.token as string);
              updateRefreshToken(response.payload.refreshToken as string);
            }
            if (isIResponse(response.payload, "token")) {
              const decod = jwt.decode(response.payload.token as string);
              const decodedJwt = decod as JwtPayload;
              if (decodedJwt.exp) {
                const exp = decodedJwt.exp;
                const expirationDate: any = new Date(exp * 1000);
                const today: any = new Date();
                const interDate = expirationDate - today;
                dispatch(AuthActions.TIME_REFRESH_SET({ interDate }));
              }
            }
          }
          // } else {
          //   if (SaiseToken) {
          //     const saiseTokenModel: ISaiseModel = { token: SaiseToken };
          //     const response = await dispatch(AuthActions.loginSaise(saiseTokenModel));
          //     if (response.succeeded) {
          //       if (isIResponse(response.payload, "token") && isIResponse(response.payload, "refreshToken")) {
          //         updateToken(response.payload.token as string);
          //         updateRefreshToken(response.payload.refreshToken as string);
          //       }
          //       if (isIResponse(response.payload, "token")) {
          //         const decod = jwt.decode(response.payload.token as string);
          //         const decodedJwt = decod as JwtPayload;
          //         if (decodedJwt.exp) {
          //           const exp = decodedJwt.exp;
          //           const expirationDate: any = new Date(exp * 1000);
          //           const today: any = new Date();
          //           const interDate = expirationDate - today;
          //           dispatch(AuthActions.TIME_REFRESH_SET({ interDate }));
          //         }
          //       }
          //     }
          //   }
          // }
        }
      }, Auth.timeToRefresh + 1000);

      setTimeoutId(timeout);
    }
  }, [time, Auth.timeToRefresh]);

  const initialize = () => {
    dispatch(AuthActions.AUTH_STATE_CHANGED({ token, SaiseToken, refreshToken }));
  };

  return <>{children}</>;
};

export default AuthProvider;
