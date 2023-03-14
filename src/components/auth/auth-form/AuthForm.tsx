import React, { useState } from "react";
import NextLink from "next/link";
import { Box, CardActions, CardContent, Link, Typography } from "@mui/material";
import { useTranslation } from "next-i18next";
import { Form, Formik } from "formik";
import { useDispatch, useSelector } from "react-redux";
import { useSnackbar } from "notistack";
import { useRouter } from "next/router";
import { AuthActionType, IAuthError, IAuthModel, IAuthResponse } from "~/models";
import { AuthActions, AuthSelectors } from "~/store";
import { IAxiosErrorPayload } from "~/core";
import { CookieKeys, getRoutePath, QueryKeys, routes, testIds } from "~/shared";
import { useCookie } from "react-use";
import { LoadingButton } from "~/components";
import { useYup } from "~/hooks";
import { FormikTextField } from "~/components/formik";

interface IAuthFormProps {
  type: AuthActionType;
}

const AuthForm: React.FC<IAuthFormProps> = ({ type }) => {
  const dispatch = useDispatch();
  const [, updateAuthCookie] = useCookie(CookieKeys.AuthToken);
  const { query } = useRouter();
  const { t } = useTranslation();
  const { enqueueSnackbar } = useSnackbar();
  const { loading } = useSelector(AuthSelectors.getRoot);
  const [redirecting, setRedirecting] = useState(false);
  const [model] = useState<IAuthModel>({
    email: "george.bluth@reqres.in",
    password: "Pa$$w0rd",
  });
  const yup = useYup({
    translationNamespace: "common",
    translationPath: "auth",
  });

  const validationSchema = yup.object({
    email: yup.string().required().email(),
    password: yup.string().required().min(6),
  });

  const handleSubmit = async (values: IAuthModel) => {
    const result = await dispatch(AuthActions[type](values));

    if (result.succeeded) {
      const { token } = result.payload as IAuthResponse;
      updateAuthCookie(token);
      window.location.href = (query[QueryKeys.ReturnUrl] as string) || getRoutePath(routes.Home);
      setRedirecting(true);
      enqueueSnackbar(t("auth.successfulLogin"), { variant: "success" });
    } else {
      const { data } = result.payload as IAxiosErrorPayload<IAuthError>;
      enqueueSnackbar(data?.error, { variant: "error" });
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
                  name="email"
                  label={t("auth.email")}
                  data-testid={testIds.components.authForm.email}
                  fullWidth
                  required
                />
              </Box>
              <Box>
                <FormikTextField
                  name="password"
                  type="password"
                  label={t("auth.password")}
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
              <NextLink href={getRoutePath(type === "login" ? routes.Register : routes.Login)} passHref>
                <Link underline="hover" variant="body2" color={({ palette }) => palette.text.primary}>
                  {t(`auth.redirectFrom.${type}`)}
                </Link>
              </NextLink>
            </CardActions>
          </Form>
        )}
      </Formik>
    </React.Fragment>
  );
};

export default AuthForm;
