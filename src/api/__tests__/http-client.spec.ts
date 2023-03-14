import mockAxios from "jest-mock-axios";
import HttpClient from "~/api/http-client";
import { RequestType, REQUEST_TYPES } from "~/core";

class MockedHttpClient extends HttpClient {
  constructor() {
    super("http://client.mock");
  }

  request(type: RequestType) {
    return this[type]("/test");
  }
}

describe("HTTP Client", () => {
  const client = new MockedHttpClient();
  const data = {};
  const mockedResponse = { data };

  afterEach(() => mockAxios.reset());

  REQUEST_TYPES.forEach(type => {
    it(`should perform ${type} request`, async () => {
      mockAxios[type].mockResolvedValueOnce(mockedResponse);
      const result = await client.request(type);
      expect(result).toEqual(data);
    });
  });
});
