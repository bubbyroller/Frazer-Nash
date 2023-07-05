/**
* File: Config.cs
* Author: Chris Goodings
* Date: 02/07/2023
* 
* Description: Model class for the JSON config data. Implements the IConfig Interface        
*              
*/

namespace TestSandwich.Fundamentals
{
    public class Config: IConfig
    {
        public int id { get; set; } = 0;
        public string file_version { get; set; } = string.Empty;
        public Dictionary<string, string> sandwiches { get; set; }
    }
}
