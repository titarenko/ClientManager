using System;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}