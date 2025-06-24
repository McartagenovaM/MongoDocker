using MongoDB.Bson;
using MongoDB.Driver;

/*Aplicación para testear manejo de MongoDB en Docker desde .NET Core*/

Console.WriteLine("🚀 Iniciando aplicación de consola .NET para MongoDB...\n");

try
{
    Console.WriteLine("🔌 Conectando a MongoDB...");
    var client = new MongoClient("mongodb://miguel:supersecreto@192.168.0.4:27017/miapp?authSource=admin");

    Console.WriteLine("✅ Conexión establecida con éxito.");

    var database = client.GetDatabase("miapp");
    var collection = database.GetCollection<BsonDocument>("animals");

    var animalesGranja = new[] { "Chanchito", "Vaca", "Toro", "Caballo", "Oveja", "Cabra", "Gallina", "Gallo", "Pato", "Perro" };
    var estados = new[] { "Feliz", "Enojado", "Triste", "Cansado", "Loco" };

    var random = new Random();

    Console.WriteLine("⌨️ Ingrese cualquier texto + ENTER para agregar un animal. Escriba 'exit' para salir.\n");

    bool modoInteractivo = Console.IsInputRedirected == false;

    if (modoInteractivo)
    {
        string? input;
        do
        {
            Console.Write("> ");
            input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrEmpty(input) && input.ToLower() != "exit")
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

        } while (input?.ToLower() != "exit");
    }
    else
    {
        Console.WriteLine("⚠️ Modo consola no interactiva detectado. Insertando 3 animales automáticamente.\n");

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
            Console.WriteLine($"✅ Animal insertado automáticamente: {tipo} ({estado})");
        }
    }

    Console.WriteLine("\n📋 Recuperando todos los documentos en la colección...\n");

    var animales = await collection.Find(new BsonDocument()).ToListAsync();
    Console.WriteLine($"🐾 Se encontraron {animales.Count} documentos:\n");

    foreach (var animal in animales)
    {
        Console.WriteLine(animal.ToJson(new MongoDB.Bson.IO.JsonWriterSettings { Indent = true }));
    }

    Console.WriteLine("\n✅ Aplicación finalizada.");
}
catch (Exception ex)
{
    Console.WriteLine("❌ Error durante la ejecución:");
    Console.WriteLine(ex.Message);
}
