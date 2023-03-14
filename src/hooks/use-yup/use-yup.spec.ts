import { ValidationError } from "yup";
import { useYup } from "~/hooks";

jest.mock("react-i18next", () => ({
  ...jest.requireActual("react-i18next"),
  useTranslation: () => ({
    t: jest.fn(str => `\${min} ${str}`),
  }),
}));

describe("useYup hook", () => {
  const params = [undefined, { translationNamespace: "testNamespace", translationPath: "testPath" }];

  params.forEach(yupParams => {
    it(`should be in error state with ${yupParams ? "" : "no "}hook params`, async () => {
      const yup = useYup(yupParams);

      const validationSchema = yup.object({
        name: yup.string().required().min(6),
        age: yup.number().min(18),
      });

      try {
        await validationSchema.validate({ name: "test", age: 16 });
      } catch (err) {
        const { type } = err as ValidationError;
        expect(type).toEqual("min");
      }
    });
  });
});
