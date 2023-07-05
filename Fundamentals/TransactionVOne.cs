using System.Text.Json;

/**
* File: TransactionVOne.cs
* Author: Chris Goodings
* Date: 03/07/2023
* 
* Description: Class file that creates the TransactionVOne child - inherits the Transaction abstract class
*              
*/

namespace TestSandwich.Fundamentals
{
    public class TransactionVOne : Transaction
    {
        /* ============================== Variables and Constants =============================== */
        // Inherited

        /* =================================== Constructor ====================================== */
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pTransactionData"></param>
        /// <param name="pTillId"></param>
        public TransactionVOne(string pTransactionData, int pTillId)
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
        public new void GetIndividualPurchases(string pTransactionData)
        {

            string[] DataRow = pTransactionData.Split(' ');
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
                    TransactionTotal += double.Parse(LastRow[1].Remove(0, 1).Trim());
                    break;
                case "CRD":
                    TransactionTotal += double.Parse(LastRow[1].Remove(0, 1).Trim());
                    break;
                default:
                    break;
            }
        }

        /* ========================================= EOF ======================================== */
    }
}
