import { IApplicationRoute, getRoutePath, getRouteByPath } from "~/shared";
import { routes } from "~/shared/routes";

describe("Routes", () => {
  it("getRoutePath returns a valid url on passing route params", () => {
    const route: IApplicationRoute = {
      path: "/test/[id]",
    };
    const routeParams = { id: 1, someOtherParam: "test" };
    const expected = "/test/1";
    expect(getRoutePath(route, routeParams)).toBe(expected);
  });

  it("getRoutePath returns a valid url on passing query params", () => {
    const route: IApplicationRoute = {
      path: "/test",
    };
    const queryParams = {
      q: "hello",
      target: "world",
    };
    const expected = "/test?q=hello&target=world";
    expect(getRoutePath(route, undefined, queryParams)).toBe(expected);
  });

  it("getRouteByPath returns a valid route", () => {
    const route = routes.Home;
    expect(getRouteByPath(route.path)).toBe(route);
  });
});
