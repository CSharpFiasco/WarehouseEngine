const unusedImports = require("eslint-plugin-unused-imports");
const js = require("@eslint/js");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");

module.exports = [{
    ignores: ["projects/**/*"],
}, {
    plugins: {
        "unused-imports": unusedImports,
    },
}, ...[
    js.configs.recommended,
    ...tseslint.configs.recommended,
    ...tseslint.configs.strict,
    ...angular.configs.tsRecommended,
].map(config => ({
    ...config,
    files: ["**/*.ts"],
    processor: angular.processInlineTemplates,
})), {
    files: ["**/*.ts"],

    languageOptions: {
        ecmaVersion: "latest",
        sourceType: "module",

        parserOptions: {
            project: ["tsconfig.(app|spec).json"],
        },
    },

    rules: {
        "@angular-eslint/directive-selector": ["error", {
            type: "attribute",
            prefix: "app",
            style: "camelCase",
        }],

        "@angular-eslint/component-selector": ["error", {
            type: "element",
            prefix: "app",
            style: "kebab-case",
        }],

        "@typescript-eslint/no-extraneous-class": ["error", {
            allowWithDecorator: true,
        }],

        "@typescript-eslint/no-dupe-class-members": "error",
        "@typescript-eslint/no-unused-expressions": "error",
        "@typescript-eslint/no-use-before-define": "error",
        "@typescript-eslint/prefer-readonly": "error",
        "@typescript-eslint/strict-boolean-expressions": "warn",
        "@angular-eslint/prefer-output-readonly": "error",
        "@angular-eslint/no-pipe-impure": "error",
        "@angular-eslint/contextual-lifecycle": "warn",
        "@angular-eslint/prefer-on-push-component-change-detection": "error",
        "@typescript-eslint/no-unused-vars": "off",
        "unused-imports/no-unused-imports": "error",

        "unused-imports/no-unused-vars": ["warn", {
            vars: "all",
            varsIgnorePattern: "^_",
            args: "after-used",
            argsIgnorePattern: "^_",
        }],
    },
}, ...[
    ...angular.configs.templateRecommended,
    ...angular.configs.templateAccessibility,
].map(config => ({
    ...config,
    files: ["**/*.html"],
})), {
    files: ["**/*.html"],

    rules: {
        "@angular-eslint/template/eqeqeq": ["error", {
            allowNullOrUndefined: true,
        }],

        "@angular-eslint/template/conditional-complexity": ["error", {
            maxComplexity: 3,
        }],

        "@angular-eslint/template/no-inline-styles": "warn",
        "@angular-eslint/template/no-duplicate-attributes": "error",
        "@angular-eslint/template/attributes-order": "warn",
    },
}];
