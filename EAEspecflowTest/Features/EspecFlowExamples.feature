Feature: Specflow examples
    Scenario: Working with tables
        Given I input following numbers to the calculator
            | Number |
            | 123     |
            | 423     |
        When I add them the results should be
            | Result | Symbol |
            | 576    | +      |
        