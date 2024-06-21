using System;

namespace App.Core.Entities
{
    public class Post : Entity<int>
    {

        public string Title { get; set; }

        public string Html { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}