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
    public static class Deli
    {
        /* ============================== Variables and Constants ============================ */
        public static Dictionary<char, double> IngredientCodeMap { get; set; } = new Dictionary<char, double>();
        public static List<Sandwich> SandwichList { get; set; } = new List<Sandwich>();
        public static List<Ingredient> IngredientList { get; set; } = new List<Ingredient>();

        const string FN_INGREDIENTS = "ingredients.csv";
        const string FN_SANDWICHES = "sandwiches.csv";

        /* =================================== Constructor =================================== */

        /// <summary>
        /// Constructor for the Deli class
        /// </summary>
        public static void Create()
        {
            try
            {

                // Connects to the ingredients file and pulls in the string list of ingredients
                Connection connIngredients = new Connection("stock", FN_INGREDIENTS, 'r');
                createIngredientList(connIngredients.getData());

                // Connects to the sandwiches file and pulls in the string list of sandwiches
                Connection connSandwiches = new Connection("stock", FN_SANDWICHES, 'r');
                createSadwichList(connSandwiches.getData());

                Display(SandwichList, IngredientCodeMap);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        /* ================================= Secondary Methods ================================ */

        /// <summary>
        /// Parses the text data passed in from the file, formatting and instantiating an 
        /// Ingredient object, which is added to the Ingredients list
        /// </summary>
        /// <param name="pIngredientsString"></param>
        private static void createIngredientList(string pIngredientsString)
        {
            // Split the data into rows
            string[] rows = pIngredientsString.Split('\n');

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
                    IngredientCodeMap.Add(kvp.Key, kvp.Value);
                    IngredientList.Add(item);
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
        private static void createSadwichList(string data)
        {
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
                Sandwich tSandwich = new Sandwich(tName, tCode);

                //Add the sndwich to a list
                SandwichList.Add(tSandwich);
            }
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Displays the header, calculates the required values and displays the handles the output
        /// </summary>
        /// <param name="pSandwiches"></param>
        /// <param name="pIngredients"></param>
        private static void Display(List<Sandwich> pSandwiches, Dictionary<char, double> pIngredients)
        {
            //Create headers with padding and spacing
            Console.WriteLine("Task 1:");
            Console.WriteLine(String.Format("{0}| {1}\t| {2}\t| {3}", "Item".PadRight(24), "Cost", "Price", "Profit"));
            Console.WriteLine(String.Format("{0}| {1}| {1}| {1}", "".PadRight(24, '-'), "".PadRight(6, '-')));

            //Calculate and display each sandwich
            foreach (Sandwich sandwich in pSandwiches)
            {
                sandwich.CalculateSandwichCost(pIngredients);
                sandwich.CalculateSellingPrice(sandwich.TotalCost);
                sandwich.CalculateProfit();
                sandwich.DisplaySandwichStats();
            }
        }

        /* ========================================= EOF ======================================== */
    }
}
