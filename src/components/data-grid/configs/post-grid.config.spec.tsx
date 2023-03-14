import { DataGrid } from "~/components";
import { render } from "~/shared";
import { PostsActions, PostListSelectors } from "~/store";
import { PostGridColumns } from ".";

jest.mock("~/hooks/use-dialog/use-dialog", () => ({
  ...jest.requireActual("~/hooks/use-dialog/use-dialog"),
  __esModule: true,
  default: jest.fn(() => ({})),
}));

jest.mock("~/hooks/use-permissions/use-permissions", () => ({
  ...jest.requireActual("~/hooks/use-permissions/use-permissions"),
  __esModule: true,
  default: jest.fn(() => ({
    hasPermission: jest.fn(),
  })),
}));

jest.mock("react-redux", () => ({
  ...jest.requireActual("react-redux"),
  useDispatch: () => jest.fn(),
  useSelector: jest.fn(() => ({
    loading: false,
    limit: 5,
    page: 0,
    total: 10,
    data: [
      {
        id: "60d21b4667d0d8992e610c85",
        image: "https://img.dummyapi.io/photo-1564694202779-bc908c327862.jpg",
        likes: 43,
        tags: ["animal", "dog", "golden retriever"],
        text: "adult Labrador retriever",
        publishDate: "2020-05-24T14:53:17.598Z",
        owner: {
          id: "60d0fe4f5311236168a109ca",
          title: "ms",
          firstName: "Sara",
          lastName: "Andersen",
          picture: "https://randomuser.me/api/portraits/women/58.jpg",
        },
      },
    ],
    selectedRows: [],
  })),
}));

describe("Post Grid Config", () => {
  const grid = () => (
    <DataGrid
      columns={PostGridColumns()}
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      paginationActions={PostsActions as any}
      paginationSelectors={PostListSelectors}
    />
  );
  it("should match snapshot", () => {
    const { asFragment } = render(grid());
    expect(asFragment()).toMatchSnapshot();
  });
});
