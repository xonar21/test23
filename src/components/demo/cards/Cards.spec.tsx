import { render } from "~/shared";
import { Cards } from "~/components";
import { plugins } from "./Cards";

describe("Cards components", () => {
  it("renders without crashing", () => {
    const component = render(<Cards />);
    expect(component).toBeTruthy();
  });

  it("must have as many items as the length of the meta data", () => {
    const { getAllByTestId } = render(<Cards />);

    const cardContainer = getAllByTestId("container");
    expect(cardContainer).toHaveLength(plugins.length);
  });
});
