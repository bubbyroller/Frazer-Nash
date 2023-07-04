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
        List<Receipt> SalesData = new List<Receipt>();
        int NumberOfSales = 0;
        double TotalRevenue = 0.0;
        double TotalProfit = 0.0;

        //Calls the Deli class which is where the main program code runs from
        Deli deli = new Deli();

        /* =================================== Constructor =================================== */
        public EPOS()
        {
            string CSVString = "";

            // Gets all of the sales data from the sales folder and stores it in the SalesData List
            SalesData = GetSalesData();

            // Displays the header text for the sales data
            DisplayHeader();
            CSVString += DisplayCSVHeader();

            // Displays the summary data for each receipt and adds to the main totals
            foreach (Receipt receipt in SalesData)
            {
                NumberOfSales += receipt.Sales;
                TotalRevenue += receipt.Revenue;
                TotalProfit += receipt.Profit;
                receipt.Display();
                CSVString += receipt.DisplayCSV();
                //TODO: Adds to CSV file
            }

            //Console.WriteLine(CSVString);
            string report = SalesData[0].TillId.ToString() + ".csv";
            Connection conn = new Connection("reports", report, 'w', CSVString);

            // Displays the summary of the total sales data
            DisplayTotals();

        }

        /* ================================= Secondary Methods ================================ */
        List<Receipt> GetSalesData()
        {
            List<Receipt> Receipts = new List<Receipt>();

            string path = Environment.CurrentDirectory + "\\resources\\sales";

            IEnumerable<string> SalesData = Directory.EnumerateFiles(path, "*.sales");

            foreach (string SalesFile in SalesData)
            {
                string FileName = SalesFile.Substring(path.Length + 1);
                
                Connection ReseiptConnection = new Connection("sales", FileName, 'r');
                string row = ReseiptConnection.getData();

                Receipts.Add(new Receipt(row, deli));

            }
            


            return Receipts;
        }

        /* ------------------------------------------------------------------------------------ */
        private void DisplayHeader()
        {
            string HeaderString = string.Format("{0}| {1}| {2}| {3}",
                "Date".PadRight(10),
                "Sales".PadRight(8),
                "Revenue".PadRight(8),
                "Profit".PadRight(8));
            Console.WriteLine("\nTask 2:");
            Console.WriteLine(HeaderString);
            Console.WriteLine("{0}", "".PadRight(38, '-'));

        }

        private string DisplayCSVHeader()
        {
            return "Date,Sales,Revenue,Profit\r\n";
        }

        /* ------------------------------------------------------------------------------------ */
        private void DisplayTotals()
        {
            string SalesString = NumberOfSales.ToString();
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
        /* ------------------------------------------------------------------------------------ */

        public string CreateCSVString()
        {


            return "";
        }
        /* ========================================= EOF ======================================== */
    }
}

