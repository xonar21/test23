import { render } from "~/shared";
import { SnackbarProvider } from "~/components";

describe("Snackbar Provider", () => {
  it("should render without crashing", () => {
    const { asFragment } = render(<SnackbarProvider />);
    expect(asFragment()).toMatchSnapshot();
  });
});
