using System.Text.Json;
/**
* File: Transaction.cs
* Author: Chris Goodings
* Date: 03/07/2023
* 
* Description: Class file that creates an abstract class for the Transactions
*              
*/

namespace TestSandwich.Fundamentals
{
    public abstract class Transaction
    {
        /* ============================== Variables and Constants ============================ */
        public string TransactionNumber { get; set; } = string.Empty;
        public string TransactionTime { get; set; } = string.Empty;
        public int NumberOfItems { get; set; } = 0;

        public (string PurchaseId, int ProductCode, int PurchaseQuantity) Purchase;
        public string PaymentType { get; set; } = string.Empty;
        public double TransactionTotal { get; set; } = 0.0;
        public List<(string, int, int)> Purchases { get; set; } = new List<(string, int, int)>();
        public double TransactionProfit { get; set; } = 0.0;
        public double CardPaid { get; set; } = 0.0;


        /* =================================== Constructor =================================== */
        

        /* ================================= Secondary Methods ================================ */

        /// <summary>
        /// Common code that separates the transactions into individual rows
        /// </summary>
        /// <param name="pTransactionData"></param>
        /// <returns>String array of transaction data</returns>
        public string[] GetTrasactionDetails(string pTransactionData){
            return pTransactionData.Trim().Split("\r\n");
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Sets the header properties for the transaction
        /// </summary>
        /// <param name="pTransactionData"></param>
        public void GetTransactionHeader(string pTransactionData)
        {
            string[] FirstRow = pTransactionData.Split(' ');
            TransactionNumber = FirstRow[0].Trim();
            TransactionTime = FirstRow[1].Trim();
            NumberOfItems = int.Parse(FirstRow[2].Trim());
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Common code that gets the individual purchases
        /// </summary>
        /// <param name="pTransactionData"></param>
        public void GetIndividualPurchases(string pTransactionData) { }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Common code that gets the payment details from the transaction
        /// </summary>
        /// <param name="pTransactionData"></param>
        private void GetPaymentDetails(string pTransactionData) { }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Reads the config file to match the EPOS sandwich codes against the Deli sandwich map
        /// and then accumulates the profit for the transaction
        /// </summary>
        /// <param name="pTillID"></param>
        public void CalculateTransactionProfit(int pTillID) {

            // Checks that the JSON file is accessible
            if(File.ReadAllText($"./resources/config/{pTillID.ToString()}.json") is not null)
            {
                // Reads in the JSON config
                string text = File.ReadAllText($"./resources/config/{pTillID}.json");

                // Checks that the JSON file is parsable
                if (JsonSerializer.Deserialize<Config>(text) is not null )
                {
                    // Parse JSON file
                    Config config = JsonSerializer.Deserialize<Config>(text);

                    // Iterate the tuple list of purchaes
                    foreach ((string PurchaseId, int ProductCode, int PurchaseQuantity) purchase in Purchases)
                    {
                        // Gets the product code for the purchase
                        string sandwich = config.sandwiches[purchase.ProductCode.ToString()];

                        // Iterate the sandwich list
                        foreach (Sandwich butty in Deli.SandwichList)
                        {
                            // Compare the product code and sandwich code
                            if (butty.Name == sandwich)
                            {
                                // Accumulate the profit based on sandwich profit and number purchased
                                TransactionProfit += butty.Profit * purchase.PurchaseQuantity;
                            }
                        }
                    }
                }
            }
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Displays the transaction details to the console - not CSV
        /// </summary>
        public void Display()
        {
            string output = string.Format("Number: {0}, Time: {1}, Sales: {2}, Payment: {3}, Amount: {4}",
                TransactionNumber, TransactionTime, NumberOfItems, PaymentType, TransactionTotal.ToString());

            Console.WriteLine(output);
        }

        /* ========================================= EOF ======================================== */
    }
}


