Feature: Add User
	In order to allow user an access to the EED application
	As an administrator
	I want to be able to add user
	
Background: 
	Given I am on the Add New User page

Scenario: Add new valid user
	When I enter the valid user data:
	| Name | Surname | Email          | UserName | Password    |
	| John | Doe     | johndoe@ny.com | johndoe  | johndoe123! |
	And I try to save the user
	Then I should see a success message "User John Doe has been successfully saved."
	And the user "johndoe" should be saved in the database

Scenario: Add user with an existing email
	When I enter the valid user data:
	| Name | Surname | Email                  | UserName | Password      |
	| Jane | Smith   | janesmith@oklahoma.com | jane     | janesmith123! |
	But the email "janesmith@oklahoma.com" is already taken
	And I try to save the user
	Then I should see an error on the screen "User is not saved. DuplicateEmail"

Scenario Outline: Add user with invalid password
	When I enter the valid user data:
	| Name | Surname | Email          | UserName | Password   |
	| John | Doe     | johndoe@ny.com | johndoe  | <password> |
	But I enter the invalid password "<password>"
	And I try to save the user
	Then I should see an error on the screen "User is not saved. InvalidPassword"

	Examples: 
	| password      |
	| shrt1@        |
	| weakpass@     |
	| noalphachars1 |
