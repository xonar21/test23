import { render } from "~/shared";
import { ConditionalTooltip } from "~/components";

describe("ConditionalTooltip component", () => {
  const tooltip = (condition: boolean) => (
    <ConditionalTooltip condition={condition} title="Content">
      <div>Content</div>
    </ConditionalTooltip>
  );

  it("renders without crashing", () => {
    const { asFragment } = render(tooltip(true));
    expect(asFragment()).toMatchSnapshot();
  });
});
