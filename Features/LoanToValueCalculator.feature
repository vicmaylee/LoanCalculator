Feature: LoanToValueCalculator

Loan Calculator Test

@SmokeTest
Scenario: Calculate a loan
	Given I navigate to "https://moneyfacts.co.uk/mortgages/loan-to-value-ltv-calculator/"
	And I enter I need to borrow test field
	And I enter Property value
	When I click on calculate
	Then Your loan to value displays correct value