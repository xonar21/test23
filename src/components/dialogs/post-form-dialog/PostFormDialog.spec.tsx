import { act, fireEvent, render, testIds } from "~/shared";
import { PostFormDialog } from "~/components";
import { postsSliceInitialState, RootState } from "~/store";
import { IPostPreview } from "~/models";

const mockDispatch = jest.fn();
jest.mock("react-redux", () => ({
  ...jest.requireActual("react-redux"),
  useDispatch: () => mockDispatch,
}));

const post: IPostPreview = {
  id: "",
  image: "",
  likes: 0,
  owner: {
    id: "testuser",
    firstName: "John",
    lastName: "Doe",
    picture: "",
    title: "mr",
  },
  publishDate: "",
  tags: ["test"],
  text: "Lorem ipsum",
};

const preloadedState: RootState = {
  ENTITIES: {
    POSTS: {
      ...postsSliceInitialState,
      form: post,
    },
  },
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
} as any;

describe("Post Form Dialog", () => {
  it("should match snapshot", () => {
    const { asFragment } = render(<PostFormDialog />, { preloadedState });
    expect(asFragment()).toMatchSnapshot();
  });

  const ids = ["", "test"];
  const results = [true, false];

  ids.forEach(id => {
    results.forEach(succeeded => {
      it(`should handle submit with id: '${id}' and result: '${succeeded}'`, async () => {
        mockDispatch.mockReturnValue({ succeeded, payload: { message: "Test" } });
        const { getByTestId } = render(<PostFormDialog />, {
          preloadedState: {
            ...preloadedState,
            ENTITIES: {
              ...preloadedState.ENTITIES,
              POSTS: {
                ...preloadedState.ENTITIES.POSTS,
                form: {
                  ...post,
                  id,
                },
              },
            },
          },
        });

        await act(async () => {
          const submitButton = getByTestId(testIds.components.dialogs.postFormDialog.submitButton);
          fireEvent.click(submitButton);
        });
      });
    });
  });

  it("should clear owner and tags inputs", async () => {
    const { getAllByLabelText } = render(<PostFormDialog />, { preloadedState });
    const clearButtons = getAllByLabelText(/Clear/);
    await act(async () => {
      clearButtons.forEach(button => {
        fireEvent.click(button);
      });
    });
  });
});
