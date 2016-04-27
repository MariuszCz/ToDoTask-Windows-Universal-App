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

        public ToDoTaskListViewModel()
        {
            getTasks();
        }
    }
}
