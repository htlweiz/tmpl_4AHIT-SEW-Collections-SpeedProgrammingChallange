using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace NamedPipeServerApp;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Named Pipe Server ===");
        Console.WriteLine("1. Start Server");
        Console.WriteLine("2. Exit");
        var choice = Console.ReadLine();
        if (choice == "1")
        {
            var server = new NamedPipeServer();
            server.StartServer();
        }
    }
}

class NamedPipeServer
{
    private const string PipeName = "PingPongPipe";
    private const int Rounds = 10;

    public void StartServer()
    {
        Console.WriteLine("Server started. Waiting for client...");
        Console.WriteLine($"Pipe Name: {PipeName}\n");

        try
        {
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(
                PipeName, 
                PipeDirection.InOut, 
                1))
            {
                // Warten auf Client-Verbindung
                pipeServer.WaitForConnection();
                Console.WriteLine("Client verbunden!\n");

                using (StreamReader reader = new StreamReader(pipeServer, Encoding.UTF8))
                using (StreamWriter writer = new StreamWriter(pipeServer, Encoding.UTF8) { AutoFlush = true })
                {
                    // Ping-Pong für 10 Runden
                    for (int i = 1; i <= Rounds; i++)
                    {
                        // Server sendet "Ping"
                        string message = $"Ping (Runde {i})";
                        writer.WriteLine(message);
                        Console.WriteLine($"[Server → Client] {message}");

                        // Server wartet auf "Pong" vom Client
                        string? response = reader.ReadLine();
                        if (response != null)
                        {
                            Console.WriteLine($"[Client → Server] {response}");
                        }

                        System.Threading.Thread.Sleep(500); // Kurze Pause zwischen Runden
                    }

                    // Abschlussnachricht
                    writer.WriteLine("DONE");
                    Console.WriteLine("\n✓ Alle Runden abgeschlossen!");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler: {ex.Message}");
        }

        Console.WriteLine("\nDrücken Sie eine Taste zum Beenden...");
        Console.ReadKey();
    }
}
