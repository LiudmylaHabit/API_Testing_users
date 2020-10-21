Feature: UsersAPI
	In order to create tasks 
	As a user
	I want to register account on users system

Background:
	Given REST-client for request is created

@successRegistration
Scenario: Register an account with unique email
	Given Data for registretion is ready
	When I send POST registration request with prepared data
	Then Server status response is OK
	Then username from response is equel to username from request
	Then email from response is equel to email from request

@negativeRegistration
Scenario: Register an account with existing email
	Given Data with existing email for registration is ready
	When I send POST registration request with prepared data
	Then Server status response is OK
	Then Server response type is error
	Then Server response message is email already exist

@negativeRegistration
Scenario: Register an account with invalid email
	Given Data with <invalid> email for registretion is ready
	When I send POST registration request with prepared data
	Then Server status response is OK
	Then Server response type is error
	Then Server response message is tjat input email is invalid exist
	Examples:
		| invalid   |
		| "email"   |
		| "email@   |
		| "email."  |
		| "email@." |

@login
Scenario: Login into an account with valid credentials
	Given Data for login is ready
	When I send POST request with login data
	Then Server status response is OK
	Then Server response is true

@companyCreating
Scenario: Creating a company 
	Given Data for creating different type of <company> is ready
	When I send POST request with company prepared data
	Then Server status response is OK
	Then Server response type is success
	Then Server response with <company> detatils math with request data

	Examples:
		| company |
		| "ИП"    |
		| "ОАО"   |
		| "ООО"   |

@createTask @tasks
Scenario: Creating task for user
	Given Data for creating task for user is ready
	When I send POST request with prepared for creating task data
	Then Server status response is OK
	Then Server response type is success
	Then Server response message inform that task was created 

@tasks
Scenario: Deleting user task 
	Given User has task
	Given Data for deleting task user is ready
	When I send POST request with prepared for deleting task data
	Then Server status response is OK
	Then Server response type is success
	Then Server response message inform that task was deleted 

@search
Scenario: Search all information about one user by email
	Given Data of existing user for magic search is ready
	When I send POST request with prepared user data 
	Then Server status response is 231
	Then Server response include email of user that we was looking for
	Then Server response include name of user that we was looking for