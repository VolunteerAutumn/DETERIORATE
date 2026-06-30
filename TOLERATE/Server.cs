using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Server
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            int choice = 0;
            
            while (choice != 6) 
            {
                Console.WriteLine(" ==== MENU ==== ");
                Console.WriteLine("0. People\n1. Films\n2. Starships\n3. Vehicles\n4. Species\n5. Planets\n6. Exit");
                Console.Write("Please enter your choice (1-6) >>> ");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        await PrintPeople();
                        Console.Write("Press enter to come back...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 1:
                        await PrintFilms();
                        Console.Write("Press enter to come back...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 2:
                        await PrintStarships();
                        Console.Write("Press enter to come back...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 3:
                        await PrintVehicles();
                        Console.Write("Press enter to come back...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 4:
                        await PrintSpecies();
                        Console.Write("Press enter to come back...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 5:
                        await PrintPlanets();
                        Console.Write("Press enter to come back...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                    case 6:
                        break;
                    default:
                        break;
                }
            }
        }

        // ================= PEOPLE =================
        static async Task PrintPeople()
        {
            Console.WriteLine("\n=== PEOPLE ===");
            await PrintAllPages<Person>("https://swapi.py4e.com/api/people/");
        }

        // ================= FILMS =================
        static async Task PrintFilms()
        {
            Console.WriteLine("\n=== FILMS ===");
            await PrintAllPages<Film>("https://swapi.py4e.com/api/films/");
        }

        // ================= STARSHIPS =================
        static async Task PrintStarships()
        {
            Console.WriteLine("\n=== STARSHIPS ===");
            await PrintAllPages<Starship>("https://swapi.py4e.com/api/starships/");
        }

        // ================= VEHICLES =================
        static async Task PrintVehicles()
        {
            Console.WriteLine("\n=== VEHICLES ===");
            await PrintAllPages<Vehicle>("https://swapi.py4e.com/api/vehicles/");
        }

        // ================= SPECIES =================
        static async Task PrintSpecies()
        {
            Console.WriteLine("\n=== SPECIES ===");
            await PrintAllPages<Species>("https://swapi.py4e.com/api/species/");
        }

        // ================= PLANETS =================
        static async Task PrintPlanets()
        {
            Console.WriteLine("\n=== PLANETS ===");
            await PrintAllPages<Planet>("https://swapi.py4e.com/api/planets/");
        }

        // ================= CORE PAGINATION ENGINE =================
        static async Task PrintAllPages<T>(string url)
        {
            while (!string.IsNullOrEmpty(url))
            {
                var json = await client.GetStringAsync(url);

                var data = JsonSerializer.Deserialize<SwapiResponse<T>>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (data?.Results == null)
                    return;

                foreach (var item in data.Results)
                {
                    Console.WriteLine(GetName(item));
                }

                url = data.Next;
            }
        }

        // ================= GENERIC NAME EXTRACTOR =================
        static string GetName<T>(T item)
        {
            var type = typeof(T);

            var titleProp = type.GetProperty("Title");
            if (titleProp != null)
                return titleProp.GetValue(item)?.ToString();

            var nameProp = type.GetProperty("Name");
            if (nameProp != null)
                return nameProp.GetValue(item)?.ToString();

            return "unknown";
        }

        // ================= SWAPI WRAPPER =================
        public class SwapiResponse<T>
        {
            [JsonPropertyName("next")]
            public string Next { get; set; }

            [JsonPropertyName("results")]
            public T[] Results { get; set; }
        }

        // ================= MODELS =================
        public class Person { public string Name { get; set; } }
        public class Film { public string Title { get; set; } }
        public class Starship { public string Name { get; set; } }
        public class Vehicle { public string Name { get; set; } }
        public class Species { public string Name { get; set; } }
        public class Planet { public string Name { get; set; } }
    }
}
