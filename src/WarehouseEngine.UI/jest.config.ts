import type { Config } from 'jest';

const config: Config = {
    // verbose: true,
    // collectCoverage: true,
    // coverageReporters: ["text-summary", "html"],
    cacheDirectory: ".angular/.jest/cache",
    collectCoverage: true,
    coverageDirectory: "<rootDir>/coverage",
    coverageProvider: "v8",
    coverageReporters: ["text-summary", "html"],
    reporters: ["summary", "default"],
  };
  
  module.exports = config;