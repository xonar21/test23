import { i18n } from "next-i18next";

export enum Locale {
  Romanian = "ro",
  Russian = "ru",
}

export const translate = (key: string | string[]) => {
  return i18n?.t(key) as string;
};
