import React, { ReactElement } from "react";
import { Form, Formik } from "formik";
import * as yup from "yup";
import { FormikAutocomplete, FormikTextField } from "~/components";
import { fireEvent, render, act } from "~/shared";

describe("Formik Controls", () => {
  const mockOnSubmit = jest.fn();
  const initialValues = {
    name: "",
    city: "NY",
  };
  const validationSchema = yup.object({
    name: yup.string().required(),
    city: yup.object().nullable().required(),
  });
  const testIds = {
    nameInput: "name-input",
    cityAutocomplete: "city-autocomplete",
    submitButton: "submit-button",
  };

  const wrapFieldWithForm = (element: ReactElement) => (
    <Formik initialValues={initialValues} validationSchema={validationSchema} onSubmit={mockOnSubmit}>
      {({ handleSubmit }) => (
        <Form onSubmit={handleSubmit}>
          {element}
          <button type="submit" data-testid={testIds.submitButton} />
        </Form>
      )}
    </Formik>
  );

  it("form should not be submitted with invalid name", async () => {
    const { getByTestId } = render(wrapFieldWithForm(<FormikTextField name="name" data-testid={testIds.nameInput} />));

    await act(async () => {
      const submitButton = getByTestId(testIds.submitButton);
      fireEvent.click(submitButton);
    });

    expect(mockOnSubmit).not.toBeCalled();
  });

  it("form should not be submitted with invalid autocomplete value", async () => {
    const { getByLabelText, getByTestId } = render(
      wrapFieldWithForm(<FormikAutocomplete name="city" options={["NY"]} data-testid={testIds.cityAutocomplete} />),
    );

    await act(async () => {
      const autocomplete = getByTestId(testIds.cityAutocomplete).querySelector("input") as HTMLInputElement;
      fireEvent.blur(autocomplete);

      const clearButton = getByLabelText(/Clear/);
      fireEvent.click(clearButton);

      const submitButton = getByTestId(testIds.submitButton);
      fireEvent.click(submitButton);
    });

    expect(mockOnSubmit).not.toBeCalled();
  });
});
