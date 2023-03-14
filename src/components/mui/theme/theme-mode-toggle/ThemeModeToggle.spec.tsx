import { render } from "~/shared";
import { ThemeModeToggle } from "~/components";
import { ThemeModeContext } from "~/context";

describe("Theme mode toggle component", () => {
  it("should render without crashing", () => {
    const { asFragment } = render(<ThemeModeToggle />);
    expect(asFragment()).toMatchSnapshot();
  });

  const { getByTestId } = render(
    <ThemeModeContext.Provider
      value={{
        mode: "dark",
        toggleThemeMode: jest.fn(),
      }}
    >
      <ThemeModeToggle />
    </ThemeModeContext.Provider>,
  );
  expect(getByTestId("WbSunnyIcon")).toBeDefined();
});
