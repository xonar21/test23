import { render } from "~/shared";
import { AuthLayout } from "~/layouts";

const mockUseMediaQuery = jest.fn(() => false);

jest.mock("@mui/material", () => ({
  ...jest.requireActual("@mui/material"),
  __esModule: true,
  useMediaQuery: () => mockUseMediaQuery(),
}));

describe("Auth Layout", () => {
  const layout = () => (
    <AuthLayout>
      <div>Content</div>
    </AuthLayout>
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
