import { render } from "~/shared";
import { AuthProvider } from "~/components";

describe("AuthProvider", () => {
  it("should match snapshot", () => {
    const { asFragment } = render(<AuthProvider />);
    expect(asFragment()).toMatchSnapshot();
  });
});
