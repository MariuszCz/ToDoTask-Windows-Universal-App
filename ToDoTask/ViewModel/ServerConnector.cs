using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTask.ViewModel
{
    class ServerConnector
    {
        public HttpClient conn;
        public ServerConnector()
        {
            conn = new HttpClient();
            conn.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
