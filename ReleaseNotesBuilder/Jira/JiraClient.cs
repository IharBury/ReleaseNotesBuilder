using System;
using System.Collections.Generic;
using System.Linq;
using ReleaseNotesBuilder.Formatting;
using RestSharp;
using RestSharp.Authenticators;

namespace ReleaseNotesBuilder.Jira
{
    public class JiraClient : IJiraConfigurer, IJiraTaskNoteCollector
    {
        private readonly INoteFormatter noteFormatter;

        public JiraClient(INoteFormatter noteFormatter)
        {
            if (noteFormatter == null)
                throw new ArgumentNullException("noteFormatter");

            this.noteFormatter = noteFormatter;
        }


        public string UserName { get; set; }
        public string Password { get; set; }


        public void CollectTaskNotes(IEnumerable<string> taskReferences)
        {
            var notes = taskReferences.Select(taskReference => new Note
            {
                TaskName = taskReference,
                Summary = GetTask(taskReference).Summary
            });
            noteFormatter.Format(notes);
        }

        private TaskDataModel GetTask(string taskReference)
        {
            var client = new RestClient(new Uri("https://evisionindustysoftware.atlassian.net"))
            {
                Authenticator = new HttpBasicAuthenticator(UserName, Password)
            };
            var link = string.Format("rest/api/2/issue/{0}?fields=summary,customfield_11000", taskReference);
            var request = new RestRequest(link, Method.GET);
            var result = client.Execute<TaskDataModel>(request);
            return result.Data;
        }
    }
}