const unusedImports = require("eslint-plugin-unused-imports");
const js = require("@eslint/js");

const {
    FlatCompat,
} = require("@eslint/eslintrc");

const compat = new FlatCompat({
    baseDirectory: __dirname,
    recommendedConfig: js.configs.recommended,
    allConfig: js.configs.all
});

module.exports = [{
    ignores: ["projects/**/*"],
}, {
    plugins: {
        "unused-imports": unusedImports,
    },
}, ...compat.extends(
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:@typescript-eslint/strict",
    "plugin:@angular-eslint/recommended",
    "plugin:@angular-eslint/template/process-inline-templates",
).map(config => ({
    ...config,
    files: ["**/*.ts"],
})), {
    files: ["**/*.ts"],

    languageOptions: {
        ecmaVersion: 5,
        sourceType: "script",

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
        "@angular-eslint/no-conflicting-lifecycle": "warn",
        "@typescript-eslint/no-unused-vars": "off",
        "unused-imports/no-unused-imports": "error",

        "unused-imports/no-unused-vars": ["warn", {
            vars: "all",
            varsIgnorePattern: "^_",
            args: "after-used",
            argsIgnorePattern: "^_",
        }],
    },
}, ...compat.extends(
    "plugin:@angular-eslint/template/recommended",
    "plugin:@angular-eslint/template/accessibility",
).map(config => ({
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

        "@angular-eslint/template/no-call-expression": "error",
        "@angular-eslint/template/no-inline-styles": "warn",
        "@angular-eslint/template/no-duplicate-attributes": "error",
        "@angular-eslint/template/attributes-order": "warn",
    },
}];
