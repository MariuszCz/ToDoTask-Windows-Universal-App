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
using ToDoTask.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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

        public ToDoTaskList()
        {
            
            this.InitializeComponent();
            DataContext = new ToDoTaskListViewModel();
        }
        private ToDoTaskListViewModel GetViewModel()
        {
            return DataContext as ToDoTaskListViewModel;
        }



        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToDoTask sel = (sender as ListView).SelectedItem as ToDoTask;

            if (sel != null)
            {
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
                listView.Background = new SolidColorBrush(Colors.Red);
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

        private void UIElement_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {

            e.Handled = true;
            ListViewItem item = sender as ListViewItem;
            if (item == null) return;
            if (e.Cumulative.Translation.X < 0)
            {
                 deleteTask(sender);
       
            }
        }

        private void ShowFlyoutPopup(object sender, RoutedEventArgs e)
        {
            if (!logincontrol1.IsOpen)
            {
                RootPopupBorder.Width = 646;
          //      logincontrol1.HorizontalOffset = Window.Current.Bounds.Width - 550;
          //      logincontrol1.VerticalOffset = Window.Current.Bounds.Height - 1000;
                logincontrol1.IsOpen = true;
            }
        }
    }
}
