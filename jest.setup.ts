import "isomorphic-unfetch";
import nock from "nock";
import dotenv from "dotenv";
import httpAdapter from "axios/lib/adapters/http";
import axios from "axios";

dotenv.config({ path: ".env.test" });

axios.defaults.adapter = httpAdapter;

afterAll(() => {
  nock.cleanAll();
  nock.restore();
});

window.matchMedia = jest.fn().mockImplementation(query => {
  return {
    matches: false,
    media: query,
    onchange: null,
    addListener: jest.fn(),
    removeListener: jest.fn(),
  };
});

window.scroll = jest.fn();
window.alert = jest.fn();

jest.mock("next/router", () => ({
  ...jest.requireActual("next/router"),
  useRouter: () => ({
    push: jest.fn(),
    pathname: "/",
  }),
}));

jest.mock("react-i18next", () => ({
  ...jest.requireActual("react-i18next"),
  useTranslation: () => ({
    t: jest.fn(str => str),
  }),
}));

jest.mock("~/api/web-api", () => ({
  ...jest.requireActual("~/api/web-api"),
  getPromisePath: jest.fn(() => []),
  Posts: jest.fn(),
  Users: jest.fn(),
  Tags: jest.fn(),
  Auth: jest.fn(),
}));

jest.mock("notistack", () => ({
  ...jest.requireActual("notistack"),
  useSnackbar: () => ({
    enqueueSnackbar: jest.fn(),
  }),
}));
