using JsonDemo.Data;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Car car = new Car
            //{
            //    Model = "Golf",
            //    Manufacturer = "VW",
            //    HorsePower = 100,
            //    CreatedOn = DateTime.Now
            //};

            //var optionsJson = new JsonSerializerOptions() {
            //    WriteIndented = true
            //};


            //var jsonSerializer =  JsonSerializer.Serialize(car, optionsJson );

            //File.WriteAllText("../../../fileJson.json", jsonSerializer);

            //Console.WriteLine(jsonSerializer);


            var db = new DbCarsContext();
            db.Database.EnsureCreated();

            string jsonString = File.ReadAllText("../../../fileJson.json");

            var jsonObject = JsonSerializer.Deserialize<Car>(jsonString);

            db.Add(jsonObject);
            db.SaveChanges();
            

        }
    }
}
