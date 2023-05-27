import { render, fireEvent, testIds, Locale } from "~/shared";
import { Header } from "~/components";

describe("Header component", () => {
  it("should render without crashing", () => {
    const { asFragment } = render(<Header />);
    expect(asFragment()).toMatchSnapshot();
  });

  it("should toggle drawer", () => {
    const { getByTestId } = render(<Header />);
    const button = getByTestId(testIds.components.header.buttons.drawerToggler);
    const drawer = getByTestId(testIds.components.navigationDrawer.root);
    fireEvent.click(button);
    const expected = "true";
    const actual = drawer.attributes.getNamedItem("aria-expanded")?.value;
    expect(actual).toEqual(expected);
  });

  it("should handle redirect on navigation link click", () => {
    const { getAllByTestId } = render(<Header />);
    const link = getAllByTestId(testIds.components.navigationDrawer.navigationLink)[0];
    fireEvent.click(link);
  });

  it("should change locale without crashing", () => {
    const { getByTestId, getAllByTestId } = render(<Header />);

    const localeMenu = getByTestId(testIds.components.header.buttons.localeMenu);

    for (let i = 0; i < Object.keys(Locale).length; i++) {
      fireEvent.click(localeMenu);
      const localeItem = getAllByTestId(testIds.components.header.localeItem)[i];
      fireEvent.click(localeItem);
    }
  });
});
