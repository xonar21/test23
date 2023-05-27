import { render } from "~/shared";
import { NavigationDrawer } from "~/components";

describe("NavigationDrawer component", () => {
  it("should render without crashing", () => {
    const { asFragment } = render(<NavigationDrawer links={[]} setOpen={jest.fn()} />);
    expect(asFragment()).toMatchSnapshot();
  });
});
