using SteimkeBioladen.Models;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SteimkeBioladen.ViewModels
{
    class NewItemViewModel
    {
        public Item ScannedItem { get; private set; }
        public NewItemViewModel() { }
        private Item GetItemFromRow(string row,TaxClass tax)
        {
            if (row == null) return null;
            var rowList = row.Split(';');
            var barcode = "?";
            var name = "?";
            var price = "?";
            if (rowList.Length > 1)
                barcode = rowList[1];
            if (rowList.Length > 3)
                name = rowList[3];
            if (rowList.Length > 11)
                price = rowList[10];
            Item it = new Item();
            it.Id = barcode;
            it.Price = price;
            it.Text = name;
            it.Tax = tax;
            return it;
        }
        public async Task<Item> TryFindItemAsync(string barcode)
        {
            var content = await Get7pFile();
            if (!string.IsNullOrWhiteSpace(content))
            {
                var row = GetRowFromCsv(content, barcode);
                if (row != null)
                {
                    return GetItemFromRow(row,TaxClass.p7);
                }
            }
            content = await Get19pFile();
            if (!string.IsNullOrWhiteSpace(content))
            {
                var row = GetRowFromCsv(content, barcode);
                if (row != null)
                {
                    var rowList = row.Split(';');
                    if (rowList[1] == barcode)
                    {
                        return GetItemFromRow(row,TaxClass.p19);
                    }
                }
            }
            return null;
        }
        public Task<string> Get7pFile()
        {
            string requri_7p = "http://rkp.intecelektro.de/steimkebioladen/Elkershausen%20Preisliste%20Excel%207%25%202.18.csv";
            return GetHttpFile(requri_7p);
        }
        public Task<string> Get19pFile()
        {
            string requri_19p = "http://rkp.intecelektro.de/steimkebioladen/Elkershausen%20Preisliste%20Excel%2019%25%202.18.csv";
            return GetHttpFile(requri_19p);
        }
        public async Task<string> GetHttpFile(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    byte[] barr = await response.Content.ReadAsByteArrayAsync();
                    return Encoding.UTF8.GetString(barr);
                }
                else
                {
                    Debug.WriteLine("No Successcode response");
                    Debug.WriteLine(response.ToString());
                    throw new FileNotFoundException(uri);
                }
            }
        }
        public string GetRowFromCsv(string file, string barcode)
        {
            var contentstring = file;
            var stringlist = contentstring.Split('\n');
            foreach (var it in stringlist)
            {
                var rowList = it.Split(';');
                if (rowList.Length > 1 && rowList[1] == barcode)
                {
                    var item = GetItemFromRow(it,TaxClass.none);
                    Debug.WriteLine("Entry found in DB: " + item.Text + "   price: " + item.Price);
                    return it;
                }
            }
            return null;
        }
        public async Task<bool> ProcessScanResult(string barcode)
        {
            ScannedItem = await TryFindItemAsync(barcode);
            return ScannedItem != null;
        }
    }
}