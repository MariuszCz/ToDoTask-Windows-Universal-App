using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTask.ViewModel
{
    class ServerConnector
    {
        public HttpClient conn;
        private string baseURL = "http://windowsphoneuam.azurewebsites.net/api/ToDoTasks";
        public ServerConnector()
        {
            conn = new HttpClient();
            conn.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }



        public async Task<ObservableCollection<ToDoTask>> getTasks(string user)
        {

            string url = baseURL + "?OwnerId=" + user;
            try
            {

                HttpResponseMessage response = await conn.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return (JsonConvert.DeserializeObject<ObservableCollection<ToDoTask>>(responseBody));
            }
            catch (WebException)
            {
                Debug.Write("Brak internetu!");

            }
            return null;
        }

        public async Task<ToDoTask> getTask(string id)
        {

            string url = baseURL + "/" + id;
            try
            {
                HttpResponseMessage response = await conn.GetAsync(baseURL + "/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return (JsonConvert.DeserializeObject<ToDoTask>(responseBody));
            }
            catch (WebException)
            {
                Debug.Write("Brak internetu!");
            }
            return null;
        }

        public async Task putTask(string id, string title, string value, string createdAt, string user)
        {
            string url = baseURL + "/" + id;
            ToDoTask putTask = new ToDoTask();


            putTask.Id = id;
            putTask.Title = title;
            putTask.Value = value;
            putTask.OwnerId = user;
            putTask.CreatedAt = createdAt;

            string obj = JsonConvert.SerializeObject(putTask);
            try
            {
                HttpResponseMessage respon = await conn.PutAsync(url, new StringContent(obj, Encoding.UTF8, "application/json"));
                string responJsonText = await respon.Content.ReadAsStringAsync();
            }
            catch (WebException)
            {
                Debug.Write("Brak internetu!");
            }

        }

        public async Task deleteTask(string selected)
        {
            try
            {
                HttpResponseMessage respon = await conn.DeleteAsync(baseURL + "/" + selected);
                string responJsonText = await respon.Content.ReadAsStringAsync();
                Debug.WriteLine("Wynik JSON'a:");
                Debug.WriteLine(responJsonText);
            }
            catch (WebException)
            {
                Debug.Write("Brak internetu!");
            }
        }

        public async Task addTask(String title, String value, String text)
        {
            ToDoTask newTask = new ToDoTask();
            DateTimeOffset time = DateTimeOffset.Now;
            string formattedDate = time.ToString("dd-MM-yyyy HH:mm:ff");

            newTask.Id = "0";
            newTask.Title = title;
            newTask.Value = value;
            newTask.OwnerId = text;
            newTask.CreatedAt = formattedDate;

            string obj = JsonConvert.SerializeObject(newTask);
            try
            {
                HttpResponseMessage respon = await conn.PostAsync(baseURL, new StringContent(obj, System.Text.Encoding.UTF8, "application/json"));
                string responJsonText = await respon.Content.ReadAsStringAsync();
                Debug.WriteLine("Wynik JSON'a:");
                Debug.WriteLine(responJsonText);
            }
            catch (WebException)
            {
                Debug.Write("Brak internetu!");
            }
        }
    }
}
