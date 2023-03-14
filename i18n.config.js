const path = require("path");

module.exports = {
  i18n: {
    defaultLocale: "ro",
    locales: ["ro", "ru", "en"],
  },
  localePath: path.resolve("./public/locales"),
  reloadOnPrerender: false,
};
