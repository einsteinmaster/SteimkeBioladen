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
        public string Price { get; set; }
        public override string ToString()
        {
            double ep,am;
            string retstr = "EAN:" + Id + "\tEinzelpreis:" + Price + "€\tMenge:" + Amount + "\tName:" + Text + "\t" + Description;
            if (Double.TryParse(Price,out ep) && Double.TryParse(Amount,out am))
            {
                retstr += "\tGesamtpreis:" + (ep*am).ToString()+"€";
            }
            return retstr;
        }
    }
}