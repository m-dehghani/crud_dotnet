Feature: CustomerPage

This feature will check the CRUD operations on customer Page

@tag1
Scenario: CustomerPageCrud
	Given The Customer page is loaded
			
	When Customer has created with the following information
	  | Email              | BankAccountNumber  | Firstname | Lastname | DateOfBirth | Phonenumber   |
      | john.doe@email.com | NL91RABO0312345678 | john      | doe      | 19-JUN-1999 | +989087645543 |
	
	Then The following customer is visible
	  | Email              | BankAccountNumber  | Firstname | Lastname | DateOfBirth | Phonenumber   |
      | john.doe@email.com | NL91RABO0312345678 | john      | doe      | 19-JUN-1999 | +989087645543 |
