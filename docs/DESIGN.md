## Assumptions

The specification provides an example of converting a monetary value into its English dollar and cent representation, but does not define all supported input boundaries or formatting rules.

The folloing assumptions have therefore been made.

1. Supported Range: `0.00` to `999,999,999,99`, value outside of this range will be rejected with a validation error.
2. Negative Values: not supported as it is not fined in the specs, but we can easily extend it in the future.
3. Decimal Precision: we only allow two decimal places

## Decision Made

* `POST` is used instead of GET because I prefer to pass input to request  body instead of exposing it to the url. This will make the url clean and prevents the normal UI workflow from being modified by editing the address bar. And using `POST`can also make request body extensible.
* The frontend contains minimal client-side business logic. Core conversion rules are covered by unit tests and the HTTP contract is covered by API integration tests. The user interface is verified through targeted browser-based functional and accessibility testing. End-to-end automation may be added where the UI grows in complexity.
