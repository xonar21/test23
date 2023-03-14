import { Autocomplete, AutocompleteProps, TextField } from "@mui/material";
import { useField } from "formik";
import React from "react";

type FormikAutocompleteProps<
  T,
  Multiple extends boolean | undefined,
  DisableClearable extends boolean | undefined,
  FreeSolo extends boolean | undefined,
> = Omit<AutocompleteProps<T, Multiple, DisableClearable, FreeSolo>, "renderInput"> & {
  name: string;
  label?: string;
  required?: boolean;
};

const FormikAutocomplete = <
  T,
  Multiple extends boolean | undefined = false,
  DisableClearable extends boolean | undefined = false,
  FreeSolo extends boolean | undefined = false,
>({
  name,
  label,
  required,
  ...props
}: FormikAutocompleteProps<T, Multiple, DisableClearable, FreeSolo>) => {
  const [field, meta, helpers] = useField(name);

  return (
    <Autocomplete
      value={field.value}
      onChange={(_, value) => helpers.setValue(value)}
      onBlur={e => {
        helpers.setTouched(true);
        field.onBlur(e);
      }}
      {...props}
      renderInput={params => (
        <TextField
          {...params}
          label={label}
          error={meta.touched && !!meta.error}
          helperText={meta.touched && meta.error}
          required={required}
        />
      )}
    />
  );
};

export default FormikAutocomplete;
