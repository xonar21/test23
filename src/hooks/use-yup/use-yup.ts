import * as yup from "yup";
import * as YupLocale from "yup/lib/locale";
import { MessageParams } from "yup/lib/types";
import { useTranslation } from "next-i18next";

interface IUseYupHookProps {
  translationNamespace?: string;
  translationPath?: string;
}

const keys = ["array", "boolean", "date", "mixed", "number", "object", "string"];

const useYup: (props?: IUseYupHookProps) => typeof yup = props => {
  const namespaces = ["common", props?.translationNamespace].filter(n => n) as string[];
  const { t } = useTranslation(namespaces);

  const translate = (translationPath: string, params: MessageParams) => {
    let errorMessage = t(translationPath).replace(
      "${path}",
      props?.translationPath ? t(`${props.translationNamespace}:${props.translationPath}.${params.path}`) : params.path,
    );

    Object.keys(params).forEach(key => {
      const keyToReplace = `\${${key}}`;
      if (errorMessage.indexOf(keyToReplace) !== -1)
        errorMessage = errorMessage.replace(`\${${key}}`, params[key as keyof MessageParams]);
    });

    return errorMessage;
  };

  const getLocale = () => {
    const localeObject: Record<string, Record<string, unknown>> = {};

    keys.forEach(key => {
      localeObject[key] = {};

      Object.keys(YupLocale[key as keyof YupLocale.LocaleObject]).forEach(rule => {
        localeObject[key][rule] = (params: MessageParams) => translate(`yup.${key}.${rule}`, params);
      });
    });

    return localeObject;
  };

  yup.setLocale(getLocale());

  return yup;
};

export default useYup;
