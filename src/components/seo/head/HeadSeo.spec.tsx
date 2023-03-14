import { HeadSeo } from "~/components";
import { render } from "~/shared";

describe("HeadSeo component", () => {
  it("should render without crashing", () => {
    const { asFragment } = render(<HeadSeo />);
    expect(asFragment()).toMatchSnapshot();
  });
});
