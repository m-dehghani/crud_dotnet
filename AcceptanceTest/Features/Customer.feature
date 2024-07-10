Feature: Customer Manager;

As a an operator I wish to be able to Create, Update, Delete customers and list all customers
	
@mytag
Scenario: User can Create, Edit, Delete, and Read customer records

	Given platform support following error codes
      | Code | Description                    |
      | 101  | Invalid Email                  |
      | 102  | Invalid Phonenumber            |
      | 103  | Invalid BankAccountNumber      |
      | 201  | Duplicated Email Address       |
      | 202  | Duplicated Firstname, Lastname |


	Given platform has "0" record of customers
	
	When When user send command to create new customer with following information 
		| Email              | BankAccountNumber  | Firstname | Lastname | DateOfBirth | Phonenumber   |
        | john.doe@email.com | NL91RABO0312345678 | john      | doe      | 19-JUN-1999 | +989087645543 |
	
	Then user can send query and receive "1" record of customer with following data
		| Email              | BankAccountNumber  | Firstname | Lastname | DateOfBirth | Phonenumber   |
        | john.doe@email.com | NL91RABO0312345678 | john      | doe      | 19-JUN-1999 | +989087645543 |

	When user send command to update customer with email of "john.doe@email.com" and with below information
		| Email                | BankAccountNumber  | Firstname | Lastname   | DateOfBirth | Phonenumber   |
        | john.smith@email.com | NL91RABO0312345679 | john      | smith      | 19-JUN-1999 | +989087645541 |

	 Then user should receive following error codes
        | Code |
        | 202  |
        | 102  |

	Then user can send query and receive "1" record of customer with following data
        | Email                | BankAccountNumber  | Firstname | Lastname   | DateOfBirth | Phonenumber   |
        | john.smith@email.com | NL91RABO0312345679 | john      | smith      | 19-JUN-1999 | +989087645541 |

	 And user can send query and receive "0" record of customer with following data
        | Email              | BankAccountNumber  | Firstname | Lastname | DateOfBirth | Phonenumber   |
        | john.doe@email.com | NL91RABO0312345678 | john      | doe      | 19-JUN-1999 | +989087645543 |