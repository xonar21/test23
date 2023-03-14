import { render } from "~/shared";
import { NavigationDrawer } from "~/components";

describe("NavigationDrawer component", () => {
  it("should render without crashing", () => {
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const { asFragment } = render(<NavigationDrawer links={undefined as any} />);
    expect(asFragment()).toMatchSnapshot();
  });
});
