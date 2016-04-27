using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.UI.Xaml;

namespace ToDoTask
{
    public class TaskManagement
    {
        private const string URL = "http://windowsphoneuam.azurewebsites.net/api/ToDoTasks";
        private LocalSettingsHandler localSettingsHandler;
        public List<ToDoTask> list;
        ToDoTask newTask = new ToDoTask();
        public async void addTask(String title, String value)
        {
              DateTimeOffset time = DateTimeOffset.Now;
              string formattedDate = time.ToString("dd-MM-yyyy HH:mm:ff");
              
            localSettingsHandler = new LocalSettingsHandler();

            String text = localSettingsHandler.getFromLoadSettings("userLogin");
     
            newTask.Id = "0";
            newTask.Title = title;
            newTask.Value = value;
            newTask.OwnerId = text;
            newTask.CreatedAt = formattedDate;

            string obj = JsonConvert.SerializeObject(newTask);

            var conn = new HttpClient();
            HttpResponseMessage respon = await conn.PostAsync(URL, new StringContent(obj, System.Text.Encoding.UTF8, "application/json"));
            string responJsonText = await respon.Content.ReadAsStringAsync();
            Debug.WriteLine("Wynik JSON'a:");
            Debug.WriteLine(responJsonText);
        }
        /*    public async void getTasks(string ownerId)
            {
                var conn = new HttpClient();
                conn.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await conn.GetAsync("http://windowsphoneuam.azurewebsites.net/api/ToDoTasks?ownerId=" + ownerId);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<ToDoTask>>(responseBody);
                Debug.Write(list[0].Title);
            }*/


    }
}
