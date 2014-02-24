Feature: Add User
	In order to allow user an access to the EED application
	As an administrator
	I want to be able to add user
	
Scenario: Navigate to the Add New User page
	When an administrator browses to the Add New User page
	Then the Add New User page should be displayed

@addNewUser
Scenario: Add new valid user
	Given I am on the Add New User page
	When I enter the valid user data
	And I try to save the user
	Then I should see a success message 
	And the user should be listed on the screen

Scenario: Add user with an existing email
	Given I am on the Add New User page
	When I enter the valid user data
	But the email "johndoe@ny.com" is already taken
	And I try to save the user
	Then I should see an error on the screen "User is not saved. DuplicateEmail"

Scenario Outline: Add user with invalid password
	Given I am on the Add New User page
	When I enter the valid user data
	And I enter the password "<password>"
	And I try to save the user
	Then I should see an error on the screen "User is not saved. InvalidPassword"

	Examples: 
	| password      |
	| shrt1@        |
	| weakpass@     |
	| noalphachars1 |
