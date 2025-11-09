using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace A3_ProducerConsumerQueue;

public class Producer
{
    private readonly int producerId;
    private readonly Random random;
    private volatile bool shouldStop = false;
    private Thread? producerThread;

    public Producer( int id)
    {
        this.producerId = id;
        this.random = new Random(id * 1000); // Verschiedene Seeds für verschiedene Producer
        
        // Thread im Konstruktor starten
        producerThread = new Thread(ProduceNumbers);
        producerThread.Start();
    }

    private void ProduceNumbers()
    {
        while (!shouldStop)
        {
            int number = random.Next(1, 101); // Zufällige Zahl zwischen 1 und 100
           
            Thread.Sleep(1000); // 1 Sekunde Takt
        }
    }

    public void Stop()
    {
        shouldStop = true;
    }
}
