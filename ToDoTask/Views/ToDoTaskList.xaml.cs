using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ToDoTask
{

    public sealed partial class ToDoTaskList : Page
    {
        private string selected;
        private LocalSettingsHandler localSettingsHandler;
        List<ToDoTask> list;
        ToDoTask newTask = new ToDoTask();
        //   private ObservableCollection<ToDoTask> list;
        //   public ToDoTask ObservableCollection { get; set; }
        public ToDoTaskList()
        {
            TaskManagement taskmanagement = new TaskManagement();
            this.InitializeComponent();
            localSettingsHandler = new LocalSettingsHandler();

            String text = localSettingsHandler.getFromLoadSettings("userLogin");
            if (text != null)
            {
                user.Text = text;
                getTasks(user.Text);
            }

            //    ToDoTask newTask = new ToDoTask();
            // Debug.Write(list[0].Title);
            //  listView.ItemsSource = taskmanagement.getList();
        }

        public async void getTasks(string ownerId)
        {
            var conn = new HttpClient();
            conn.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await conn.GetAsync("http://windowsphoneuam.azurewebsites.net/api/ToDoTasks?ownerId=" + ownerId);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            //list = JsonConvert.DeserializeObject<ObservableCollection<ToDoTask>>(responseBody);

            list = (JsonConvert.DeserializeObject<List<ToDoTask>>(responseBody));
            listView.DataContext = list;


        }
    

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToDoTask sel = (sender as ListView).SelectedItem as ToDoTask;

            if (sel != null)
            {
               // thisApp.Edytowalne = Zazn;
                selected = sel.Id;
                Debug.Write(selected);
            }
        }

        public async void deleteTask(object sender)
        {
            string apiUrl = "http://windowsphoneuam.azurewebsites.net/api/ToDoTasks";
    
            var objClint = new System.Net.Http.HttpClient();

            System.Net.Http.HttpResponseMessage respon = await objClint.DeleteAsync(apiUrl + "/" + selected);
            string responJsonText = await respon.Content.ReadAsStringAsync();
            Debug.WriteLine("Wynik JSON'a:");
            Debug.WriteLine(responJsonText);

        }

        private async void bar_delete(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex >= 0)
            {

                deleteTask(sender);

            }
            else
            {
                MessageDialog msgbox = new MessageDialog("Zaznacz element!");
                await msgbox.ShowAsync();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
