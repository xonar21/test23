import { IAuthModel, ILocation, IPageRequest, IPost, IPostCreate, IPostPreview, IUserPreview } from "~/models";

const authModel: IAuthModel = { userName: "johndoe@domain.com", password: "Pa$$w0rd" };

const location: ILocation = {
  city: "New York",
  country: "USA",
  state: "NY",
  street: "Brooklyn Str",
  timezone: "UTC",
};

const pageRequest = { page: 0, limit: 10 };

const postCreate: IPostCreate = {
  image: "https://client.mock/img/1.jpg",
  likes: 0,
  owner: "John Doe",
  tags: ["news"],
  text: "Lorem Ipsum",
};

const postPreview: IPostPreview = {
  ...postCreate,
  id: "1",
  publishDate: "2020-03-03",
  owner: null,
};

const post: IPost = {
  ...postPreview,
  link: "https://client.mock/posts/1",
};

const userPreview: IUserPreview = {
  id: "1",
  email: "admin@mail.ru",
  userName: "Admin",
};

export default {
  authModel,
  location,
  pageRequest,
  postCreate,
  postPreview,
  post,
  userPreview,
};
