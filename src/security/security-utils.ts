import { Nullable } from "~/core";
import { UserPermissions, EntityType, UserAction } from "~/security";

export const getPermissionsFromToken = (token: Nullable<string>): UserPermissions => {
  if (!token)
    return {
      create: [],
      read: [],
      update: [],
      delete: [],
    };

  // Replace this with real implementation of token decoding
  return {
    create: [EntityType.Post],
    read: [EntityType.Post, EntityType.Tag, EntityType.User],
    update: [EntityType.Post],
    delete: [EntityType.Post],
  };
};

export const hasPermissions = (permissions: Partial<UserPermissions>, currentUserPermissions: UserPermissions) => {
  return Object.keys(permissions).every(key => {
    const action = key as UserAction;
    return permissions[action]?.every(entity => currentUserPermissions[action].includes(entity));
  });
};
