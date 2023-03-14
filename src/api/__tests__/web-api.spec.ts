import mockAxios from "jest-mock-axios";
import { REQUEST_TYPES } from "~/core";
import { mocks } from "~/models";
import { WebApi } from "~/api/web-api";

const webApi = WebApi.getInstance();

describe("Web API", () => {
  const defaultResponse = { data: {} };
  const params = mocks.pageRequest;

  beforeAll(() => {
    REQUEST_TYPES.forEach(type => mockAxios[type].mockResolvedValue(defaultResponse));
  });

  describe("Posts", () => {
    it("should get list", async () => {
      await webApi.Posts.getList(params);
      expect(mockAxios.get).toHaveBeenCalledWith("/post", { params });
    });

    it("should get list by user", async () => {
      const userId = "1";
      await webApi.Posts.getListByUser(userId, params);
      expect(mockAxios.get).toHaveBeenCalledWith(`/user/${userId}/post`, { params });
    });

    it("should get list by tag", async () => {
      const tag = "news";
      await webApi.Posts.getListByTag(tag, params);
      expect(mockAxios.get).toHaveBeenCalledWith(`/tag/${tag}/post`, { params });
    });

    it("should get post by id", async () => {
      const id = "1";
      await webApi.Posts.getPostById(id);
      expect(mockAxios.get).toHaveBeenCalledWith(`/post/${id}`, undefined);
    });

    it("should create post", async () => {
      await webApi.Posts.createPost(mocks.postCreate);
      expect(mockAxios.post).toHaveBeenCalledWith("/post/create", mocks.postCreate, undefined);
    });

    it("should update post", async () => {
      await webApi.Posts.updatePost(mocks.post);
      expect(mockAxios.put).toHaveBeenCalledWith(`/post/${mocks.post.id}`, mocks.post, undefined);
    });

    it("should delete post", async () => {
      const id = "1";
      await webApi.Posts.deletePost(id);
      expect(mockAxios.delete).toHaveBeenCalledWith(`/post/${id}`, undefined);
    });
  });

  describe("Users", () => {
    it("should get list", async () => {
      await webApi.Users.getList(params);
      expect(mockAxios.get).toHaveBeenCalledWith("/user", { params });
    });

    it("should get user by id", async () => {
      const id = "1";
      await webApi.Users.getUserById(id);
      expect(mockAxios.get).toHaveBeenCalledWith(`/user/${id}`, undefined);
    });

    it("should create user", async () => {
      await webApi.Users.createUser(mocks.user);
      expect(mockAxios.post).toHaveBeenCalledWith("/user/create", mocks.user, undefined);
    });

    it("should update user", async () => {
      await webApi.Users.updateUser(mocks.user);
      expect(mockAxios.put).toHaveBeenCalledWith(`/user/${mocks.user.id}`, mocks.user, undefined);
    });

    it("should delete user", async () => {
      const id = "1";
      await webApi.Users.deleteUser(id);
      expect(mockAxios.delete).toHaveBeenCalledWith(`/user/${id}`, undefined);
    });
  });

  describe("Tags", () => {
    it("should get list", async () => {
      await webApi.Tags.getList();
      expect(mockAxios.get).toHaveBeenCalledWith("/tag", undefined);
    });
  });

  describe("Auth", () => {
    it("should register", async () => {
      await webApi.Auth.register(mocks.authModel);
      expect(mockAxios.post).toHaveBeenCalledWith("/register", mocks.authModel, { baseUrl: undefined });
    });

    it("should login", async () => {
      await webApi.Auth.login(mocks.authModel);
      expect(mockAxios.post).toHaveBeenCalledWith("/login", mocks.authModel, { baseUrl: undefined });
    });
  });
});
