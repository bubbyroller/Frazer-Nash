/**
* File: Receipt.cs
* Author: Chris Goodings
* Date: 29/06/2023
* 
* Description: Abstract class for the receipts - allows child classes to be added to a 
*               generic Receipt list as well as providing the structure contract for the
*               chhild classes. Common methods are declared here.
*              
*/

namespace TestSandwich.Fundamentals
{
    public abstract class Receipt
    {
        /* ============================== Properties ============================ */
        public string ReceiptDate { get; set; } = string.Empty;
        public int EPOSId { get; set; } = 0;
        public string EPOSVersion { get; set; } = string.Empty;
        public Dictionary<string, string> EPOSSandwichMap { get; set; } = new Dictionary<string, string>();
        public List<Transaction> ReceiptTransactions { get; set; } = new List<Transaction>();
        public int ReceiptSales { get; set; } = 0;
        public double ReceiptRevenue { get; set; } = 0.0;
        public double ReceiptProfit { get; set; } = 0.0;

        /* ============================== Methods ============================ */
        /// <summary>
        /// Base method that allows child to define how to get config data
        /// </summary>
        /// <param name="pEPOSId"></param>
        private void ConfigureEPOSSystem(int pEPOSId) { }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Common code to select the appropriate Transaction class for the legacy and updated 
        /// file data
        /// </summary>
        /// <param name="pTransactionData"></param>
        public void GetReceiptTransactions(string pTransactionData)
        {
            // Updated file data
            if(EPOSVersion == "v2.0")
            {
                ReceiptTransactions.Add(new TransactionVTwo(pTransactionData, EPOSId));
            }
            // Legacy data
            else
            {
                ReceiptTransactions.Add(new TransactionVOne(pTransactionData, EPOSId));
            }
        }

        /* ------------------------------------------------------------------------------------ */
        /// <summary>
        /// Common code that enummerates the transaction data to the receipt totals
        /// </summary>
        public void SetData()
        {
            foreach (Transaction transaction in ReceiptTransactions)
            {
                ReceiptRevenue += transaction.TransactionTotal;
                ReceiptSales += transaction.NumberOfItems;
                ReceiptProfit += transaction.TransactionProfit;
            }
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Common code that displays the formatted data to the CONSOLE - NOT CSV
        /// </summary>
        public void DisplayTotals()
        {
            string output = string.Format("{0}| {1}| {2}| {3}",
                ReceiptDate.PadRight(10),
                ReceiptSales.ToString().PadRight(8),
                string.Format("{0:C}", ReceiptRevenue).PadRight(8),
                string.Format("{0:C}", ReceiptProfit).PadRight(8));
            Console.WriteLine(output);
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Common code that generates the CSV format for the data
        /// </summary>
        /// <returns></returns>
        public string GenerateCSVData()
        {
            string output = string.Format("{0},{1},{2},{3}\r\n",
                ReceiptDate,
                ReceiptSales.ToString(),
                string.Format("{0:C}", ReceiptRevenue),
                string.Format("{0:C}", ReceiptProfit));

            return output;
        }

        /* ========================================= EOF ======================================== */
    }
}
