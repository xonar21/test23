import { Avatar, Typography } from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { Edit as EditIcon, Delete as DeleteIcon } from "@mui/icons-material";
import { IPostPreview, IUserPreview } from "~/models";
import { translate } from "~/shared";
import { useDispatch, useSelector } from "react-redux";
import { PostsActions, PostsSelectors } from "~/store";
import { useDialog, usePermissions } from "~/hooks";
import { useSnackbar } from "notistack";
import { IAxiosErrorPayload } from "~/core";
import { EntityType, UserAction } from "~/security";
import { GridActionsCellItem } from "~/components";

const columns: () => GridColDef[] = () => {
  const dispatch = useDispatch();
  const { openDialog, openPrompt } = useDialog();
  const { enqueueSnackbar } = useSnackbar();
  const { data, page, limit } = useSelector(PostsSelectors.getRoot);
  const { hasPermission } = usePermissions();

  /* istanbul ignore next */
  const initializeUpdateForm = (post: IPostPreview) => {
    dispatch(PostsActions.FORM_INITIALIZED(post));
    openDialog({
      title: translate("posts:updatePost"),
      dialogType: "PostFormDialog",
      dialogProps: {
        maxWidth: "md",
        fullWidth: true,
        disableCloseOnBackdropClick: true,
      },
    });
  };

  /* istanbul ignore next */
  const promptDelete = (post: IPostPreview) => {
    openPrompt({
      title: translate("posts:deletePost"),
      dialogProps: {
        maxWidth: "xs",
        fullWidth: true,
      },
      content: () => <Typography>{translate("posts:deletePostConfirmation")}</Typography>,
      onConfirm: async setLoading => {
        setLoading(true);

        const result = await dispatch(PostsActions.deletePost(post.id));

        if (result.succeeded) {
          const pageParam = data.length < 2 ? (page - 1 < 0 ? 0 : page - 1) : page;
          await dispatch(PostsActions.getList({ page: pageParam, limit } as any));
          enqueueSnackbar(translate("posts:snackbar.deleted"), { variant: "success" });
        } else {
          const { message } = result.payload as IAxiosErrorPayload;
          enqueueSnackbar(message, { variant: "error" });
          return false;
        }
      },
    });
  };

  return [
    {
      field: "text",
      headerName: translate("posts:table.columns.text"),
      sortable: false,
      flex: 2,
    },
    {
      field: "likes",
      headerName: translate("posts:table.columns.likes"),
      flex: 0.15,
    },
    {
      field: "owner",
      headerName: translate("posts:table.columns.owner"),
      valueGetter: ({ row }) => {
        const { userName } = row.owner as IUserPreview;
        return `${userName}`;
      },
      flex: 0.15,
    },
    {
      field: "publishDate",
      headerName: translate("posts:table.columns.publishDate"),
      type: "dateTime",
      valueGetter: /* istanbul ignore next */ ({ value }) => value && new Date(value),
      flex: 0.3,
    },
    {
      field: "actions",
      type: "actions",
      flex: 0.15,
      getActions: /* istanbul ignore next */ (params: { row: IPostPreview }) => [
        <GridActionsCellItem
          icon={<EditIcon />}
          label={translate("posts:updatePost")}
          onClick={() => initializeUpdateForm(params.row)}
          // disabled={!hasPermission(UserAction.Update, EntityType.Post)}
        />,
        <GridActionsCellItem
          icon={<DeleteIcon />}
          label={translate("posts:deletePost")}
          onClick={() => promptDelete(params.row)}
          // disabled={!hasPermission(UserAction.Delete, EntityType.Post)}
        />,
      ],
    },
  ];
};

export default columns;
