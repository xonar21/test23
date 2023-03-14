import { Circle } from "@mui/icons-material";
import { GridActionsCellItem } from "~/components";
import { render } from "~/shared";

describe("GridActionsCellItem component", () => {
  it("should match snapshot", () => {
    const { asFragment } = render(<GridActionsCellItem label="Test" icon={<Circle />} />);
    expect(asFragment()).toMatchSnapshot();
  });
});
