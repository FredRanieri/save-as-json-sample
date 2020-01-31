using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace json_sample {

    class ReportJSON {
        private static string GetHeader() {
            return "date;views;likes;dislikes";
        }
        private static List<string> GetContent() {
            return new List<string> {
                "2020-01-27;10;1;1",
                "2020-01-28;5;0;1",
                "2020-01-29;3;0;1",
                "2020-01-30;1;1;1",
            };
        }

        private static string[] SplitData(string data, string characters) {
            return data.Split(characters);
        }

        private static void PrintData(string[] headers, List<string[]> contents){
            foreach (var header in headers) {
                Console.Write($"   {header} |");
            }
            Console.Write("\n");
            foreach(var content in contents){
                foreach(var metric in content) {
                    Console.Write($"  {metric}\t|");
                }
                Console.Write("\n");
            }

        }

        private static JObject ConvertArrayToJSON(string tableName, string[] headers, List<string[]> contents) {
            JObject table = new JObject();
            List<JObject> json = new List<JObject>();
            List<JArray> jsonContent = new List<JArray>();
            
            for(int i = 0; i < headers.Length; i++) {
                jsonContent.Add(new JArray());
                for(int j = 0; j < contents[i].Length; j++){
                    jsonContent[i].Add(contents[j][i]); 
                } 
            }

            for(int i = 0; i < headers.Length; i++) {
                json.Add(new JObject());
                json[i][$"{headers[i]}"] = jsonContent[i];
            }

            table[tableName] = new JArray() { json };
            return table;
        }

        private static void SaveToFile(string schemaName, JObject json){
            using (StreamWriter file = File.CreateText($"{schemaName}.json")) {
                using (JsonTextWriter writer = new JsonTextWriter(file)) {
                    json.WriteTo(writer);
                }
            }
        }

        public static void ExportAsJSON() {
            var schemaName = "Youtube";
            var tableName = "Videos";

            // Split the data using ";" and save into an Array/List
            // headers = ["data", "views", "likes", "dislikes"]
            var headers = SplitData(GetHeader(), ";");
            List<string[]> contents =  new List<string[]>(); 
            foreach(var content in GetContent()) {
                contents.Add(SplitData(content, ";"));
            }

            // Uncomment the line below to see the result
            // PrintData(headers, contents);

            // Convert data into Json
            var json = ConvertArrayToJSON(tableName, headers, contents);
            
            // Save the json into a .json file
            Console.WriteLine(json.ToString());
            SaveToFile(schemaName, json);
        }

    }

    class Program {
        static void Main(string[] args) {
            ReportJSON.ExportAsJSON();
        }
    }
}
