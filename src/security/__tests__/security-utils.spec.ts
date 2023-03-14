import { EntityType, getPermissionsFromToken, UserPermissions, hasPermissions } from "~/security";

const userPermissions: UserPermissions = {
  create: [EntityType.Post],
  read: [EntityType.Post, EntityType.Tag, EntityType.User],
  update: [EntityType.Post],
  delete: [EntityType.Post],
};

describe("Security utils", () => {
  it("getPermissionsFromToken returns empty permissions on passing no token", () => {
    const expected = {
      create: [],
      read: [],
      update: [],
      delete: [],
    };
    const actual = getPermissionsFromToken("");
    expect(actual).toEqual(expected);
  });

  it("getPermissionsFromToken returns permissions object on passing token", () => {
    const expected = userPermissions;
    const actual = getPermissionsFromToken("QpwL5tke4Pnpja7X4");
    expect(actual).toEqual(expected);
  });

  it("hasPermissions returns true on passing valid permissions", () => {
    expect(hasPermissions({ read: [EntityType.Post] }, userPermissions)).toBeTruthy();
  });
});
