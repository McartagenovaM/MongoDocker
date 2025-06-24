// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

Console.WriteLine("🚀 Iniciando aplicación de consola .NET para MongoDB...\n");

try
{
    Console.WriteLine("🔌 Conectando a MongoDB...");
    //var client = new MongoClient("mongodb://miguel:supersecreto@192.168.0.4:27017/?authSource=admin");
    var client = new MongoClient("mongodb://miguel:supersecreto@192.168.0.4:27017/miapp?authSource=admin");


    Console.WriteLine("✅ Conexión establecida con éxito.");

    Console.WriteLine("📂 Accediendo a la base de datos 'miapp'...");
    var database = client.GetDatabase("miapp");

    Console.WriteLine("📄 Accediendo a la colección 'animals'...");
    var collection = database.GetCollection<BsonDocument>("animals");

    // Listas de animales y sentimientos
    var animalesGranja = new[] { "Chanchito", "Vaca", "Toro", "Caballo", "Oveja", "Cabra", "Gallina", "Gallo", "Pato", "Perro" };
    var estados = new[] { "Feliz", "Enojado", "Triste", "Cansado", "Loco" };

    var random = new Random();

    for (int i = 0; i < 3; i++)
    {
        var tipo = animalesGranja[random.Next(animalesGranja.Length)];
        var estado = estados[random.Next(estados.Length)];

        var nuevoAnimal = new BsonDocument
    {
        { "tipo", tipo },
        { "estado", estado }
    };

        await collection.InsertOneAsync(nuevoAnimal);
        Console.WriteLine($"✅ Animal insertado: {tipo} ({estado})");
    }


    Console.WriteLine("✅ Documento insertado correctamente.\n");

    Console.WriteLine("📋 Recuperando todos los documentos en la colección...\n");
    var animales = await collection.Find(new BsonDocument()).ToListAsync();

    Console.WriteLine($"🐾 Se encontraron {animales.Count} documentos:\n");
    foreach (var animal in animales)
    {
        Console.WriteLine(animal.ToJson(new MongoDB.Bson.IO.JsonWriterSettings { Indent = true }));
    }

    Console.WriteLine("\n✅ Operación finalizada con éxito.");
}
catch (Exception ex)
{
    Console.WriteLine("❌ Error durante la ejecución:");
    Console.WriteLine(ex.Message);
}
