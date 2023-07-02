/**
* File: Sandwich.cs
* Author: Chris Goodings
* Date: 29/06/2023
* 
* Description: Class file for the Sandwich. It takes in the string values from the CSV file
*              and saves them as private properties. Public getters return the values of 
*              member variables.        
*              
*/
namespace TestSandwich.Fundamentals
{
    public class Sandwich
    {
        /* ============================== Variables and Constants ============================ */
        private const double OVERHEAD = 0.2;
        private const double LABOUR_COST_LOWER = 0.2;
        private const double LABOUR_COST_UPPER = 0.4;
        private const double GROSS_PROFIT = 0.6;

        public string Name { get; set; }
        public string Recipe { get; set; }
        public double TotalCost { get; set; }
        public double SellingPrice { get; set; }
        public double Profit { get; set; }

        /* =================================== Constructor =================================== */

        /// <summary>
        /// Constructor for the Sandwich class
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pRecipe"></param>
        public Sandwich(string pName, string pRecipe)
        {
            Name = pName;
            Recipe = pRecipe;
            //PrintTestString();
        }

        /* ================================= Secondary Methods ================================ */


        /// <summary>
        /// Iterates through the recipe codes and uses the dictionary to add the 
        /// prices to the base cost of a sandwich
        /// </summary>
        /// <returns>The Cost of the sandwich (double)</returns>
        public void CalculateSandwichCost(Dictionary<char, double> ingredients)
        {
            bool Valid = true;
            try
            {
                //If no recipe/ blank value return and set total cost to 0
                if (Recipe.Length == 0) 
                {
                    TotalCost = 0.0;
                    Valid = false;
                    return;
                }
                else    // Recipe present
                {
                    // Go through each item in the recipe as a char
                    foreach (char Item in Recipe.ToCharArray())
                    {
                        // If a valid key field and not a white space add key value to total
                        if (ingredients.ContainsKey(Item) && !Char.IsWhiteSpace(Item)){
                            TotalCost += ingredients[Item];
                        }else if (!Char.IsWhiteSpace(Item)) //If a non-valid char, white space ignored
                        {
                            TotalCost = 0.0;
                            Valid = false;
                            return;
                        } 
                    }
                }

                // If a valid char add additional costs
                if (Valid)
                {
                    TotalCost += OVERHEAD;
                    TotalCost += CalculateLabourCost();
                }
                else
                {
                    TotalCost = 0.0;
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                TotalCost = 0.0;
            }
            finally
            {
                TotalCost = Math.Round(TotalCost, 3);
                //PrintTestString();   <-- used to test member states throughout (breakpoint print out) 
                                       // Delete before production
            }
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Calcultes the Labour cost to be added to a sandwich based on the number
        /// of ingredients.
        /// </summary>
        /// <returns>
        /// ZERO if recipe length == 0
        /// LOWER_VALUE if <= 3 items
        /// UPPER_VALUE if > 3 items
        /// </returns>
        /// 

        /*** SET TO PUBLIC FOR TESTING - CHANGE TO PRIVATE BEFORE DEPLOYMENT ***/
        public double CalculateLabourCost()
        {
            try
            {
                //Initial length check to return 0 if no recipe listed
                if (Recipe.Length == 0) { return 0.0; }


                //Determine how many valid characters are present
                int Count = 0;
                foreach (char Item in Recipe.ToCharArray())
                {
                    if (Item != 'b')
                    {
                        Count++;
                    }
                }

                //If <= 3 valid chars return lower calue, else return upper
                if (Count <= 3) return LABOUR_COST_LOWER;

                return LABOUR_COST_UPPER;

            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                return 0.0;
            }

            //PrintTestString();   <-- used to test member states throughout (breakpoint print out) 
            // Delete before production
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Adds the GP to the initial cost. Then works out the remainder based on a 20p factor.
        /// Rounds down so that selling price is evenly divisibleby 20p
        /// </summary>
        /// <param name="InitialCost"></param>
        public void CalculateSellingPrice(double InitialCost)
        {
            // Add the GP
            double TempSellingPrice = InitialCost + (InitialCost * GROSS_PROFIT);

            // Get the modular remainder after the 20p division
            double ModSellPrice = TempSellingPrice % 0.2;

            // remove the remainder so that the final amount is divisible by 20p
            TempSellingPrice -= ModSellPrice;

            SellingPrice = Math.Round(TempSellingPrice, 2);
            //PrintTestString();   <-- used to test member states throughout (breakpoint print out) 
            // Delete before production

        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Simply works out the profit as selling price minus the costs
        /// </summary>
        public void CalculateProfit()
        {
            Profit = SellingPrice - TotalCost;
            //PrintTestString();   <-- used to test member states throughout (breakpoint print out) 
            // Delete before production
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Formats the decimal values of cost, selling price, and profit as currency strings. These
        /// are passed into the output string with stylistic formatting
        /// </summary>
        public void DisplaySandwichStats()
        {
            // Format decimals
            string StrTotalCost = String.Format("{0:C}", TotalCost);
            string StrSellingPrice = String.Format("{0:C}", SellingPrice);
            string StrProfit = String.Format("{0:C}", Profit);

            //format output string
            string Output = String.Format("{0}| {1}\t| {2}\t| {3}", Name.PadRight(24), StrTotalCost, StrSellingPrice, StrProfit);

            //This writes to console, but can be changed here to write to an external source if needed
            Console.WriteLine(Output);
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Simple test function that displays the values of the member variables when called. 
        /// Can be used as a breakpoint stack dump
        /// </summary>
        private void PrintTestString()
        {
            string TestStr = String.Format("Name: {0}, Recipe: {1}, Total Cost: {2}, Selling Price: {3}, Profit: {4}",
                Name, Recipe, TotalCost.ToString(), SellingPrice.ToString(), Profit.ToString());

            Console.WriteLine(TestStr);
        }

        /* ========================================= EOF ======================================== */
    }
}
