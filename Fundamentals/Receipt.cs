/**
* File: Receipt.cs
* Author: Chris Goodings
* Date: 03/07/2023
* 
* Description: Class file that creates the receipt
*              
*/

namespace TestSandwich.Fundamentals
{
    public class Receipt
    {
        /* ============================== Variables and Constants ============================ */
        public int Sales { get; set; }
        public double Revenue { get; set; }
        public double Profit { get; set; }
        public string Version { get; set; } 
        public string ReceiptDate { get; set; }
        public int TillId { get; set; }
        List<Transaction> Transactions = new List<Transaction>();


        /* =================================== Constructor =================================== */
        public Receipt(string ReceiptData, Deli deli)
        {
            string[] strings = ReceiptData.Split("\r\n\r");
            for(int line = 0; line < strings.Length; line++)
            {
                if (line  == 0)
                {
                    string[] FirstRow = strings[line].Split("\r\n");
                    Version = FirstRow[0];

                    string[] ReceiptDetails = FirstRow[1].Split(" ");
                    ReceiptDate = ReceiptDetails[0].Trim();
                    TillId = int.Parse(ReceiptDetails[1].Trim());
                    
                }else if(strings[line] != "\n")
                {
                    Transactions.Add(new Transaction(strings[line], deli));
                }
            }

            SetData();

        }

        /* ================================= Secondary Methods ================================ */
        public void Display()
        {
            string output = string.Format("{0}| {1}| {2}| {3}",
                ReceiptDate.PadRight(10),
                Sales.ToString().PadRight(8),
                string.Format("{0:C}", Revenue).PadRight(8),
                string.Format("{0:C}", Profit).PadRight(8));
            Console.WriteLine(output);
        }

        public string DisplayCSV()
        {
            string output = string.Format("{0},{1},{2},{3}\r\n",
                ReceiptDate,
                Sales.ToString(),
                string.Format("{0:C}", Revenue),
                string.Format("{0:C}", Profit));

            return output;
        }

        /* ------------------------------------------------------------------------------------ */
        public void SetData()
        {
            foreach (Transaction transaction in Transactions)
            {
                Revenue += transaction.TransactionTotal;
                Sales += transaction.NumberOfSales;
                Profit += transaction.TransactionProfit;
            }
        }

        /* ------------------------------------------------------------------------------------ */
       


        /* ========================================= EOF ======================================== */
    }
}

