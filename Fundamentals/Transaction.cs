

using System.Text.Json;
using System.Transactions;
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
    public class Transaction
    {
        /* ============================== Variables and Constants ============================ */
        public string TransactionNumber { get; set; }
        public string TransactionTime { get; set; }
        public int NumberOfSales { get; set; }
        
        public (string PurchaseId, int ProductCode, int PurchaseQuantity) Purchase;
        public List<(string, int, int)> Purchases = new List<(string, int, int)> ();

        public string PaymentType { get; set; }
        public double TransactionTotal { get; set; }

        public double TransactionProfit { get; set; }

        Deli deli;


        /* =================================== Constructor =================================== */
        public Transaction(string TransactionData, Deli pDeli)
        {
            deli = pDeli;
            string[] IndividualTransactions = TransactionData.Trim().Split("\r\n\r");

            foreach(string line in IndividualTransactions)
            {
                string[] TransactionDetails = line.Split("\r\n");
                for (int row = 0; row < TransactionDetails.Length; row++)
                {

                    if (row == 0)
                    {
                        string[] FirstRow = TransactionDetails[row].Split(' ');
                        TransactionNumber = FirstRow[0].Trim();
                        TransactionTime = FirstRow[1].Trim();
                        NumberOfSales = int.Parse(FirstRow[2].Trim());

                    }
                    else if (row == TransactionDetails.Length - 1)
                    {
                        string[] LastRow = TransactionDetails[row].Split(' ');
                        PaymentType = LastRow[0].Trim();
                        TransactionTotal += double.Parse(LastRow[1].Remove(0, 1).Trim());
                    }
                    else
                    {
                        string[] DataRow = TransactionDetails[row].Split(' ');
                        Purchase.PurchaseId = DataRow[0].Trim();
                        Purchase.ProductCode = int.Parse(DataRow[1].Trim());
                        Purchase.PurchaseQuantity = int.Parse(DataRow[2].Trim());

                        Purchases.Add(Purchase);
                    } 
                }
            }

            CalculateProfit();
        }

        /* ================================= Secondary Methods ================================ */
        public void Display()
        {
            string output = string.Format("Number: {0}, Time: {1}, Sales: {2}, Payment: {3}, Amount: {4}",
                TransactionNumber, TransactionTime, NumberOfSales, PaymentType, TransactionTotal.ToString());

            Console.WriteLine(output);
        }

        /* ------------------------------------------------------------------------------------ */
        public void CalculateProfit()
        {
            string text = File.ReadAllText(@"./resources/config/config.json");
            Config config = JsonSerializer.Deserialize<Config>(text);

            double profit = 0.0;

            foreach((string PurchaseId, int ProductCode, int PurchaseQuantity) purchase in Purchases)
            {
               
                string sandwich = config.sandwiches[purchase.ProductCode.ToString()];
                foreach(Sandwich butty in deli.sandwiches)
                {
                    if(butty.Name == sandwich)
                    {
                        profit += butty.Profit * purchase.PurchaseQuantity;
                    }
                }
            }

            

            TransactionProfit = profit;
        }

        /* ------------------------------------------------------------------------------------ */



        /* ========================================= EOF ======================================== */
    }
}


