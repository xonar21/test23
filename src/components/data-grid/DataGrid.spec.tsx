/* eslint-disable @typescript-eslint/no-explicit-any */
import { DataGrid } from "~/components";
import { fireEvent, render } from "~/shared";

const mockDispatch = jest.fn();
const mockSelectors = jest.fn();
const mockActions = {
  getList: jest.fn(),
  SELECTED_ROWS_CHANGED: jest.fn(),
  PAGE_CHANGED: jest.fn(),
};

jest.mock("react-redux", () => ({
  ...jest.requireActual("react-redux"),
  useDispatch: () => mockDispatch,
  useSelector: jest.fn(() => ({
    loading: false,
    limit: 5,
    page: 0,
    total: 10,
    data: [{ id: 1 }, { id: 2 }, { id: 3 }, { id: 4 }, { id: 5 }],
    selectedRows: [],
  })),
}));

describe("DataGrid component", () => {
  const columns = [
    {
      field: "id",
      headerName: "Id",
    },
  ];

  const grid = () => (
    <DataGrid
      paginationActions={mockActions as any}
      paginationSelectors={mockSelectors as any}
      columns={columns}
      checkboxSelection
    />
  );

  it("should match snapshot", () => {
    const { asFragment } = render(grid());
    expect(asFragment()).toMatchSnapshot();
  });

  it("should change page", () => {
    const { getByLabelText } = render(grid());
    const button = getByLabelText("Go to next page");
    fireEvent.click(button);
    expect(mockActions.PAGE_CHANGED).toBeCalled();
  });

  it("should select all rows", () => {
    const { getByLabelText } = render(grid());
    const checkbox = getByLabelText("Select all rows");
    fireEvent.click(checkbox);
    expect(mockActions.SELECTED_ROWS_CHANGED).toBeCalled();
  });
});
