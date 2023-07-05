/**
* File: Ingredient.cs
* Author: Chris Goodings
* Date: 29/06/2023
* 
* Description: Class file for the Ingredient. It takes in the string values from the CSV file
*              and saves them as private properties. Public getters return the values of 
*              member variables. A secondary method converts the string parameters of the 
*              constructor, removing any currency signatures, converting to decimal values
*              and storing as a double to 2dp.       
*              
*/
namespace TestSandwich.Fundamentals
{
    public class Ingredient
    {
        /* ============================== Variables and Constants ============================ */
        public string Name { get; set; }
        public char Code { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public double UnitCost { get; set; }

        /* =================================== Constructor =================================== */

        /// <summary>
        /// Constructor for the ingredient.
        /// </summary>
        /// <param name="pName"></param>
        /// <param name="pCode"></param>
        /// <param name="pUnit"></param>
        /// <param name="pQuantity"></param>
        /// <param name="pUnitCost"></param>
        public Ingredient(string pName, string pCode, string pUnit, string pQuantity, string pUnitCost)
        {
            Name = pName;
            Code = char.Parse(pCode);
            Unit = pUnit;
            Quantity = int.Parse(pQuantity);
            UnitCost = ConvertUnitCost(pUnitCost);
        }

        /* ================================= Secondary Methods ================================ */

        /// <summary>
        /// Strips the leading £ and trailing p from Unit cost. If in pence, then convert to £ to 3dp.
        /// </summary>
        /// <returns>The cost per unit (double)</returns>
        private double ConvertUnitCost(string UnitCost)
        {
            // Strip the special chars and parse to double
            double Output = 0f;

            // Removes £ from text string
            if (UnitCost.StartsWith('£'))
            {
                Output = double.Parse(UnitCost.Remove(0, 1));
            }

            // The pence value needs dividing by 100 to convert to pounds
            if (UnitCost.EndsWith('p'))
            {
                Output = double.Parse(UnitCost.Remove(UnitCost.Length - 1, 1)) / 100;
            }

            // return the cost
            return Math.Round(Output / Quantity, 3);
        }

        /* ------------------------------------------------------------------------------------ */

        /// <summary>
        /// Generates a key value pair for the code and cost that allows quick retrieval of
        /// ingredient cost per unit.
        /// </summary>
        /// <returns>Key Value Pair (char, double)</returns>
        public KeyValuePair<char, double> GenerateCodeCostPair()
        {
            return new KeyValuePair<char, double>(Code, UnitCost);
        }

        /* ========================================= EOF ======================================== */
    }
}
