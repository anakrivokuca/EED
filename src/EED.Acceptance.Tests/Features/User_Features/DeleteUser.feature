Feature: Delete user
	In order to remove old users
	As an administrator
	I want to be able to delete user

Scenario: Delete valid user
	Given I am on the Users page
	When I select the valid user:
	| Name | Surname | Email          | UserName | Password    |
	| John | Doe     | johndoe@ny.com | johndoe  | johndoe123! |
	And I try to delete the user
	Then I should see a success message "User John Doe has been successfully deleted."
	And the user "johndoe" should not be saved in the database
