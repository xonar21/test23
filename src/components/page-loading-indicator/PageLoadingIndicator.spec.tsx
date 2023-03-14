import { act, render } from "~/shared";
import { PageLoadingIndicator } from "~/components";
import { Router } from "next/router";

describe("PageLoadingIndicator component", () => {
  it("should match snapshot", () => {
    const { asFragment } = render(<PageLoadingIndicator />);
    expect(asFragment()).toMatchSnapshot();
  });

  it("should handle start without errors", () => {
    render(<PageLoadingIndicator />);
    act(() => {
      Router.events.emit("routeChangeStart");
    });
  });

  it("should handle end without errors", () => {
    render(<PageLoadingIndicator />);
    act(() => {
      Router.events.emit("routeChangeComplete");
    });
  });
});
