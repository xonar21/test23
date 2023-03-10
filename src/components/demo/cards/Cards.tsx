import React from "react";
import { Grid } from "@mui/material";
import { Card } from "~/components";

const Cards: React.FC = () => {
  return (
    <Grid container spacing={2} mb={3}>
      {plugins.map(plugin => (
        <Grid item lg={3} md={4} sm={6} key={`key-${plugin.name}`} sx={{ width: "100%" }} data-testid="container">
          <Card title={plugin.name} url={plugin.url}>
            {plugin.description}
          </Card>
        </Grid>
      ))}
    </Grid>
  );
};

export const plugins = [
  {
    name: "SASS/SCSS",
    description:
      "Sass is a stylesheet language that’s compiled to CSS. It allows you to use variables, nested rules, mixins, functions, and more, all with a fully CSS-compatible syntax.",
    url: "https://sass-lang.com/documentation",
  },
  {
    name: "Axios",
    description: "Promise based HTTP client for the browser and node.js.",
    url: "https://github.com/axios/axios",
  },
  {
    name: "Environment Variables",
    description: "Use environment variables in your next.js project for server side, client or both.",
    url: "https://github.com/vercel/next.js/tree/canary/examples/environment-variables",
  },
  {
    name: "react-use",
    description: "A Collection of useful React hooks.",
    url: "https://github.com/streamich/react-use",
  },
  {
    name: "React Redux",
    description:
      "Redux helps you write applications that behave consistently, run in different environments (client, server, and native), and are easy to test.",
    url: "https://redux.js.org/introduction/getting-started",
  },
  {
    name: "next-i18next",
    description:
      "next-i18next is a plugin for Next.js projects that allows you to get translations up and running quickly and easily, while fully supporting SSR, multiple namespaces with codesplitting, etc.",
    url: "https://github.com/isaachinman/next-i18next",
  },
  {
    name: "ESLint",
    description:
      "A pluggable and configurable linter tool for identifying and reporting on patterns in JavaScript. Maintain your code quality with ease.",
    url: "https://eslint.org/docs/user-guide/getting-started",
  },
  {
    name: "Prettier",
    description: "An opinionated code formatter; Supports many languages; Integrates with most editors.",
    url: "https://prettier.io/docs/en/index.html",
  },
  {
    name: "lint-staged",
    description:
      " The concept of lint-staged is to run configured linter (or other) tasks on files that are staged in git.",
    url: "https://github.com/okonet/lint-staged",
  },
  {
    name: "Testing Library",
    description:
      "The React Testing Library is a very light-weight solution for testing React components. It provides light utility functions on top of react-dom and react-dom/test-utils.",
    url: "https://testing-library.com/docs/",
  },
  {
    name: "Github Actions",
    description:
      "GitHub Actions makes it easy to automate all your software workflows, now with world-class CI/CD. Build, test, and deploy your code right from GitHub.",
    url: "https://docs.github.com/en/actions",
  },
];

export default Cards;
