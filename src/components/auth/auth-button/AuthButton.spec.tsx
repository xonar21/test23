import { render, testIds, fireEvent, routes } from "~/shared";
import { AuthButton } from "~/components";
import { authSliceInitialState } from "~/store";

describe("AuthButton", () => {
  const useRouter = jest.spyOn(require("next/router"), "useRouter");
  const mockPush = jest.fn();
  const mockUseRouter = {
    push: mockPush,
  };

  beforeAll(() => useRouter.mockImplementation(() => mockUseRouter));

  afterEach(() => jest.clearAllMocks());

  it("should match snapshot", () => {
    const { asFragment } = render(<AuthButton />);
    expect(asFragment()).toMatchSnapshot();
  });

  const authStates = [true, false];
  authStates.forEach(isAuthenticated => {
    it(`should handleOnClick with isAuthenticated: ${isAuthenticated}`, () => {
      const { getByTestId } = render(<AuthButton />, {
        preloadedState: {
          AUTH: {
            ...authSliceInitialState,
            isAuthenticated,
          },
        },
      });

      const button = getByTestId(testIds.components.header.buttons.authButton);
      fireEvent.click(button);
      expect(mockPush).toBeCalledWith(isAuthenticated ? routes.Home.path : routes.Login.path);
    });
  });
});
