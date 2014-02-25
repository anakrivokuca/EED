Feature: Filter Users
	In order to preview specific users
	As an administrator
	I want to be able to filter users by specified criteria

Scenario Outline: Filter users with valid criteria
	Given I am on the Users page
	And the user with "janesmith" username exists
	When I enter criteria "<criteria>"
	Then the user "janesmith" should be listed on the screen

	Examples: 
	| criteria  |
	| jane      |
	| janesmith |
	| US        |
	| jane US   |

Scenario Outline: Filter users with invalid criteria
	Given I am on the Users page
	And the user with "johndoe" username does not exists
	When I enter criteria "<criteria>"
	Then no user should not be listed on the screen

	Examples: 
	| criteria |
	| john     |
	| johndoe  |
	| john US  |