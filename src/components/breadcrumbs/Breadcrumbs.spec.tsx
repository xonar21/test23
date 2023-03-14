import { render, routes } from "~/shared";
import { Breadcrumbs } from "~/components";

describe("Breadcrumbs component", () => {
  const useRouter = jest.spyOn(require("next/router"), "useRouter");
  const mockUseRouter = {
    pathname: routes.Posts.path,
  };

  it("should render without crashing", () => {
    const { asFragment } = render(<Breadcrumbs />);
    expect(asFragment()).toMatchSnapshot();
  });

  it("should render with one node", () => {
    useRouter.mockImplementationOnce(() => mockUseRouter);
    const { asFragment } = render(<Breadcrumbs />);
    expect(asFragment()).toMatchSnapshot();
  });
});
