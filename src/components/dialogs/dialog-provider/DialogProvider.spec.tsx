import { render } from "~/shared";
import { DialogProvider } from "~/components";

jest.mock("~/components/dialog-types/dialog-types", () => ({
  ...jest.requireActual("~/components/dialog-types/dialog-types"),
  __esModule: true,
  default: { TestDialog: () => <div>Test</div> },
}));

jest.mock("~/hooks/use-dialog/use-dialog", () => ({
  ...jest.requireActual("~/hooks/use-dialog/use-dialog"),
  __esModule: true,
  default: jest.fn(() => ({
    openDialog: jest.fn(),
    closeDialog: jest.fn(),
    open: true,
    dialogType: "TestDialog",
    title: "Test",
    dialogProps: {},
  })),
}));

describe("Generic Dialog component", () => {
  it("should match snapshot", () => {
    const { asFragment } = render(<DialogProvider />);
    expect(asFragment()).toMatchSnapshot();
  });
});
