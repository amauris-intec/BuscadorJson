using System.Text.Json;

const string SEPARADOR = "->";

// DE INTERES
static JsonElement BuscarElemento(JsonElement elementoInicial, string[] caminoAElemento)
{
    foreach (JsonProperty elemento in elementoInicial.EnumerateObject())
    {
        if (elemento.Name == caminoAElemento[0])
        {
            if (caminoAElemento.Length > 1)
                return BuscarElemento(elemento.Value, caminoAElemento[1..]);
            else
                return elemento.Value;
        }
    }

    throw new Exception($"No se encontró el elemento {caminoAElemento[0]}");
}

static JsonElement ObtenerPrimero(JsonElement elemento) => elemento[0];
static JsonElement ObtenerUltimo(JsonElement elemento) => elemento[-1];
static JsonElement ObtenerPosicion(JsonElement elemento, int indice) => elemento[indice];
/////////////

static JsonElement BuscarElementoConString(JsonElement elementoInicial, string caminoAElemento)
{
    return BuscarElemento(elementoInicial, caminoAElemento.Split(SEPARADOR, StringSplitOptions.TrimEntries));
}

static void LoopPrincipal(JsonElement root)
{
    bool salir = false;
    JsonElement elementoActual = root;

    do
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(elementoActual.GetRawText());
        Console.ResetColor();

        Console.WriteLine("Seleccione una acción para realizar\n" +
                          "1. Mostrar elemento actual\n" +
                          "2. Regresar al root\n" +
                          "3. Buscar child por nombre\n" +
                          "4. Buscar child por posición\n" +
                          "5. Primer child\n" +
                          "6. Último child\n" +
                          "ESCAPE para terminar\n");
        ConsoleKeyInfo key = Console.ReadKey(true);
        
        switch (key.Key)
        {
            case ConsoleKey.D1:
                break;
            case ConsoleKey.D2:
                elementoActual = root;
                
                break;
            case ConsoleKey.D3:
                Console.Write("Cuál elemento quiere? (use una flecha -> para buscar subelementos): ");
                string expresionBusqueda = Console.ReadLine() ?? "";
                elementoActual = BuscarElementoConString(elementoActual, expresionBusqueda);
                break;
            case ConsoleKey.D4:
                Console.Write("Cuál elemento quiere? (coloque una pocisión iniciando con 1): ");
                int indice = System.Convert.ToInt32(Console.ReadLine());
                elementoActual = ObtenerPosicion(elementoActual, indice - 1);
                break;
            case ConsoleKey.D5:
                elementoActual = ObtenerPrimero(elementoActual);
                break;
            case ConsoleKey.D6:
                elementoActual = ObtenerUltimo(elementoActual);
                break;
            case ConsoleKey.Escape:
                Console.WriteLine("Goodbye");
                return;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Opción inválida");
                Console.ResetColor();
                continue;

        }

    } while (true);
    
}

string json = @"{
    ""firstName"": ""John"",
    ""lastName"": ""doe"",
    ""age"": 26,
    ""address"": {
        ""streetAddress"": ""naist street"",
        ""city"": ""Nara"",
        ""postalCode"": ""630-0192""
    },
    ""phoneNumbers"": [
        {
            ""type"": ""iPhone"",
            ""number"": ""0123-4567-8888""
        },
        {
            ""type"": ""home"",
            ""number"": ""0123-4567-8910""
        }
    ]
}";

using (JsonDocument doc = JsonDocument.Parse(json))
{
    JsonElement root = doc.RootElement;
    LoopPrincipal(root);
}