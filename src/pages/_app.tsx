import React, { ReactElement } from "react";
import { Provider } from "react-redux";
import { appWithTranslation } from "next-i18next";
import { store, reduxWrapper } from "~/store";
import { ServiceCollection, ServiceCollectionBuilder } from "~/context";
import { AppPropsWithLayout } from "~/core";
import { DefaultLayout } from "~/layouts";
import { AuthProvider, DialogProvider, PageLoadingIndicator, SnackbarProvider, ThemeProvider } from "~/components";
import "~/styles/scss/index.scss";

function MyApp({ Component, pageProps }: AppPropsWithLayout): JSX.Element {
  const getLayout = (page: ReactElement) => {
    const Layout = Component.layout ?? DefaultLayout;

    return <Layout>{page}</Layout>;
  };

  return (
    <Provider store={store}>
      <ServiceCollection.Provider value={ServiceCollectionBuilder.initialize()}>
        <AuthProvider>
          <ThemeProvider>
            <PageLoadingIndicator>
              <SnackbarProvider>
                <DialogProvider>{getLayout(<Component {...pageProps} />)}</DialogProvider>
              </SnackbarProvider>
            </PageLoadingIndicator>
          </ThemeProvider>
        </AuthProvider>
      </ServiceCollection.Provider>
    </Provider>
  );
}

export default reduxWrapper.withRedux(appWithTranslation(MyApp));
