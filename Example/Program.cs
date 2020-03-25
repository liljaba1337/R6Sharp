using Newtonsoft.Json;
using R6Sharp;
using System;
using System.IO;

namespace Example
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(@"..\..\..\..\credentials.json"));
            string email = json.email, password = json.password;
            var api = new R6Api(email, password);
            Console.WriteLine(api.GetTokenAsync().Result);
        }
    }
}
