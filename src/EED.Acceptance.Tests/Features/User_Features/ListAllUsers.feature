Feature: List All Users
	In order to preview users
	As an administrator
	I want to list all existing users

Scenario: List all users
	Given I am on the Users page
	Then all users from the database should be listed
