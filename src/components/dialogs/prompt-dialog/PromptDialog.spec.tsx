import { fireEvent, render, testIds } from "~/shared";
import { PromptDialog } from "~/components";

const mockOnConfirm = jest.fn();
const mockOnReject = jest.fn();
jest.mock("~/hooks/use-dialog/use-dialog", () => ({
  ...jest.requireActual("~/hooks/use-dialog/use-dialog"),
  __esModule: true,
  default: jest.fn(() => ({
    promptConfig: {
      content: () => <></>,
      onConfirm: mockOnConfirm,
      onReject: mockOnReject,
    },
    closeDialog: jest.fn(),
  })),
}));

describe("PromptDialog component", () => {
  afterEach(() => jest.clearAllMocks());

  it("should match snapshot", () => {
    const { asFragment } = render(<PromptDialog />);
    expect(asFragment()).toMatchSnapshot();
  });

  it("should handle confirm", () => {
    const { getByTestId } = render(<PromptDialog />);
    const confirmButton = getByTestId(testIds.components.dialogs.promptDialog.confirmButton);
    fireEvent.click(confirmButton);
    expect(mockOnConfirm).toBeCalled();
  });

  it("should handle failed confirm", () => {
    mockOnConfirm.mockReturnValueOnce(false);
    const { getByTestId } = render(<PromptDialog />);
    const confirmButton = getByTestId(testIds.components.dialogs.promptDialog.confirmButton);
    fireEvent.click(confirmButton);
    expect(mockOnConfirm).toBeCalled();
  });

  it("should handle reject", () => {
    const { getByTestId } = render(<PromptDialog />);
    const rejectButton = getByTestId(testIds.components.dialogs.promptDialog.rejectButton);
    fireEvent.click(rejectButton);
    expect(mockOnReject).toBeCalled();
  });
});
