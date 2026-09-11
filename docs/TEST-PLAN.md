# Test Plan

## 1. Objective

The purpose of testing is to verify that the application:

- correctly converts monetary values into words
- handles algorithmic boundary conditions
- validates invalid user input
- returns appropriate HTTP status codes and error messages
- behaves consistently in local and CI environments

## 2. Test Scope

Testing covers three areas:

1. Currency conversion logic
2. HTTP API behaviour

## 3. Unit Testing

Unit tests focus on the `CurrencyToWordsConverter`.

Test cases are concentrated around algorithmic and domain boundaries rather than arbitrary representative values.

### Boundary Cases

Examples include:

- `0`
- `1`
- `19 → 20`
- `99 → 100`
- `999 → 1000`
- `999,999 → 1,000,000`

These values verify transitions between different conversion rules.

### Currency Rules

Tests cover:

- singular and plural dollars
- singular and plural cents
- zero dollars
- dollar and cent combinations
- hyphenation of compound numbers

Example:

`123.45`

Expected:

`ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS`

### Validation

Tests verify rejection of:

- negative values
- values containing more than two decimal places
- values above the supported maximum

## 4. API Integration Testing

Integration tests use the ASP.NET Core test server through
`WebApplicationFactory<Program>`.

These tests exercise the HTTP request pipeline rather than invoking the controller directly.

The following scenarios are covered:

| Scenario                     | Expected Result                  |
| ---------------------------- | -------------------------------- |
| Valid amount                 | `200 OK` with converted result |
| Empty value                  | `400 Bad Request`              |
| Non-numeric value            | `400 Bad Request`              |
| Negative value               | `400 Bad Request`              |
| More than two decimal places | `400 Bad Request`              |
| Value above supported range  | `400 Bad Request`              |

Error responses are also checked to ensure they contain meaningful, user-readable messages.

## 5. Continuous Integration

GitHub Actions automatically performs:

1. dependency restore
2. Release build
3. automated test execution

The CI workflow runs on pushes targeting `main`.


## 6. Continuous Deployment

The application is configured for continuous deployment through Railway.

Each push to the `main` branch triggers the deployment workflow. Railway is configured to wait for the GitHub Actions CI workflow to complete before deploying the new version.

The deployment flow is:

1. Code is pushed to `main`.
2. GitHub Actions restores dependencies, builds the application, and runs the automated test suite.
3. Railway waits for the CI workflow to complete.
4. If CI succeeds, Railway automatically builds and deploys the latest version of the application.
5. If CI fails, the new version is not deployed.

This ensures that only code which successfully passes the automated build and test pipeline is deployed to the public environment.
