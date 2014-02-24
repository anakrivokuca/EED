Feature: List All Users
	In order to preview users
	As an administrator
	I want to list all existing users

Scenario: Navigate to the Users page
	When an administrator browses to the Users page
	Then the Users page should be displayed

Scenario: List all users
	Given I am on the Users page
	Then all users from the database should be listed
