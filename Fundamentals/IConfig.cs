/**
* File: IConfig.cs
* Author: Chris Goodings
* Date: 29/06/2023
* 
* Description: Interface for the Config data model        
*              
*/

namespace TestSandwich.Fundamentals
{
    public interface IConfig
    {
        int id { get; set; }
        string file_version { get; set; }
        Dictionary<string, string> sandwiches { get; set; }
    }
}
