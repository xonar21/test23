import { ReactElement } from "react";
import { Provider } from "react-redux";
import { render as baseRender, RenderOptions, RenderResult } from "@testing-library/react";
import { reducer, RootState } from "~/store";
import { ThemeProvider } from "~/components";
import { configureStore } from "@reduxjs/toolkit";

/**
 * Custom renderer example with @testing-library/react
 * You can customize it to your needs.
 *
 * To learn more about customizing renderer,
 * please visit https://testing-library.com/docs/react-testing-library/setup
 */
const render = (
  ui: ReactElement,
  options?: Omit<RenderOptions, "queries"> & { preloadedState: Partial<RootState> },
) => {
  const AllTheProviders: React.FC = ({ children }) => {
    const mockStore = configureStore({ reducer, preloadedState: options?.preloadedState });

    return (
      <>
        <Provider store={mockStore}>
          <ThemeProvider>{children}</ThemeProvider>
        </Provider>
      </>
    );
  };

  return baseRender(ui, { wrapper: AllTheProviders, ...options }) as RenderResult;
};

// Re-export everything
export * from "@testing-library/react";

// Override render method
export { render };
