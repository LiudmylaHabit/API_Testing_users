using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace ApiPractice.Steps
{
    [Binding]
    public class UsersFeatureFileSteps
    {
        RestClient client;
        RestRequest request;
        Dictionary<string, string> userData;
        IRestResponse response;

        [Given(@"REST-client  for request is created")]
        public void GivenREST_ClientForRequestIsCreated()
        {
            client = new RestClient(" http://users.bugred.ru/");
        }

        [Given(@"Data  for registretion is ready")]
        public void GivenDataForRegistretionIsReady()
        {
            string now = DateTime.Now.ToString();
            now = now.Replace(":", "");
            now = now.Replace("/", "");
            now = now.Replace(" ", "");
            string email = "nums"+ now + "@test.com";
            string name = "user" + now;
            string pass = now;
            userData = new Dictionary<string, string>();
            userData.Add("name", name);
            userData.Add("email", email);
            userData.Add("password", pass);
        }

        [When(@"I  send POST registration request with prepared data")]
        public void WhenISendPOSTRegistrationRequestWithPreparedData()
        {
            request = new RestRequest("tasks/rest/doregister ", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(userData);
            response = client.Execute(request);
        }

        [Then(@"Server  status response is OK")]
        public void ThenServerStatusResponseIsOK()
        {
            Assert.AreEqual("OK", response.StatusCode.ToString());
        }

        [Then(@"username  from response is equel to username from request")]
        public void ThenUsernameFromResponseIsEquelToUsernameFromRequest()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(userData["name"], json["name"]?.ToString());
        }

        [Then(@"email  from response is equel to email from request")]
        public void ThenEmailFromResponseIsEquelToEmailFromRequest()
        {
            var temp = response.Content;
            JObject json = JObject.Parse(temp);
            Assert.AreEqual(userData["email"], json["email"]?.ToString());
        }

        // Scenario that chacking impossibility to create account with existing email

        [Given(@"Data with existing email for registretion is ready")]
        public void GivenDataWithExistingEmailForRegistretionIsReady()
        {            
            string email = "zx@z.x";
            string name = "user";
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

        [Then(@"Server status response OK")]
        public void ThenServerStatusResponseOK()
        {
            Assert.AreEqual("OK", response.StatusCode.ToString());
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
        [Given(@"Data with invalid email for registretion is ready")]
        public void GivenDataWithInvalidEmailForRegistretionIsReady()
        {
            string email = "invalid";
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

    }
}
