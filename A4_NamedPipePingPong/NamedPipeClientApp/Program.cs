using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace NamedPipeClientApp;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Named Pipe Client ===");
        Console.WriteLine("1. Start Client");
        Console.WriteLine("2. Exit");
        var choice = Console.ReadLine();
        if (choice == "1")
        {
            var client = new NamedPipeClient();
            client.StartClient();
        }
    }
}

class NamedPipeClient
{
    private const string PipeName = "PingPongPipe";

    public void StartClient()
    {
        Console.WriteLine("Client started. Connecting to server...");
        Console.WriteLine($"Pipe Name: {PipeName}\n");

        try
        {
            using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(
                ".", 
                PipeName, 
                PipeDirection.InOut))
            {
                // Verbindung zum Server herstellen
                pipeClient.Connect(5000); // 5 Sekunden Timeout
                Console.WriteLine("Verbunden mit Server!\n");

                using (StreamReader reader = new StreamReader(pipeClient, Encoding.UTF8))
                using (StreamWriter writer = new StreamWriter(pipeClient, Encoding.UTF8) { AutoFlush = true })
                {
                    // Ping-Pong Schleife
                    while (true)
                    {
                        // Client wartet auf "Ping" vom Server
                        string? message = reader.ReadLine();
                        
                        if (message == null || message == "DONE")
                        {
                            Console.WriteLine("\n✓ Alle Runden abgeschlossen!");
                            break;
                        }

                        Console.WriteLine($"[Server → Client] {message}");

                        // Client antwortet mit "Pong"
                        string roundInfo = message.Contains("Runde") 
                            ? message.Substring(message.IndexOf("Runde")) 
                            : "";
                        string response = $"Pong ({roundInfo})";
                        writer.WriteLine(response);
                        Console.WriteLine($"[Client → Server] {response}");
                    }
                }
            }
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Fehler: Verbindung zum Server konnte nicht hergestellt werden (Timeout).");
            Console.WriteLine("Stellen Sie sicher, dass der Server läuft!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: {ex.Message}");
        }

        Console.WriteLine("\nDrücken Sie eine Taste zum Beenden...");
        Console.ReadKey();
    }
}
