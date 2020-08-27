using System;

namespace SteimkeBioladen.Models
{
    public class Item
    {
        public Item()
        {
            Id = "[no ean]";
            Text = "[no name]";
            Description = "[no desc]";
            Amount = "1";
        }
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public override string ToString()
        {
            return "EAN:" + Id + "\t" + "Menge:" + Amount + "\t" + "Name:" + Text + "\t" + Description;
        }
    }
}