import { useEffect } from "react";
import { useSelector } from "react-redux";
import { Permission } from "~/security";
import { UserSelectors } from "~/store";
import { usePermissions } from "~/hooks";
import { useRouter } from "next/router";

export const usePermissionRedirect = (permissions: Permission[], path?: string) => {
  const { replace } = useRouter();
  const claims = useSelector(UserSelectors.getClaims);
  const { hasPermissions, loading } = usePermissions();

  useEffect(() => {
    if (!loading && !hasPermissions(permissions)) {
      replace(path || "/403");
    }
  }, [claims, permissions]);
};
