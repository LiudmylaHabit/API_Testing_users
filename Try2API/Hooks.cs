using TechTalk.SpecFlow;

namespace Try2API
{
    [Binding]
    public sealed class Hooks
    { /*
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
        }*/
    }
}
