Feature: UsersAPI
	In order to create tasks 
	As a user
	I want to register account on users system

	Background: 
	Given REST-client  for request is created

@successRegistration
Scenario: Register an account with unique email 
	Given Data  for registretion is ready
	When I  send POST registration request with prepared data
	Then Server  status response is OK
	Then username  from response is equel to username from request
	Then email  from response is equel to email from request

@negativeRegistration
Scenario: Register an account with existing email 
	Given Data with existing email for registretion is ready
	When I send POST request with prepared data
	Then Server status response OK
	Then Server response type is error
	Then Server response message is email already exist

@negativeRegistration
Scenario: Register an account with invalid email 
	Given Data with invalid email for registretion is ready
	When I send POST request with prepared data
	Then Server status response OK
	Then Server response type is error
	Then Server response message is tjat input email is invalid exist



	@negativeRegistration
Scenario: Register an account with invalid email 
	Given Data with invalid email for registretion is ready
	When I send POST request with prepared data
	Then Server status response OK
	Then Server response type is error
	Then Server response message is tjat input email is invalid exist