const withPlugins = require("next-compose-plugins");
const i18n = require("./i18n.config");

/**
 * @type {import('next').NextConfig}
 **/
const nextConfig = {
  ...i18n,
};

module.exports = withPlugins([], nextConfig);
