import { Main } from "~/components";
import { render } from "~/shared";

describe("Main component", () => {
  it("renders without crashing", () => {
    const component = render(<Main />);
    expect(component).toBeTruthy();
  });
});
