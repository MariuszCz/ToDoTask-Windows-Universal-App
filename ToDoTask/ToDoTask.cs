using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTask
{
    public class ToDoTask
    {
        public ToDoTask(string id, string title, string value, string ownerId, string createdAt)
        {
            this.Id = id;
            this.Title = title;
            this.Value = value;
            this.OwnerId = ownerId;
            this.CreatedAt = createdAt;
        }
        public ToDoTask() { }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string OwnerId { get; set; }
        public string CreatedAt { get; set; }


     /*   public override string ToString()
        {
            return Id + " by " + Title + ", Released: " + Value + " " + OwnerId + "" + CreatedAt;
        }*/
    }
}
