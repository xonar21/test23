import { IAuthModel, ILocation, IPageRequest, IPost, IPostCreate, IPostPreview, IUser, IUserPreview } from "~/models";

const authModel: IAuthModel = { email: "johndoe@domain.com", password: "Pa$$w0rd" };

const location: ILocation = {
  city: "New York",
  country: "USA",
  state: "NY",
  street: "Brooklyn Str",
  timezone: "UTC",
};

const pageRequest: IPageRequest = { page: 0, limit: 10 };

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
  title: "mr",
  firstName: "John",
  lastName: "Doe",
  picture: "https://client.mock/img/1.jpg",
};

const user: IUser = {
  ...userPreview,
  gender: "M",
  email: "johndoe@domain.com",
  dateOfBirth: "1990-05-05",
  registerDate: "2020-03-03",
  phone: "+123456789",
  location: location,
};

export default {
  authModel,
  location,
  pageRequest,
  postCreate,
  postPreview,
  post,
  userPreview,
  user,
};
