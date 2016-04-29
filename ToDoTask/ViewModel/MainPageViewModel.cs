using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoTask.Commands;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ToDoTask.ViewModel
{
    public class MainPageViewModel : ViewModel
    {
        private const string URL = "http://windowsphoneuam.azurewebsites.net/api/ToDoTasks";
        private LocalSettingsHandler localSettingsHandler;

        private string user;
 
        public string User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                NotifyPropertyChanged();
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

        public async void addTask(String title, String value)
        {
            ToDoTask newTask = new ToDoTask();
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

        public MainPageViewModel()
        {   
            getUser();
        }
    }
}
