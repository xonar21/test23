import { Card } from "~/components";
import { render } from "~/shared";

describe("Card component", () => {
  const component = render(<Card title="News" />);

  it("renders without crashing", () => {
    expect(component).toBeTruthy();
  });
});
