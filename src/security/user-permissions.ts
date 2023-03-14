import { UserAction, EntityType } from "~/security";

export type UserPermissions = Record<UserAction, EntityType[]>;
