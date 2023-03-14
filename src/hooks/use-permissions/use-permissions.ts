import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import {
  EntityType,
  UserAction,
  UserPermissions,
  hasPermissions as checkPermissions,
  getPermissionsFromToken,
} from "~/security";
import { AuthSelectors } from "~/store";

const usePermissions = () => {
  const { token } = useSelector(AuthSelectors.getRoot);
  const [permissions, setPermissions] = useState<UserPermissions>({
    create: [],
    read: [],
    update: [],
    delete: [],
  });

  useEffect(() => {
    setPermissions(getPermissionsFromToken(token));
  }, [token]);

  const hasPermission = (action: UserAction, entity: EntityType) => permissions[action].includes(entity);

  const hasPermissions = (_permissions: Partial<UserPermissions>) => checkPermissions(_permissions, permissions);

  return { hasPermission, hasPermissions };
};

export default usePermissions;
