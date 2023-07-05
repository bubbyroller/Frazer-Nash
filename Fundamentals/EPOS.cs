using System.Text.RegularExpressions;
/**
* File: EPOS.cs
* Author: Chris Goodings
* Date: 03/07/2023
* 
* Description: Class file that gets the sales data from the receipts
*              
*/

namespace TestSandwich.Fundamentals
{
    public class EPOS
    {
        /* ============================== Variables and Constants ============================ */
        List<Receipt> EPOSSalesData { get; set; } = new List<Receipt>();
        int TotalSales { get; set; } = 0;
        double TotalRevenue { get; set; } = 0.0;
        double TotalProfit { get; set; } = 0.0;

        string CSVReportHeader { get; set; } = string.Empty;
        string CSVLegacyReportBody { get; set; } = string.Empty;
        string CSVTillOneData { get; set; } = string.Empty;
        string CSVTillTwoData { get; set; } = string.Empty;

        /* =================================== Constructor =================================== */
        public EPOS()
        {
            // Gets all of the sales data from the sales folder and stores it in the SalesData List
            GetSalesData();

            // Displays the header text for the sales data
            DisplayHeader();
            SetCSVHeader();

            // Displays the summary data for each receipt and adds to the main totals
            SetCSVBody();

            //Console.WriteLine(CSVString);
            GenerateSalesReports();

            // Displays the summary of the total sales data
            DisplayTotals();
        }

        /* ================================= Secondary Methods ================================ */

        
        // Gets the sales receipts from the sales directory and saves the results to a Receipt list
        private void GetSalesData()
        {
            // Sets thdirectory path to the sales folder
            string path = Environment.CurrentDirectory + "\\resources\\sales";

            // Calls a private method to get the string representation of the file locations
            IEnumerable<string> SalesFiles = GetSalesFilesList(path);


            GetSalesReceiptList(SalesFiles, path);

        }

        /* ------------------------------------------------------------------------------------ */
        // Gets the string representation of the file locations
        private IEnumerable<string> GetSalesFilesList(string path)
        {
            return Directory.EnumerateFiles(path, "*");
        }

        /* ------------------------------------------------------------------------------------ */

        // Iterates through the files and calls a private method to add the file as a receipt
        private void GetSalesReceiptList(IEnumerable<string> pSalesFiles, string pPath)
        {

            foreach (string SalesFile in pSalesFiles)
            {
                string row = GetSalesFiles(SalesFile, pPath);

                AddNewReceipt(SalesFile, row);
            }
        }

        /* ------------------------------------------------------------------------------------ */

        // Isolates the file name and passes it into the connection string returning the string 
        // representation of the file data
        private string GetSalesFiles(string pSalesFile, string pPath)
        {
            string FileName = pSalesFile.Substring(pPath.Length + 1);

            Connection ReseiptConnection = new Connection("sales", FileName, 'r');

            return ReseiptConnection.getData();
        }

        /* ------------------------------------------------------------------------------------ */

        // Based on the file data, a new Receipt object is created and stored to a global list
        private void AddNewReceipt(string pSalesFile, string pDataRow)
        {
            // If the file format is one of the updated versions, instantiate the respective class
            if (IsUpdatedFile(pSalesFile))
            {
                EPOSSalesData.Add(new ReceiptVTwo(pDataRow));
            }
            else
            {
                EPOSSalesData.Add(new ReceiptVOne(pDataRow));
            }
        }

        /* ------------------------------------------------------------------------------------ */

        // Checks to see if the file is an updated format
        private bool IsUpdatedFile(string SalesFile)
        {
            // Matches [XXXXXX_day_YY.sales] => Left YY as \d+ not \d{2} as it allows for future growth  
            return Regex.IsMatch(SalesFile, @"\d{6}(_day_)\d+(.sales)");
        }

        /* ------------------------------------------------------------------------------------ */

