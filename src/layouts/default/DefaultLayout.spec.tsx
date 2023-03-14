import { render } from "~/shared";
import { DefaultLayout } from "~/layouts";

const mockUseMediaQuery = jest.fn(() => false);

jest.mock("@mui/material", () => ({
  ...jest.requireActual("@mui/material"),
  __esModule: true,
  useMediaQuery: () => mockUseMediaQuery(),
}));

describe("Default Layout", () => {
  const layout = () => (
    <DefaultLayout>
      <div>Content</div>
    </DefaultLayout>
  );
  it("renders without crashing", () => {
    const { asFragment } = render(layout());
    expect(asFragment()).toMatchSnapshot();
  });

  it("renders in dark mode", () => {
    mockUseMediaQuery.mockImplementation(() => true);
    const { asFragment } = render(layout());
    expect(asFragment()).toMatchSnapshot();
  });
});
