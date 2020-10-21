using Gherkin.Ast;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace ApiPractice.Steps
{
    [Binding]
    public class UsersFeatureFileSteps
    {
        RestClient client;
        RestRequest request;
        Dictionary<string, string> userData;
        Dictionary<string, object> companyDetails;
        Dictionary<string, string> userWithTask;
        string createdTaskId;
        IRestResponse response;

        [Given(@"REST-client for request is created")]
        public void GivenREST_ClientForRequestIsCreated()
        {
            client = new RestClient(" http://users.bugred.ru/");
        }

        [Given(@"Data for registretion is ready")]
        public void GivenDataForRegistretionIsReady()
        {
            string now = DateTime.Now.ToString();
            now = now.Replace(":", "");
            now = now.Replace("/", "");
            now = now.Replace(" ", "");
            string email = "nums" + now + "@test.com";
            string name = "user" + now;
            string pass = now;
            userData = new Dictionary<string, string>();
            userData.Add("name", name);
            userData.Add("email", email);
            userData.Add("password", pass);
        }

        [When(@"I send POST registration request with prepared data")]
        public void WhenISendPOSTRegistrationRequestWithPreparedData()
        {
            request = new RestRequest("tasks/rest/doregister ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userData);
            response = client.Execute(request);
        }

        [Then(@"Server status response is OK")]
        public void ThenServerStatusResponseIsOK()
        {
            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [Then(@"username from response is equel to username from request")]
        public void ThenUsernameFromResponseIsEquelToUsernameFromRequest()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(userData["name"], json["name"]?.ToString());
        }

        [Then(@"email from response is equel to email from request")]
        public void ThenEmailFromResponseIsEquelToEmailFromRequest()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(userData["email"], json["email"]?.ToString());
        }

        // Scenario that chacking impossibility to create account with existing email
        [Given(@"Data with existing email for registration is ready")]
        public void GivenDataWithExistingEmailForRegistrationIsReady()
        {
            string email = "zx@z.x";
            string name = "	user";
            string pass = "123qwe";
            userData = new Dictionary<string, string>();
            userData.Add("name", name);
            userData.Add("email", email);
            userData.Add("password", pass);
        }

        [When(@"I send POST request with prepared data")]
        public void WhenISendPOSTRequestWithPreparedData()
        {
            request = new RestRequest("tasks/rest/doregister ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userData);
            response = client.Execute(request);
        }
       
        [Then(@"Server response type is error")]
        public void ThenServerResponseTypeIsError()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("error", json["type"]?.ToString());
        }

        [Then(@"Server response message is email already exist")]
        public void ThenServerResponseMessageIsEmailAlreadyExist()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual($"email {userData["email"]} уже есть в базе",
                json["message"]?.ToString().Trim());
        }

        // Scenario that chacking impossibility to create account with invalid email
        [Given(@"Data with (.*) email for registretion is ready")]
        public void GivenDataWithEmailForRegistretionIsReady(string email)
        {          
            string name = "user";
            string pass = "123qwe";
            userData = new Dictionary<string, string>();
            userData.Add("name", name);
            userData.Add("email", email);
            userData.Add("password", pass);
        }

        [Then(@"Server response message is tjat input email is invalid exist")]
        public void ThenServerResponseMessageIsTjatInputEmailIsInvalidExist()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual($"Некоректный  email {userData["email"]}",
                json["message"]?.ToString().Trim());
        }

        // Cheking ability to login         
        [Given(@"Data for login is ready")]
        public void GivenDataForLoginIsReady()
        {
            userData = new Dictionary<string, string>();
            userData.Add("name", "BloodyMery");
            userData.Add("email", "mary@hellowin.usa");
            userData.Add("password", "123qwe");
        }

        [When(@"I send POST request with login data")]
        public void WhenISendPOSTRequestWithLoginData()
        {
            request = new RestRequest("tasks/rest/dologin ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userData);
            response = client.Execute(request);
        }

        [Then(@"Server response is true")]
        public void ThenServerResponseIsTrue()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual($"True", json["result"]?.ToString().Trim());
        }

        // Company creating 
        [Given(@"Data for creating different type of (.*) is ready")]
        public void GivenDataForCreatingDifferentTypeOfIsReady(string companyType)
        {
            companyDetails = new Dictionary<string, object>();
            string companyName = "Ferrero " + companyType;
            List<string> companyUsers = new List<string>
            {
                "first@test.ru"
            };
            companyDetails.Add("company_name", companyName);
            companyDetails.Add("company_type", companyType.Trim(new char[] { '"' }));
            companyDetails.Add("company_users", companyUsers);
            companyDetails.Add("email_owner", "mary@hellowin.usa");
        }

        [When(@"I send POST request with company prepared data")]
        public void WhenISendPOSTRequestWithCompanyPreparedData()
        {
            request = new RestRequest("tasks/rest/createcompany ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(companyDetails);
            response = client.Execute(request);
        }

        [Then(@"Server response type is success")]
        public void ThenServerResponseTypeIsSuccess()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual("success", json["type"]?.ToString());
        }

        [Then(@"Server response with (.*) detatils math with request data")]
        public void ThenServerResponseWithDetatilsMathWithRequestData(string companyType)
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            var companyList = json.Children().ToArray();
            string companyName = json["company"]["name"].ToString();
            Assert.AreEqual("Ferrero " + companyType, companyName);
        }
                
        // Creating task for user
        [Given(@"Data for creating task for user is ready")]
        public void GivenDataForCreatingTaskForUserIsReady()
        {
            userWithTask = new Dictionary<string, string>();
            string task = "Building ship models";
            string descriprion = "There is new request for your legendary ship model";
            userWithTask.Add("task_title", task);
            userWithTask.Add("task_description", descriprion);
            userWithTask.Add("email_owner", "mary@hellowin.usa");
            userWithTask.Add("email_assign", "mary@hellowin.usa");
        }

        [When(@"I send POST request with prepared for creating task data")]
        public void WhenISendPOSTRequestWithPreparedForCreatingTaskData()
        {
            request = new RestRequest("tasks/rest/createtask", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userWithTask);
            response = client.Execute(request);
        }

        [Then(@"Server response message inform that task was created")]
        public void ThenServerResponseMessageInformThatTaskWasCreated()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            createdTaskId = json["id_task"]?.ToString();
            Assert.AreEqual("Задача успешно создана!", json["message"]?.ToString());
        }                    

        // Deleting user task
        [Given(@"User has task")]
        public void GivenUserHasTask()
        {
            userWithTask = new Dictionary<string, string>()
            {
                { "task_title", "Building ship models" },
                { "task_description", "There is new request for your legendary ship model"},
                { "email_owner", "first@test.ru"},
                { "email_assign", "mary@hellowin.usa"}
            };
            request = new RestRequest("tasks/rest/createtask ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userWithTask);
            response = client.Execute(request);
            JObject json = JObject.Parse(response.Content);
            createdTaskId = json["id_task"]?.ToString();
        }

        [Given(@"Data for deleting task user is ready")]
        public void GivenDataForDeletingTaskUserIsReady()
        {
            userWithTask = new Dictionary<string, string>();
            userWithTask.Add("email_owner", "first@test.ru");
            userWithTask.Add("task_id", createdTaskId);
        }

        [When(@"I send POST request with prepared for deleting task data")]
        public void WhenISendPOSTRequestWithPreparedForDeletingTaskData()
        {
            request = new RestRequest("tasks/rest/deletetask ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userWithTask);
            response = client.Execute(request);
        }

        [Then(@"Server response message inform that task was deleted")]
        public void ThenServerResponseMessageInformThatTaskWasDeleted()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual($" Задача с ID {createdTaskId} успешно удалена", json["message"]?.ToString());
        }

        // MagicSearchByEmail
        [Given(@"Data of existing user for magic search is ready")]
        public void GivenDataOfExistingUserForMagicSearchIsReady()
        {
            userData = new Dictionary<string, string>()
            {
                {"query", "mary@hellowin.usa" }
            };            
        }

        [When(@"I send POST request with prepared user data")]
        public void WhenISendPOSTRequestWithPreparedUserData()
        {
            request = new RestRequest("tasks/rest/magicsearch", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userData);
            response = client.Execute(request);
        }

        [Then(@"Server status response is (.*)")]
        public void ThenServerStatusResponseIs(int code)
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string serverResponseCode = response.StatusCode.ToString();
            Assert.AreEqual($"{code}", serverResponseCode);
        }


        [Then(@"Server response include email of user that we was looking for")]
        public void ThenSwerverResponseIncludeEmailOfUserThatWeWasLookingFor()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string responceEmail = json["results"][0]["email"]?.ToString();
            Assert.AreEqual(userData["query"], responceEmail);
        }

        [Then(@"Server response include name of user that we was looking for")]
        public void ThenServerResponseIncludeNameOfUserThatWeWasLookingFor()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            string responseName = json["results"][0]["name"]?.ToString();
            Assert.AreEqual("bloodymery", responseName);
        }

        // Hooks 
        [AfterScenario("successRegistration")]
        public void DeleteCreatedUser()
        {
            Dictionary<string, string> userToDelete = new Dictionary<string, string>()
            {
                {"email", userData["email"] }
            };
            request = new RestRequest("tasks/rest/deleteuser ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userToDelete);
            response = client.Execute(request);
        }

        [AfterScenario("createTask")]
        public void DeleteUserTask()
        {
            Dictionary<string, string> taskToDelete = new Dictionary<string, string>()
                 {
                     { "email_owner", "mary@hellowin.usa" },
                     { "task_id", createdTaskId.Trim() }
                 };
            request = new RestRequest("tasks/rest/deletetask ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(taskToDelete);
            response = client.Execute(request);
            createdTaskId = "";
        }
    }
}