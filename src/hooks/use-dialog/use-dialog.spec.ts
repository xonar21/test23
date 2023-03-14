import { useDialog } from "~/hooks";

const mockSetPromptConfig = jest.fn();
jest.mock("react", () => ({
  ...jest.requireActual("react"),
  useContext: jest.fn(() => ({
    promptConfig: {},
    setPromptConfig: mockSetPromptConfig,
  })),
}));

const mockDispatch = jest.fn();
jest.mock("react-redux", () => ({
  ...jest.requireActual("react-redux"),
  useDispatch: () => mockDispatch,
  useSelector: jest.fn(() => ({
    open: true,
    dialogType: "PostFormDialog",
    title: "Test Title",
    dialogProps: {
      disableCloseOnBackdropClick: true,
    },
  })),
}));

describe("useDialog hook", () => {
  const { openDialog, closeDialog } = useDialog();

  beforeEach(() => jest.clearAllMocks());

  it("should open dialog", () => {
    openDialog({
      title: "Test Title",
      dialogType: "PostFormDialog",
      dialogProps: {},
    });
    expect(mockDispatch).toBeCalled();
  });

  it("should close dialog", () => {
    closeDialog();
    expect(mockDispatch).toBeCalled();
  });

  it("should not close dialog on backdrop click", () => {
    closeDialog(null, "backdropClick");
    expect(mockDispatch).not.toBeCalled();
  });
});
