import React, { useState, useEffect } from "react";
import { FormControl, TextField, CircularProgress, Autocomplete, styled } from "@mui/material";
import { useDispatch } from "react-redux";
import { SelectListAction, SelectListSelector, SelectListsActions } from "~/store";

interface AutocompleteFieldProps {
  field: any;
  fieldValue: any;
  fieldKey: string;
  setValue: (newValue: any) => void;
  value: any;
  dinamicTitle: string;
  t: any;
  openModal: boolean;
  selectList: any;
  paramsRequest: any;
}

const AutocompleteField: React.FC<AutocompleteFieldProps> = ({
  field,
  fieldValue,
  fieldKey,
  setValue,
  value,
  dinamicTitle,
  t,
  openModal,
  selectList,
  paramsRequest,
}) => {
  const dispatch = useDispatch();
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [totalPages, setTotalPages] = useState(0);
  const [searchValue, setSearchValue] = useState("");
  const actionKey: SelectListAction = field.action;
  const action = SelectListsActions[actionKey];
  const selectkey: SelectListSelector = field.keySelect;
  const [items, setItems] = useState(selectList[selectkey].items);
  const StyledListbox = styled("ul")({
    "&::-webkit-scrollbar": {
      width: "4px",
    },
    "&::-webkit-scrollbar-thumb": {
      backgroundColor: "rgba(0, 48, 92, 1)",
      borderRadius: "4px",
    },
    "&::-webkit-scrollbar-track": {
      backgroundColor: "rgba(230, 230, 230, 1)",
    },
  });

  useEffect(() => {
    const fetchData = async () => {
      if (field.action && !field.parent) {
        await dispatch(
          action(
            paramsRequest(page, 20, {
              fieldName: "value",
              values: [searchValue],
            }),
          ),
        );
      }
    };

    fetchData();
  }, [page, searchValue]);

  useEffect(() => {
    setTotalPages(selectList[selectkey].totalPages);

    if (selectList[selectkey].totalPages === 1) {
      setItems(selectList[selectkey].items);

      return;
    }

    setItems((prevItems: any) => [...prevItems, ...selectList[selectkey].items]);
  }, [selectList[selectkey].items]);

  useEffect(() => {
    const fetchData = async () => {
      if (field.parent && value[field.parent]) {
        const paramsRequest = (
          id: string,
          number?: number,
          size?: number,
          filters?: object,
          sortField?: string,
          sortOrder?: string,
        ) => {
          return {
            id,
            number,
            size,
            filters,
            sortField,
            sortOrder,
          };
        };

        await dispatch(
          action(
            paramsRequest(value[field.parent], page, 20, {
              fieldName: "value",
              values: [searchValue],
            }),
          ),
        );
      }
    };

    fetchData();
  }, [value[field.parent]]);

  useEffect(() => {
    if (field.parent) {
      setItems([]);
    }
  }, [openModal]);

  return (
    <FormControl fullWidth>
      <Autocomplete
        id="demo-autocomplete"
        options={items}
        value={fieldValue}
        getOptionLabel={option => {
          if (dinamicTitle === t("edit")) {
            let itemValue = "";
            items.forEach((e: any) => {
              if (e.key === option) {
                itemValue = e.value;
              }
            });
            return itemValue;
          }
          return option ? option.value : "";
        }}
        onChange={(e, newValue) => {
          setValue({ ...value, [fieldKey]: newValue?.key });
        }}
        onInputChange={(event, value, reason) => {
          if (reason === "reset") {
            setPage(1);
          }
          if (reason === "input") {
            setSearchValue(value);
            setPage(1);
            setItems([]);
          }
          if (reason === "clear") {
            setPage(1);
            setSearchValue("");
          }
          if (value === "") {
            setPage(1);
            setSearchValue("");
          }
        }}
        ListboxComponent={StyledListbox}
        ListboxProps={{
          onScroll: event => {
            const target = event.target as HTMLUListElement;
            const { scrollTop, scrollHeight, clientHeight } = target;
            if (scrollHeight - scrollTop === clientHeight) {
              setPage(prevPage => {
                if (prevPage !== totalPages) {
                  return prevPage + 1;
                }
                return prevPage;
              });
            }
          },
        }}
        renderInput={params => (
          <TextField
            {...params}
            label={t(`${field.fieldName}`)}
            InputLabelProps={{ ...params.InputLabelProps }}
            InputProps={{
              ...params.InputProps,
              endAdornment: (
                <>
                  {loading ? <CircularProgress size={20} /> : null}
                  {params.InputProps.endAdornment}
                </>
              ),
            }}
          />
        )}
        renderOption={(props, option) => <li {...props}>{option.value}</li>}
      />
    </FormControl>
  );
};

export default AutocompleteField;
