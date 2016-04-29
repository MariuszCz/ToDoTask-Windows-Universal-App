using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ToDoTask.Commands;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ToDoTask.ViewModel
{
    class ToDoTaskListViewModel : ViewModel
    {
        private LocalSettingsHandler localSettingsHandler;
        private string user;
        private ToDoTask popupTask;
        private ObservableCollection<ToDoTask> taskList;

        public ObservableCollection<ToDoTask> TaskList
        {
            get { return taskList; }
            set
            {
                taskList = value;
                NotifyPropertyChanged("TaskList");
            }
        }

        public string User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                NotifyPropertyChanged("User");
            }
        }
        public ToDoTask PopupTask
        {
            get
            {
                return popupTask;
            }
            set
            {
                popupTask = value;
                NotifyPropertyChanged("PopupTask");
            }
        }


        public string getUser()
        {
            localSettingsHandler = new LocalSettingsHandler();

            String text = localSettingsHandler.getFromLoadSettings("userLogin");
            if (text != null)
            {
                user = text;
                return user;
            }
            return null;
        }


        public async void getTasks()
        {
            var conn = new HttpClient();
            conn.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await conn.GetAsync("http://windowsphoneuam.azurewebsites.net/api/ToDoTasks?OwnerId=" + getUser());
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            TaskList = new ObservableCollection<ToDoTask>();
            TaskList = (JsonConvert.DeserializeObject<ObservableCollection<ToDoTask>>(responseBody));
        }

        public async void getTask(string id)
        {
            var conn = new HttpClient();
            conn.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await conn.GetAsync("http://windowsphoneuam.azurewebsites.net/api/ToDoTasks/" + id);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            PopupTask = new ToDoTask();
            PopupTask = (JsonConvert.DeserializeObject<ToDoTask>(responseBody));
        }

        public async void putTask(string id, string title, string value, string createdAt)
        {

            localSettingsHandler = new LocalSettingsHandler();
            ToDoTask putTask = new ToDoTask();
            String text = localSettingsHandler.getFromLoadSettings("userLogin");


            putTask.Id = id;
            putTask.Title = title;
            putTask.Value = value;
            putTask.OwnerId = text;
            putTask.CreatedAt = createdAt;

            string obj = JsonConvert.SerializeObject(putTask);
        
            var conn = new HttpClient();
            HttpResponseMessage respon = await conn.PutAsync("http://windowsphoneuam.azurewebsites.net/api/ToDoTasks/" + id, new StringContent(obj, Encoding.UTF8, "application/json"));
            string responJsonText = await respon.Content.ReadAsStringAsync();
        }

        public async void deleteTask(string selected)
        {
            string apiUrl = "http://windowsphoneuam.azurewebsites.net/api/ToDoTasks";

            var objClint = new System.Net.Http.HttpClient();

            System.Net.Http.HttpResponseMessage respon = await objClint.DeleteAsync(apiUrl + "/" + selected);
            string responJsonText = await respon.Content.ReadAsStringAsync();
            Debug.WriteLine("Wynik JSON'a:");
            Debug.WriteLine(responJsonText);

        }

        public ToDoTaskListViewModel()
        {
            getTasks();  
        }
        public ToDoTaskListViewModel(string id)
        {
            getTask(id);
        }
    }
}
