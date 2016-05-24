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
        private bool progressRing = false;
        private ToDoTask popupTask;
        private ObservableCollection<ToDoTask> taskList;
        private string baseURL = "http://windowsphoneuam.azurewebsites.net/api/ToDoTasks";
        private ServerConnector server = new ServerConnector();

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

        public bool ProgressRing
        { 
            get
            {
                return progressRing;
            }
            set
            {
                progressRing = value;
                NotifyPropertyChanged("ProgressRing");
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

            string url = baseURL + "?OwnerId=" + getUser();
            try
            {

                HttpResponseMessage response = await server.conn.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                TaskList = new ObservableCollection<ToDoTask>();
                TaskList = (JsonConvert.DeserializeObject<ObservableCollection<ToDoTask>>(responseBody));
            }
            catch (Exception)
            {
                Debug.Write("Brak internetu!");
                progressRing = true;
                NotifyPropertyChanged("ProgressRing");
            }
        }


        public async void getTask(string id)
        {

            string url = baseURL + "/" + id;
            try
            {
                HttpResponseMessage response = await server.conn.GetAsync("http://windowsphoneuam.azurewebsites.net/api/ToDoTasks/" + id);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                PopupTask = new ToDoTask();
                PopupTask = (JsonConvert.DeserializeObject<ToDoTask>(responseBody));
            }
            catch (Exception)
            {
                Debug.Write("Brak internetu!");
                progressRing = true;
                NotifyPropertyChanged("ProgressRing");
            }
        }

        public async void putTask(string id, string title, string value, string createdAt)
        {
            string url = baseURL + "/" + id;
            localSettingsHandler = new LocalSettingsHandler();
            ToDoTask putTask = new ToDoTask();
            String text = localSettingsHandler.getFromLoadSettings("userLogin");

            putTask.Id = id;
            putTask.Title = title;
            putTask.Value = value;
            putTask.OwnerId = text;
            putTask.CreatedAt = createdAt;

            string obj = JsonConvert.SerializeObject(putTask);
            try
            {
                HttpResponseMessage respon = await server.conn.PutAsync(url, new StringContent(obj, Encoding.UTF8, "application/json"));
                string responJsonText = await respon.Content.ReadAsStringAsync();
            } catch(Exception)
            {
                Debug.Write("Brak internetu!");
                progressRing = true;
                NotifyPropertyChanged("ProgressRing");
            }
           
        }

        public async void deleteTask(string selected)
        {
            try
            {
                HttpResponseMessage respon = await server.conn.DeleteAsync(baseURL + "/" + selected);
                string responJsonText = await respon.Content.ReadAsStringAsync();
                Debug.WriteLine("Wynik JSON'a:");
                Debug.WriteLine(responJsonText);
            }
            catch (Exception)
            {
                Debug.Write("Brak internetu!");
                progressRing = true;
                NotifyPropertyChanged("ProgressRing");
            }
        }
        public void Wait(int ms)
        {
            DateTime start = DateTime.Now;
            while ((DateTime.Now - start).TotalMilliseconds < ms) { }
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
