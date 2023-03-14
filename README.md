# CEC ITSS ESL

## Technologies

This application includes technologies and features as:

- **React 17** - A JavaScript library for building reactive user interfaces.
- **Next.js 12** - framework enabling React apps functionalities such as server-side rendering.
- **TypeScript** - Adds static type definitions to JavaScript code.
- **Redux Toolkit** - Toolset for efficient Redux development.
- **Babel** - Transpiler to different versions of EcmaScript.
- **Jest** - Testing framework.
- **ESLint** - Static code analysis tool for identifying problematic patterns found in JavaScript code.
- **Prettier** - An opinionated code formatter.
- **EditorConfig** - Maintains consistent coding styles for developers working on the same project across various IDEs.
- **Axios** - Promise based HTTP client for the browser and node.js.
- **i18next** - Lightweight simple translation module with dynamic JSON storage.
- **SASS** - CSS with superpowers.
- **Material UI** - Provides a robust, customizable, and accessible library of foundational and advanced components.

### **IMPORTANT**: Recommended Node.js version: **^16.15.0**

## Setup and Scripts

> **Install** all the dependencies before starting the application

```sh
npm install
```

> **Run** the project for local development.

```sh
npm run dev
```

> **Build** the project for a production environment.

```sh
npm run build
```

> **Run** the production server.

```sh
npm run start
```

> **Lint** your code.

```sh
npm run lint
```

> Run **tests** in the interactive mode.

```sh
npm test
```

> Run **tests** and generate a coverage report.

```sh
npm run test:cov
```

## Environment variables

This project can consume variables declared in the environment as if they were declared locally in the JS files. By default you will have **NODE_ENV** defined for you, and any other environment variables starting with **NEXT_PUBLIC**.

#### Setting a variable

> .env.development

```sh
NEXT_PUBLIC_API_URL=https://dummyapi.io/data/v1/
```

#### Consuming a variable

> Accessing **process.env**

```js
this.baseUrl = process.env.NEXT_PUBLIC_API_URL;
```

_Read more: [[Adding Custom Environment Variables]][adding-custom-environment-variables]_

## Localization

**next-i18next** is a plugin that allows you to get translations up and running quickly and easily, while fully supporting SSR, multiple namespaces with codesplitting.
The translations of custom text messsages will be stored in each language's own separate folder.<br/>

> Example translation folder structure:

```
.
└── static
    └── locales
        ├── en
        |   └── common.json
        ├── ro
            └── common.json
```

> i18n.config.js

```js
const path = require("path");

module.exports = {
  i18n: {
    defaultLocale: "en",
    locales: ["en", "ro"],
  },
  localePath: path.resolve("./public/locales"),
};
```

#### Usage

> Page component

```tsx
export const getServerSideProps: GetServerSideProps = async ({ locale }) => {
  return {
    props: {
      ...(await serverSideTranslations(locale as string, ["common"], i18nConfig)),
    },
  };
};
```

> Functional component

```tsx
import React from "react";
import { useTranslation } from "next-i18next";

const Component = () => {
  const { t } = useTranslation(["common"]);

  return <div>{t("hello")}</div>;
};
```

_Read more: [[Official documentation and detailed usage of next-i18next]][i18next-documentation]_

## Redux with Next.js

Setting up Redux for static apps is rather simple: a single Redux store has to be created that is provided to all pages.

When Next.js static site generator or server side rendering is involved, however, things start to get complicated as another store instance is needed on the server to render Redux-connected components.

Therefore, this app uses **next-redux-wrapper**. It automatically creates the store instances for you and makes sure they all have the same state.
<br/>

#### Usage

> Page component

```tsx
export const getServerSideProps: GetServerSideProps = reduxWrapper.getServerSideProps(
  async ({ dispatch }) =>
    await dispatch(/* your action */);

    (ctx) => {
      return {
        props: {},
      };
    },
);
```

Please note that your reducer must have the **HYDRATE** action handler. **HYDRATE** action handler must properly reconciliate the hydrated state on top of the existing state (if any).

_Read more: [[Official documentation and detailed usage of next-redux-wrapper]][next-redux-wrapper-docs]_

## Routing

This project has a preconfigured private/annonymous routing system. It makes use of the Next.js middleware api.

To register a route, add a property in the **IApplicationRoutes** interface like so:

```tsx
export interface IApplicationRoutes {
  MyCustomPage: IApplicationRoute;
}
```

Now add the route configuration in the **routes** object:

```tsx
export const routes: IApplicationRoutes = {
  MyCustomPage: {
    path: "/",
    authorized: true, // Marks the route as authorized. Only a logged user can access this page
  },
};
```

You can also mark the route as an **annonymous** one with **annonymous: true**. This will prevent logged users to access the route.

## Layouts

Each page component created under the **pages** folder uses the default layout component, preconfigured in the **\_app.tsx** file. To use a custom layout for a Next.js page, you can specify the layout explicitly by setting the layout property (please note that the page component must have the custom **NextPageWithLayout** type):

```tsx
import { NextPageWithLayout } from "~/core";
import { AuthForm } from "~/components";
import { AuthLayout } from "~/layouts";

const Login: NextPageWithLayout = () => {
  return <AuthForm type="login" />;
};

Login.layout = AuthLayout;

export default Login;
```

[adding-custom-environment-variables]: https://pankod.github.io/superplate/docs/nextjs/env
[i18next-documentation]: https://github.com/isaachinman/next-i18next
[next-redux-wrapper-docs]: https://github.com/kirill-konshin/next-redux-wrapper
