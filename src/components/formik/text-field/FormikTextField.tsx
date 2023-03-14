import React from "react";
import { Field, useField } from "formik";
import { TextFieldProps, TextField } from "@mui/material";

type FormikTextFieldProps = Omit<TextFieldProps, "name"> & {
  name: string;
};

const FormikTextField: React.FC<FormikTextFieldProps> = props => {
  const [, meta] = useField(props.name);

  return (
    <Field as={TextField} error={meta.touched && !!meta.error} helperText={meta.touched && meta.error} {...props} />
  );
};

export default FormikTextField;
