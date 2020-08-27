using System;
using System.ComponentModel;
using System.Reflection;

namespace SteimkeBioladen.Models
{
    public enum TaxClass
    {
        [Description("unknown")]
        none,
        [Description("7%")]
        p7,
        [Description("19%")]
        p19
    }
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
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
        public TaxClass Tax { get; set; }
        public double GetSinglePrice()
        {
            double ret;
            bool worked = Double.TryParse(Price, out ret);
            if (!worked) throw new ArgumentException("cant parse price (" + Price + ")");
            return ret;
        }
        public double GetAmount()
        {
            double am;
            bool worked = Double.TryParse(Amount, out am);
            if (!worked) throw new ArgumentException("cant parse amount (" + Amount + ")");
            return am;
        }
        public double GetTotalPrice()
        {
            return GetSinglePrice() * GetAmount();
        }
        public override string ToString()
        {
            double ep, am;
            string retstr = "EAN:" + Id + "\tEinzelpreis:" + Price + "€\tSteuer:" + EnumHelper.GetDescription(Tax) + "\tMenge:" + Amount + "\tName:" + Text + "\t" + Description;
            if (Double.TryParse(Price, out ep) && Double.TryParse(Amount, out am))
            {
                retstr += "\tGesamtpreis:" + (ep * am).ToString() + "€";
            }
            return retstr;
        }

        
    }
}