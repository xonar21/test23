import React, { useEffect } from "react";
import { useDispatch } from "react-redux";
import { useCookie } from "react-use";
import { CookieKeys } from "~/shared";
import { AuthActions } from "~/store";

const AuthProvider: React.FC = ({ children }) => {
  const dispatch = useDispatch();
  const [token] = useCookie(CookieKeys.AuthToken);

  useEffect(() => {
    initialize();
  }, [token]);

  const initialize = () => {
    dispatch(AuthActions.AUTH_STATE_CHANGED({ token }));
  };

  return <>{children}</>;
};

export default AuthProvider;
