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
            try
            {
                TaskList = await server.getTasks(getUser());
                NotifyPropertyChanged("TaskList");
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

            try
            {
                PopupTask = await server.getTask(id);
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
            localSettingsHandler = new LocalSettingsHandler();
            String text = localSettingsHandler.getFromLoadSettings("userLogin");

            try
            {
                await server.putTask(id, title, value, createdAt, text);
            }
            catch (Exception)
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
                await server.deleteTask(selected);
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