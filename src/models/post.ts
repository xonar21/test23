import { Nullable } from "~/core";
import { IUserPreview } from "~/models";

export interface IPostPreview {
  id: string;
  text: string;
  image: string;
  likes: number;
  tags: string[];
  publishDate: string;
  owner: Nullable<IUserPreview>;
}

export interface IPost extends IPostPreview {
  link: string;
}

export interface IPostCreate {
  text: string;
  image: string;
  likes: number;
  tags: string[];
  owner: string;
}
