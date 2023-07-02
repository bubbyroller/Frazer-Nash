/**
* File: Connection.cs
* Author: Chris Goodings
* Date: 29/06/2023
* 
* Description: Class file that establishes a connection to a csv file in the resources folder
*               parses it into a string and allows for it to be returned back to the application.
*              
*/

namespace TestSandwich.Fundamentals
{
    public class Connection
    {
        /* ============================== Variables and Constants ============================ */
        private StreamReader reader;
        private StreamWriter writer;
        private string data;
        string path;

        /* =================================== Constructor =================================== */
        /// <summary>
        /// Constructor for the connection class
        /// </summary>
        /// <param name="pFileName"></param>
        /// <param name="pMode"></param>
        public Connection(string pFileName, char pMode)
        {
            try
            {
                //Gets the path to the file in the res folder
                path = Path.Combine(Environment.CurrentDirectory, @"res\" + pFileName);

                //Choose whether to read or write to a file based on the mode
                switch (pMode)
                {
                    case 'r':
                        reader = new StreamReader(path);
                        data = reader.ReadToEnd();
                        break;

                    case 'w':
                        // Write  to file ** CODE TO BE ADDED IF FILE WRITING NEEDED **
                        break;
                    default:
                        Console.WriteLine("Need to select a read or write option.");
                        break;
                }
            }
            catch (FileNotFoundException e)
            {
                //if the file cannot be found
                Console.WriteLine(e.ToString());
            }
            finally
            {
                //Housekeeping
                if (pMode == 'r')
                {
                    reader.Close();
                }

                if (pMode == 'w')
                {

                    writer.Close();
                }
            }
        }

        /* ================================= Secondary Methods ================================ */

        /// <summary>
        /// Returns the private data strind
        /// </summary>
        /// <returns>Data (string)</returns>
        public string getData()
        {
            return data;
        }

        /* ========================================= EOF ======================================== */
    }
}
