import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { Permission } from "~/security";
import { UserSelectors } from "~/store";

const usePermissions = () => {
  const claims = useSelector(UserSelectors.getClaims);
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    setPermissions(
      claims.map(c => {
        return c.type;
      }),
    );
  }, [claims]);

  useEffect(() => {
    if (claims.length) {
      setLoading(false);
    }
  }, [permissions]);

  const hasPermission = (permission: Permission) => {
    if (permissions.length) {
      return permissions.includes(permission);
    }
  };
  const hasPermissions = (_permissions: Permission[]) => {
    return _permissions.every(p => {
      return permissions.includes(p);
    });
  };
  return { hasPermission, hasPermissions, loading };
};

export default usePermissions;
