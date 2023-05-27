import React from "react";
import { useDispatch } from "react-redux";
import { GetServerSideProps, NextPage } from "next";
import i18nConfig from "@i18n";
import { serverSideTranslations } from "next-i18next/serverSideTranslations";
import { Button, Typography } from "@mui/material";
import { useTranslation } from "next-i18next";
import { PostsActions, PostListSelectors, reduxWrapper, SelectListsActions, TagsActions } from "~/store";
import { DataGrid, PostGridColumns } from "~/components";
import { useDialog, usePermissions } from "~/hooks";
import { EntityType, UserAction } from "~/security";

const Posts: NextPage = () => {
  const dispatch = useDispatch();
  const { t } = useTranslation();
  const { openDialog } = useDialog();
  const { hasPermission } = usePermissions();

  const handleOpenModal = () => {
    dispatch(PostsActions.FORM_INITIALIZED());
    openDialog({
      title: t("posts:createPost"),
      dialogType: "PostFormDialog",
      dialogProps: {
        maxWidth: "md",
        fullWidth: true,
        disableCloseOnBackdropClick: true,
      },
    });
  };

  return (
    <React.Fragment>
      <Typography variant="h6" mb={1} color="text.primary">
        {t("routes.posts")}
      </Typography>
      <Button
        variant="contained"
        size="small"
        onClick={handleOpenModal}
        // disabled={!hasPermission(UserAction.Create, EntityType.Post)}
        sx={{ mb: 2 }}
      >
        {t("posts:createPost")}
      </Button>
      <DataGrid
        columns={PostGridColumns()}
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        paginationActions={PostsActions as any}
        paginationSelectors={PostListSelectors}
        checkboxSelection
      />
    </React.Fragment>
  );
};

export const getServerSideProps: GetServerSideProps = reduxWrapper.getServerSideProps(
  ({ dispatch }) =>
    async ({ locale }) => {
      await dispatch(PostsActions.getList({ page: 0, limit: 10 } as any));
      // await dispatch(SelectListsActions.getUsersSelectList());
      await dispatch(TagsActions.getTags());

      return {
        props: {
          ...(await serverSideTranslations(locale as string, ["common", "posts"], i18nConfig)),
        },
      };
    },
);

export default Posts;
