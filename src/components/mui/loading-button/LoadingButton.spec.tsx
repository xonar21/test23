import { render } from "~/shared";
import { LoadingButton } from "~/components";

describe("LoadingButton component", () => {
  it("should match snapshot", () => {
    const { asFragment } = render(<LoadingButton />);
    expect(asFragment()).toMatchSnapshot();
  });

  it("should match snapshot with truthy loading prop", () => {
    const { asFragment } = render(<LoadingButton loading />);
    expect(asFragment()).toMatchSnapshot();
  });
});
