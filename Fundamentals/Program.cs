/**
* File: Program.cs
* Author: Chris Goodings
* Date: 29/06/2023
* 
* Description: Main code that runs the program
*              
*/
namespace TestSandwich.Fundamentals
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Creates the Deli instance which brings together the Sandwiches and ingredients
            Deli.Create();

            //Calls the EPOS class which runs the till system
            EPOS ePOS = new EPOS();
        }
    }
}