        // Formats the header text for CONSOLE display - NOT CSV
        private void DisplayHeader()
        {
            string HeaderString = string.Format("{0}| {1}| {2}| {3}",
                "Date".PadRight(10),
                "Sales".PadRight(8),
                "Revenue".PadRight(8),
                "Profit".PadRight(8));
            Console.WriteLine("\nTasks 2 & 3:");
            Console.WriteLine(HeaderString);
            Console.WriteLine("{0}", "".PadRight(38, '-'));

        }

        /* ------------------------------------------------------------------------------------ */

        // Formats CSV header text
        private void SetCSVHeader()
        {
            CSVReportHeader = "Date,Sales,Revenue,Profit\r\n";
        }

        /* ------------------------------------------------------------------------------------ */

        // Iterates the receipts accumulating sales data before displaying console data and 
        // generating CSV data
        private void SetCSVBody()
        {
            foreach (Receipt receipt in EPOSSalesData)
            {
                // Set internal properties
                TotalSales += receipt.ReceiptSales;
                TotalRevenue += receipt.ReceiptRevenue;
                TotalProfit += receipt.ReceiptProfit;

                // Display Console data
                receipt.DisplayTotals();

                // From riginal dataset
                if (receipt.EPOSVersion == "v1.0" && IsLegacyFile(receipt.ReceiptDate))
                {
                    CSVLegacyReportBody += receipt.GenerateCSVData();
                }
                // From Till 2 of the new dataset
                else if (receipt.EPOSVersion == "v2.0")
                {
                    CSVTillTwoData += receipt.GenerateCSVData();

                }
                // From till1 of the new data set
                else
                {
                    CSVTillOneData += receipt.GenerateCSVData();
                }
            }
        }

        /* ------------------------------------------------------------------------------------ */
        // Compares file dates to check for original dataset
        private bool IsLegacyFile(string pDate)
        {
            var tSalesDate = DateTime.Parse(pDate);
            var tLegacyDate = DateTime.Parse("2021/05/10");
            if (tSalesDate <= tLegacyDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* ------------------------------------------------------------------------------------ */

        // Based on version number, till number and date the appropriate data is written to the CSV file
        private void GenerateSalesReports()
        {
            // Iterate the receipts
            foreach (Receipt entry in EPOSSalesData)
            {
                // Set the comparison criteria
                string tEPOSId = entry.EPOSId.ToString();
                string tEPOSVersion = entry.EPOSVersion;

                // Create a file name
                string FileName = tEPOSId + "_" + tEPOSVersion + ".csv";

                string CSVString = "";

                // If original dataset - set legacy data
                if (tEPOSVersion == "v1.0" && IsLegacyFile(entry.ReceiptDate))
                {
                    CSVString = CSVReportHeader + CSVLegacyReportBody;

                    // Create a custom name otherwise data for till 1 will write to the same file
                    FileName = tEPOSId + ".csv";
                }
                // If updated dataset for till 1 - set till 1  data
                else if ((tEPOSVersion == "v1.0") && (entry.EPOSId == 267534))
                {
                    CSVString = CSVReportHeader + CSVTillOneData;

                }
                // Set Till 2 updated data
                else
                {
                    CSVString = CSVReportHeader + CSVTillTwoData;
                }

                // Creates the connection that handles the CSV writer
                Connection conn = new Connection("reports", FileName, 'w', CSVString);
            }
        }

        /* ------------------------------------------------------------------------------------ */

        // Displays a formated totals summary for the CONSOLE - NOT CSV
        private void DisplayTotals()
        {
            string SalesString = TotalSales.ToString();
            string RevenueString = String.Format("{0:C}", TotalRevenue);
            string ProfitString = String.Format("{0:C}", TotalProfit);

            string FooterString = string.Format("{0}| {1}|\n{2}| {3}|\n{4}| {5}|",
                "| Total Sales:".PadRight(20), SalesString.PadRight(15),
                "| Total Revenue:".PadRight(20), RevenueString.PadRight(15),
                "| Total Profit:".PadRight(20), ProfitString.PadRight(15));

            Console.WriteLine("{0}", "".PadRight(38, '='));
            Console.WriteLine(FooterString);
            Console.WriteLine("{0}", "".PadRight(38, '='));
        }
        
        /* ========================================= EOF ======================================== */
    }
}

