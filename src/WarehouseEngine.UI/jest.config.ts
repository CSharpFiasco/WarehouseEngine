import type { Config } from 'jest';

/*
  Currently writes out coverage but fails to generate full report.
  Tracked here: https://github.com/angular/angular-cli/issues/25293
*/

const config: Config = {
  verbose: true,
  cacheDirectory: '.angular/.jest/cache',
  // collectCoverage: true,
  // coverageDirectory: './coverage',
  // coverageProvider: 'v8',
  // coverageReporters: ['text-summary', 'html'],
  // reporters: ['summary', 'default'],
};

module.exports = config;
