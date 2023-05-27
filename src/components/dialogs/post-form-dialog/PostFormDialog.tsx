import React from "react";
import { DialogContent, DialogActions, Button, Grid } from "@mui/material";
import { useSnackbar } from "notistack";
import { useTranslation } from "next-i18next";
import { useDialog, useYup } from "~/hooks";
import { useDispatch, useSelector } from "react-redux";
import { PostsActions, PostsSelectors, SelectListsSelectors, TagsSelectors } from "~/store";
import { Form, Formik } from "formik";
import { IPost, IPostCreate, IPostPreview } from "~/models";
import { IAxiosErrorPayload } from "~/core";
import { testIds } from "~/shared";
import { LoadingButton, FormikTextField, FormikAutocomplete } from "~/components";

const PostFormDialog = () => {
  const dispatch = useDispatch();
  const { t } = useTranslation(["posts"]);
  const { closeDialog } = useDialog();
  const { enqueueSnackbar } = useSnackbar();
  const { submitting, form, limit, page } = useSelector(PostsSelectors.getRoot);
  const { tags } = useSelector(TagsSelectors.getRoot);
  const usersSelectList = useSelector(SelectListsSelectors.getRoot);
  const yup = useYup({
    translationNamespace: "posts",
    translationPath: "table.columns",
  });

  const validationSchema = yup.object({
    owner: yup.object().nullable().required(),
    tags: yup.array(),
    text: yup.string().required().min(8).max(1000),
  });

  const handleSave = async (values: IPostPreview) => {
    const model = values.id
      ? values
      : {
          ...values,
          owner: values.owner?.id as string,
        };
    const action = values.id ? PostsActions.updatePost : PostsActions.createPost;
    const result = await dispatch(action(model as IPostCreate & IPost));

    if (result.succeeded) {
      dispatch(PostsActions.getList({ page: values.id ? page : 0, limit } as any));
      enqueueSnackbar(t("snackbar.saved"), { variant: "success" });
      closeDialog();
      dispatch(PostsActions.FORM_RESET());
    } else {
      const { message } = result.payload as IAxiosErrorPayload;
      enqueueSnackbar(message, { variant: "error" });
    }
  };

  return (
    <Formik initialValues={form as IPostPreview} validationSchema={validationSchema} onSubmit={handleSave}>
      {({ values, handleSubmit }) => (
        <Form onSubmit={handleSubmit} noValidate>
          <DialogContent dividers>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <FormikTextField name="text" label={t("table.columns.text")} rows={4} multiline fullWidth required />
              </Grid>
              <Grid item xs={6}>
                {/* <FormikAutocomplete
                  name="owner"
                  label={t("table.columns.owner")}
                  options={usersSelectList}
                  getOptionLabel={user => `${user}`}
                  disabled={!!values.id}
                  data-testid={testIds.components.dialogs.postFormDialog.owner}
                  fullWidth
                  required
                /> */}
              </Grid>
              <Grid item xs={6}>
                <FormikAutocomplete
                  name="tags"
                  label={t("table.columns.tags")}
                  options={tags}
                  limitTags={2}
                  data-testid={testIds.components.dialogs.postFormDialog.tags}
                  fullWidth
                  multiple
                />
              </Grid>
            </Grid>
          </DialogContent>
          <DialogActions>
            <Button variant="outlined" onClick={closeDialog}>
              {t("common:close")}
            </Button>
            <LoadingButton
              type="submit"
              variant="contained"
              loading={submitting}
              data-testid={testIds.components.dialogs.postFormDialog.submitButton}
            >
              {t("common:save")}
            </LoadingButton>
          </DialogActions>
        </Form>
      )}
    </Formik>
  );
};

export default PostFormDialog;
