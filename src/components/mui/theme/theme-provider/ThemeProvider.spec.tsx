import { useContext } from "react";
import { render, fireEvent } from "~/shared";
import { ThemeProvider } from "~/components";
import { ThemeModeContext } from "~/context";

const mockUseMediaQuery = jest.fn(() => false);
const localStorageGetSpy = jest.spyOn(window.localStorage.__proto__, "getItem");
const localStorageSetSpy = jest.spyOn(window.localStorage.__proto__, "setItem");

const Toggler = () => {
  const { toggleThemeMode } = useContext(ThemeModeContext);
  return <button onClick={toggleThemeMode}></button>;
};

jest.mock("@mui/material", () => ({
  ...jest.requireActual("@mui/material"),
  __esModule: true,
  useMediaQuery: () => mockUseMediaQuery(),
}));

describe("ThemeProvider component", () => {
  beforeEach(() => jest.resetAllMocks());

  it("renders without crashing", () => {
    const { asFragment } = render(<ThemeProvider />);
    expect(asFragment()).toMatchSnapshot();
  });

  it("renders with dark preffered color scheme", () => {
    mockUseMediaQuery.mockImplementation(() => true);
    const { asFragment } = render(<ThemeProvider />);
    expect(asFragment()).toMatchSnapshot();
  });

  it("renders with local storage value", () => {
    localStorageGetSpy.mockImplementation(() => "light");
    const { asFragment } = render(<ThemeProvider />);
    expect(asFragment()).toMatchSnapshot();
  });

  it("changes theme mode accordingly", () => {
    const { getByRole } = render(
      <ThemeProvider>
        <Toggler />
      </ThemeProvider>,
    );
    const button = getByRole("button");
    fireEvent.click(button);
    fireEvent.click(button);
    expect(localStorageSetSpy).toBeCalledTimes(2);
  });
});
