## Assumptions

The specification provides an example of converting a monetary value into its English dollar and cent representation, but does not define all supported input boundaries or formatting rules.

The following assumptions have therefore been made.

1. Supported Range: `0.00` to `999,999,999.99`, value outside of this range will be rejected with a validation error.
2. Negative Values: not supported as it is not defined in the specification, but we can easily extend it in the future.
3. Decimal Precision: we only allow two decimal places

## Decision Made

* `POST` is used because the input is passed in the request body, keeping the URL clean and leaving the request contract extensible.
* The API accepts non-negative decimal amounts using `.` as the decimal separator, independent of the server regional settings. Group separators, signs, and scientific notation are rejected.
* The frontend contains minimal client-side business logic. Core conversion rules are covered by unit tests and the HTTP contract is covered by API integration tests. The user interface is verified through targeted browser-based functional and accessibility testing. `End-to-end automation may be added where the UI grows in complexity`.
* The user interface `does not automatically truncate or modify` values entered with more than two decimal places. Automatically modifying monetary input could cause the user to submit a different amount from the value they originally intended.
* `No database` has been introduced because the application does not require persistent state.

## Project Structure

```text
NumberToWordsWebPage/
│
├── .github/
│   └── workflows/
│       └── ci.yml
│
├── docs/
│   ├── DESIGN.md
│   └── TEST-PLAN.md
│
├── NumberToWordsWebPage/
│   ├── Controllers/
│   │   └── NumberToWordsController.cs
│   │
│   ├── Models/
│   │   ├── ErrorResponse.cs
│   │   ├── NumberToWordsRequest.cs
│   │   └── NumberToWordsResponse.cs
│   │
│   ├── Services/
│   │   ├── NumberToWordsException.cs
│   │   └── NumberToWordsConverter.cs
│   │
│   ├── wwwroot/
│   │   ├── index.html
│   │   ├── styles.css
│   │   └── app.js
│   │
│   ├── Program.cs
│   └── NumberToWordsWebPage.csproj
│
├── NumberToWordsWebPage.Tests/
│   ├── NumberToWordsConverterTests.cs
│   ├── NumberToWordsApiTests.cs
│   └── NumberToWordsWebPage.Tests.csproj
│
├── .dockerignore
├── .gitignore
├── Dockerfile
├── NumberToWordsWebPage.slnx
└── README.md
```

# Application Architecture

The application separates HTTP concerns from the number-conversion logic.

```text
Browser
   ↓
POST /api/number-to-words
   ↓
NumberToWordsController
   ↓
NumberToWordsConverter
   ↓
NumberToWordsResponse
   ↓
Browser
```

# Conversion Approach

The conversion algorithm is broken into progressively larger number ranges:

```text
0–19
↓
20-99
↓
100-999
↓
thousands
↓
millions
↓
dollars and cents
```

Numbers are processed in groups of up to three digits and combined with the appropriate scale.

For example:

```text
12,345,678
```

is decomposed into:

```text
12
→ TWELVE MILLION

345
→ THREE HUNDRED AND FORTY-FIVE THOUSAND

678
→ SIX HUNDRED AND SEVENTY-EIGHT
```

This approach keeps the individual conversion methods small and allows additional magnitude groups to be added without redesigning the lower-level number rules.

# Deployment

The application is containerised and deployed to Railway.

Railway was selected because it provides lightweight GitHub-integrated deployment with minimal infrastructure overhead.

The application is packaged as a standard ASP.NET Core Docker image, so it is not tightly coupled to Railway and could be migrated to a container-based hosting platform such as Azure if production requirements changed.
