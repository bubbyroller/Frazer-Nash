

using System.IO;
/**
* File: Deli.cs
* Author: Chris Goodings
* Date: 29/06/2023
* 
* Description: Class file for that brings together the ingredients and the sandwiches to 
*               calculate and display the relevant data.        
*              
*/

namespace TestSandwich.Fundamentals
{
    public class Deli
    {
        /* ============================== Variables and Constants ============================ */
        Dictionary<char, double> Output = new Dictionary<char, double>();
        public Dictionary<char, double> ingredients;
        public List<Sandwich> sandwiches = new List<Sandwich>();

        /* =================================== Constructor =================================== */

        /// <summary>
        /// Constructor for the Deli class
        /// </summary>
        public Deli()
        {

            
            const string FN_INGREDIENTS = "ingredients.csv";
            const string FN_SANDWICHES = "sandwiches.csv";

            try
            {

                // Connects to the ingredients file and pulls in the string list of ingredients
                
                
                Connection connIngredients = new Connection("stock", FN_INGREDIENTS, 'r');
                createIngredientList(connIngredients.getData());

                // Connects to the sandwiches file and pulls in the string list of sandwiches
                
                Connection connSandwiches = new Connection("stock", FN_SANDWICHES, 'r');
                sandwiches = createSadwichList(connSandwiches.getData());

                Display(sandwiches, Output);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        /* ================================= Secondary Methods ================================ */

        /// <summary>
        /// Displays the header, calculates the required values and displays the handles the output
        /// </summary>
        /// <param name="sandwiches"></param>
        /// <param name="ingredients"></param>
        private void Display(List<Sandwich> sandwiches, Dictionary<char, double> ingredients)
        {
            //Create headers with padding and spacing
            Console.WriteLine("Task 1:");
            Console.WriteLine(String.Format("{0}| {1}\t| {2}\t| {3}", "Item".PadRight(24), "Cost", "Price", "Profit"));
            Console.WriteLine(String.Format("{0}| {1}| {1}| {1}", "".PadRight(24,'-'), "".PadRight(6, '-')));
            
            //Calculate and display each sandwich
            foreach (Sandwich butty in sandwiches)
            {
                butty.CalculateSandwichCost(ingredients);
                butty.CalculateSellingPrice(butty.TotalCost);
                butty.CalculateProfit();
                butty.DisplaySandwichStats();
            }
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Parses the text data passed in from the file, formatting and instantiating an 
        /// Ingredient object, which is added to the Ingredients list
        /// </summary>
        /// <param name="data"></param>
        private void createIngredientList(string data)
        {
            // Split the data into rows
            string[] rows = data.Split('\n');

            //Traverse the rows
            foreach (string row in rows)
            {
                //Split each row as per CSV
                string[] cols = row.Split(',');

                //Trim any whitespace
                string tName = cols[0].Trim();
                string tCode = cols[1].Trim();
                string tUnit = cols[2].Trim();
                string tQuantity = cols[3].Trim();
                string tCost = cols[4].Trim();

                //This ignores the first rom of data which is a header
                if (tName != "Ingredient")
                {
                    //Instantiates a new Ingredient
                    Ingredient item = new Ingredient(tName, tCode, tUnit, tQuantity, tCost);

                    //KVP generated
                    KeyValuePair<char, double> kvp = item.GenerateCodeCostPair();

                    //KVP added to global dictionary
                    Output.Add(kvp.Key, kvp.Value);
                }
            }
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Creates a list of sandwiches based on data reteieved from a CSV file. Dat is formatted 
        /// and used to instantiate sandwich objects, which are stored in a list.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A list of sandwich items</returns>
        private List<Sandwich> createSadwichList(string data)
        {
            //Create a new sandwich list
            List<Sandwich> items = new List<Sandwich>();

            //split the data based on rows
            string[] rows = data.Split('\n');

            foreach (string row in rows)
            {
                //Split each row based on a CSV
                string[] cols = row.Split(',');

                //Trim whitespaces
                string tName = cols[0].Trim();
                string tCode = cols[1].Trim();

                //Create a sandwich object
                Sandwich butty = new Sandwich(tName, tCode);

                //Add the sndwich to a list
                items.Add(butty);
            }
            return items;
        }

        /* ========================================= EOF ======================================== */
    }
}
