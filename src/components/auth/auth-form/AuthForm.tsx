import React, { useState } from "react";
import { Box, CardActions, CardContent, Typography } from "@mui/material";
import { useTranslation } from "next-i18next";
import { Form, Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { AuthActionType, IMpassModel, IMpassResponse } from "~/models";
import { AuthActions, AuthSelectors, UserActions } from "~/store";
import { CookieKeys, getRoutePath, routes, testIds } from "~/shared";
import { useCookie } from "react-use";
import { LoadingButton } from "~/components";
import { useYup } from "~/hooks";
import { FormikTextField } from "~/components/formik";
import ReactDOM from "react-dom";
import SuccessModal from "~/components/successModal";

interface IAuthFormProps {
  type: AuthActionType;
}

const AuthForm: React.FC<IAuthFormProps> = ({ type }) => {
  const dispatch = useDispatch();
  const [, updateAuthCookie] = useCookie(CookieKeys.AuthToken);
  const [, updateRefreshToken] = useCookie(CookieKeys.AuthRefreshToken);
  const [, updateIdnp] = useCookie(CookieKeys.ExternalUserIdnp);
  const [, updateEmail] = useCookie(CookieKeys.ExternalUserEmail);
  const { t } = useTranslation();
  const { loading } = useSelector(AuthSelectors.getRoot);
  const [redirecting, setRedirecting] = useState(false);
  const [model] = useState<IMpassModel>({
    requestId: "123456",
    idnp: "",
    firstName: "jen",
    nameIdentifier: "test23",
    lastName: "done",
    phoneNumber: "11122233",
    email: "",
    gender: "male",
    birthDate: "2000-03-29T14:20:51.863Z",
  });
  const yup = useYup({
    translationNamespace: "common",
    translationPath: "auth",
  });

  const validationSchema = yup.object({
    idnp: yup.string().required().min(13),
    email: yup.string().required().email(),
  });

  const extractToken = (url: string) => {
    const matchToken = url.match(/token=([^&]*)/);
    const token = matchToken ? matchToken[1] : "";

    return token;
  };

  const extractRefreshToken = (url: string) => {
    const matchRefreshToken = url.match(/refreshToken=([^&]*)/);
    const refreshToken = matchRefreshToken ? matchRefreshToken[1] : "";

    return refreshToken;
  };

  const showSuccessModal = (title: string, message: string) => {
    const successModal = document.createElement("div");
    document.body.appendChild(successModal);

    ReactDOM.render(React.createElement(SuccessModal, { title, message }), successModal);
  };

  const handleSubmit = async (values: IMpassModel) => {
    const result = await dispatch(AuthActions.loginMpass(values));
    const { statusCode, url } = result.payload as IMpassResponse;
    if (statusCode) {
      return;
    }

    if (url) {
      updateAuthCookie(extractToken(url));
      updateRefreshToken(extractRefreshToken(url));
      const action: any = { token: extractToken(url), refreshToken: extractRefreshToken(url) };

      updateIdnp(values.idnp);
      updateEmail(values.email);
      await dispatch(AuthActions.AUTH_REQUEST_SUCCEEDED(action));
      await dispatch(UserActions.getCurrentUser());
      showSuccessModal("Succes", "Înregistrare finalizată cu succes");

      window.location.href = getRoutePath(routes.Home);
      setRedirecting(true);
    }
  };

  return (
    <React.Fragment>
      <Formik initialValues={model} validationSchema={validationSchema} onSubmit={handleSubmit}>
        {({ handleSubmit }) => (
          <Form onSubmit={handleSubmit} noValidate>
            <CardContent sx={{ padding: ({ spacing }) => spacing(0, 0, 3, 0) }}>
              <Typography mb={3} variant="h5">
                {t(`auth.${type}`)}
              </Typography>
              <Box mb={3}>
                <FormikTextField
                  name="idnp"
                  label="Idnp"
                  data-testid={testIds.components.authForm.email}
                  fullWidth
                  required
                />
              </Box>
              <Box>
                <FormikTextField
                  name="email"
                  type="email"
                  label={t("auth.email")}
                  data-testid={testIds.components.authForm.password}
                  fullWidth
                  required
                />
              </Box>
            </CardContent>
            <CardActions sx={{ display: "block", padding: 0, textAlign: "center" }}>
              <LoadingButton
                fullWidth
                type="submit"
                variant="contained"
                size="large"
                loading={loading || redirecting}
                data-testid={testIds.components.authForm.submitButton}
                sx={{ marginBottom: 2 }}
              >
                {t(`auth.${type}`)}
              </LoadingButton>
            </CardActions>
          </Form>
        )}
      </Formik>
    </React.Fragment>
  );
};

export default AuthForm;
