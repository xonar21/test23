/**
 * Note: NextJS middleware doesn't support imports based on index files.
 * Use explicit module paths instead. Example:
 * import Example from "~/components/example/Example" <--- This works
 * import { Example } from "~/components"             <--- This doesn't
 * @see @link https://nextjs.org/docs/middleware
 */
import { NextRequest, NextResponse } from "next/server";
import { CookieKeys } from "~/shared/constants";
import { routes } from "~/shared/routes";
import { getRouteByPath, getRoutePath } from "~/shared/utils/routing-utils";
import { hasPermissions, getPermissionsFromToken } from "~/security/security-utils";

export function middleware(request: NextRequest) {
  // #region Authorized route redirect behavior

  const path = request.nextUrl.pathname as string;
  if (path) {
    const route = getRouteByPath(path);
    if (route) {
      const token = request.cookies.get(CookieKeys.AuthToken);
      // request.cookies.set(CookieKeys.SaiseToken, "bd9010d3-473e-4e10-8e04-46c1148754be");
      const saiseToken = request.cookies.get(CookieKeys.SaiseToken);

      const url = request.nextUrl.clone();

      if (route.authorized && saiseToken && !token) {
        url.search = `${CookieKeys.SaiseToken}=${saiseToken}`;
        url.pathname = getRoutePath(routes.loginSaise);

        return NextResponse.redirect(url);
      }

      if (route.authorized && !token) {
        url.pathname = getRoutePath(routes.Public);
        return NextResponse.redirect(url);
      }

      if (route.annonymous && !!token) {
        url.pathname = getRoutePath(routes.Home);
        return NextResponse.redirect(url);
      }

      if (
        route.permissions &&
        (!token || (!!token && !hasPermissions(route.permissions, getPermissionsFromToken(token))))
      ) {
        url.pathname = getRoutePath(routes.Home);
        return NextResponse.redirect(url);
      }
    }
  }

  // #endregion

  return NextResponse.next();
}
