/**
* File: ReceiptVTwo.cs
* Author: Chris Goodings
* Date: 03/07/2023
* 
* Description: Class file that creates the updated receipt format
*              
*/

namespace TestSandwich.Fundamentals
{
    public class ReceiptVTwo : Receipt
    {
        /* ============================== Variables and Constants ============================ */
        
        public string GUID { get; set; } = string.Empty;

        /* =================================== Constructor =================================== */

        /// <summary>
        /// Constructor for the ReceiptVTwo class
        /// </summary>
        /// <param name="pReceiptData"></param>
        public ReceiptVTwo(string pReceiptData)
        {
            // Separates the data onto individual lines
            string[] strings = pReceiptData.Split("\r\n\r");

            // Iterates the lines
            for (int line = 0; line < strings.Length; line++)
            {
                // First line of data
                if (line == 0)
                {
                    ConfigureEPOSSystem(strings[line]);
                }
                // Any line that isn't blank
                else if (strings[line] != "\n")
                {
                    GetReceiptTransactions(strings[line]);
                }
            }

            // Inherited method that sets the data to the properties
            SetData();
        }

        /* ================================= Secondary Methods ================================ */

        /// <summary>
        /// Sets the config data
        /// </summary>
        /// <param name="pEPOSConfigData"></param>
        private void ConfigureEPOSSystem(string pEPOSConfigData)
        {
            string[] FirstRow = pEPOSConfigData.Split("\r\n");
            EPOSVersion = FirstRow[0];

            string[] ReceiptDetails = FirstRow[1].Split(" ");
            ReceiptDate = ReceiptDetails[0].Trim();
            EPOSId = int.Parse(ReceiptDetails[1].Trim());

            if(EPOSVersion == "v2.0")
            {
                GUID = FirstRow[2];
            } 
        }

        /* ========================================= EOF ======================================== */
    }
}
