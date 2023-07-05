using System.Text.Json;

/**
* File: TransactionVTwo.cs
* Author: Chris Goodings
* Date: 03/07/2023
* 
* Description: Class file that creates the TransactionVTwo child - inherits the Transaction abstract class
*              
*/

namespace TestSandwich.Fundamentals
{
    public class TransactionVTwo : Transaction
    {


        /* ============================== Variables and Constants =============================== */
        public double TransactionChange { get; set; } = 0.0;

        public double CashPaid { get; set; } = 0.0;

        public string CardNumber { get; set; } = string.Empty;


        /* =================================== Constructor ====================================== */
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pTransactionData"></param>
        /// <param name="pTillId"></param>
        public TransactionVTwo(string pTransactionData, int pTillId)
        {
            //Break down The transaction data into individual rows            
            string[] IndividualTransactions = GetTrasactionDetails(pTransactionData);

            for (int i = 0; i < IndividualTransactions.Length; i++)
            {
                if (i == 0)
                {
                    //Process the header information from each transaction
                    GetTransactionHeader(IndividualTransactions[i]);
                }
                else if (i == IndividualTransactions.Length - 1)
                {
                    //Process and store the Payment details
                    GetPaymentDetails(IndividualTransactions[i]);
                }
                else
                {
                    //Process and store the individual purchaes
                    GetIndividualPurchases(IndividualTransactions[i]);
                }
            }
            //Calculate the Transaction Profit
            CalculateTransactionProfit(pTillId);
        }

        /* ================================= Secondary Methods ================================== */
        /// <summary>
        /// Inherited method that gets the formatted data for the individual purchases
        /// </summary>
        /// <param name="pTransactionData"></param>
        private new void GetIndividualPurchases(string pTransactionData)
        {

            string[] DataRow = pTransactionData.Split('|');
            Purchase.PurchaseId = DataRow[0].Trim();
            Purchase.ProductCode = int.Parse(DataRow[1].Trim());
            Purchase.PurchaseQuantity = int.Parse(DataRow[2].Trim());

            Purchases.Add(Purchase);
        }

        /* -------------------------------------------------------------------------------------- */
        /// <summary>
        /// Inherited method that gets the payment details from the transaction
        /// </summary>
        /// <param name="pTransactionData"></param>
        private void GetPaymentDetails(string pTransactionData)
        {
            // Separates the rows of the text file
            string[] LastRow = pTransactionData.Split(' ');

            // Gets payment type
            PaymentType = LastRow[0].Trim();

            // Accumulates the total based on the payment type
            switch (PaymentType)
            {
                case "CSH":
                    CashPaid += double.Parse(LastRow[1].Trim().Remove(0, 1));
                    TransactionTotal += double.Parse(LastRow[2].Remove(0, 1).Trim());
                    TransactionChange += double.Parse(LastRow[3].Remove(0, 1).Trim());
                    break;
                case "CRD":
                    CardNumber = LastRow[1].Trim();
                    CardPaid += double.Parse(LastRow[2].Trim().Remove(0, 1));
                    TransactionTotal += double.Parse(LastRow[2].Trim().Remove(0, 1));
                    break;
                default:
                    break;
            }
        }

        /* ========================================= EOF ======================================== */
    }
}
