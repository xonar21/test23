import { AuthForm } from "~/components";
import { AuthActionType } from "~/models";
import { render, testIds, fireEvent, act } from "~/shared";

const mockDispatch = jest.fn();
jest.mock("react-redux", () => ({
  ...jest.requireActual("react-redux"),
  useDispatch: () => mockDispatch,
}));

jest.mock("next/router", () => ({
  ...jest.requireActual("next/router"),
  useRouter: () => ({
    query: {},
  }),
}));

describe("Auth Form component", () => {
  it("should match snapshot", () => {
    const { asFragment } = render(<AuthForm type="login" />);
    expect(asFragment()).toMatchSnapshot();
  });

  const actions: AuthActionType[] = ["login", "register"];
  const payloads = [
    {
      succeeded: true,
      payload: { token: "test" },
    },
    {
      succeeded: false,
      payload: { data: { error: "Test error" } },
    },
  ];

  actions.forEach(action => {
    payloads.forEach(payload => {
      it(`should ${action} with succeeded: ${payload.succeeded}`, async () => {
        mockDispatch.mockReturnValue(payload);

        const { getByTestId } = render(<AuthForm type={action} />);
        const submitButton = getByTestId(testIds.components.authForm.submitButton);
        const emailInput = getByTestId(testIds.components.authForm.email).querySelector("input") as HTMLInputElement;
        const passwordInput = getByTestId(testIds.components.authForm.password).querySelector(
          "input",
        ) as HTMLInputElement;

        await act(async () => {
          fireEvent.input(emailInput, { target: { value: "johndoe@domain.com" } });
          fireEvent.input(passwordInput, { target: { value: "Pa$$w0rd" } });
        });

        await act(async () => {
          fireEvent.click(submitButton);
        });
      });
    });
  });
});
