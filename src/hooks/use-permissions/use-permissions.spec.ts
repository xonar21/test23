import { usePermissions } from "~/hooks";
import { EntityType, UserAction } from "~/security";

jest.mock("react", () => ({
  ...jest.requireActual("react"),
  useState: jest.fn(() => [
    {
      create: ["post"],
      read: ["post", "tag", "user"],
      update: ["post"],
      delete: ["post"],
    },
    jest.fn(),
  ]),
  useEffect: jest.fn(cb => {
    cb();
  }),
}));

jest.mock("react-redux", () => ({
  ...jest.requireActual("react-redux"),
  useSelector: jest.fn(() => ({
    token: "QpwL5tke4Pnpja7X4",
  })),
}));

describe("usePermissions hook", () => {
  const { hasPermission, hasPermissions } = usePermissions();

  it("hasPermission returns true on checking valid permission", () => {
    expect(hasPermission(UserAction.Create, EntityType.Post)).toBeTruthy();
  });

  it("hasPermissions returns true on checking valid permissions", () => {
    expect(hasPermissions({ read: [EntityType.Post] })).toBeTruthy();
  });
});
