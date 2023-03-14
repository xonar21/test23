import { IPostPreview } from "~/models";
import { postListReducer, postsSliceInitialState, PostsActions } from "~/store";

describe("Posts Slice", () => {
  const apiCallSpy = jest.spyOn(require("~/store/actions/api.actions"), "apiCall");
  const form: IPostPreview = {
    id: "",
    image: "",
    likes: 0,
    owner: null,
    publishDate: "",
    tags: [],
    text: "",
  };

  it("should handle FORM_INITIALIZED", () => {
    expect(postListReducer(postsSliceInitialState, { type: PostsActions.FORM_INITIALIZED })).toEqual({
      ...postsSliceInitialState,
      form,
    });
  });

  it("should handle FORM_RESET", () => {
    expect(postListReducer({ ...postsSliceInitialState, form }, { type: PostsActions.FORM_RESET })).toEqual(
      postsSliceInitialState,
    );
  });

  it("should handle POST_SUBMIT_STARTED", () => {
    expect(postListReducer(postsSliceInitialState, { type: PostsActions.POST_SUBMIT_STARTED })).toEqual({
      ...postsSliceInitialState,
      submitting: true,
    });
  });

  it("should handle POST_SUBMIT_ENDED", () => {
    expect(
      postListReducer({ ...postsSliceInitialState, submitting: true }, { type: PostsActions.POST_SUBMIT_ENDED }),
    ).toEqual(postsSliceInitialState);
  });

  it("should create post", () => {
    PostsActions.createPost({ ...form, owner: form.id });
    expect(apiCallSpy).toBeCalled();
  });

  it("should update post", () => {
    PostsActions.updatePost({ ...form, link: "" });
    expect(apiCallSpy).toBeCalled();
  });

  it("should delete post", () => {
    PostsActions.deletePost("1");
    expect(apiCallSpy).toBeCalled();
  });
});
