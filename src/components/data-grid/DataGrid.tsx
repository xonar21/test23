import React, { useEffect, useRef, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { DataGrid as MuiDataGrid, DataGridProps, GridSelectionModel } from "@mui/x-data-grid";
import { PaginationActions, PaginationSelectors } from "~/store";

interface IDataGridProps<TEntity> extends Omit<DataGridProps, "rows"> {
  paginationActions: PaginationActions;
  paginationSelectors: PaginationSelectors<TEntity>;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const DataGrid: React.FC<IDataGridProps<any>> = ({
  paginationActions,
  paginationSelectors,
  rowsPerPageOptions = [5, 10, 25, 50],
  ...rest
}) => {
  const dispatch = useDispatch();
  const { root } = paginationSelectors;
  const { PAGE_SIZE_CHANGED, PAGE_CHANGED, SELECTED_ROWS_CHANGED, getList } = paginationActions;
  const { loading, limit, page, total, data, selectedRows } = useSelector(root);
  const prevSelectedRows = useRef<GridSelectionModel>(selectedRows);
  const [loaded, setLoaded] = useState(false);

  /* istanbul ignore next */
  useEffect(() => {
    if (loaded) {
      let active = true;

      (async () => {
        await dispatch(getList({ page, limit } as any));
        if (!active) return;
        setTimeout(() => {
          dispatch(SELECTED_ROWS_CHANGED(prevSelectedRows.current));
        });
      })();

      return () => {
        active = false;
      };
    } else {
      setLoaded(true);
    }
  }, [page, limit]);

  /* istanbul ignore next */
  const handleOnPageSizeChange = (pageSize: number) => {
    dispatch(PAGE_SIZE_CHANGED(pageSize));
  };

  const handleOnPageChange = (page: number) => {
    prevSelectedRows.current = selectedRows;
    dispatch(PAGE_CHANGED(page));
  };

  const handleOnSelectionModelChange = (selectionModel: GridSelectionModel) => {
    dispatch(SELECTED_ROWS_CHANGED(selectionModel));
  };

  return (
    <MuiDataGrid
      paginationMode="server"
      autoHeight
      disableSelectionOnClick
      rows={data}
      loading={loading}
      rowsPerPageOptions={rowsPerPageOptions}
      pageSize={limit}
      page={page}
      rowCount={total}
      selectionModel={selectedRows}
      onPageSizeChange={handleOnPageSizeChange}
      onPageChange={handleOnPageChange}
      onSelectionModelChange={handleOnSelectionModelChange}
      {...rest}
    />
  );
};

export default DataGrid;
