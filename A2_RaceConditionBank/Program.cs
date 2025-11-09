using System;
using System.Threading;

namespace A2_RaceConditionBank;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Übung 2: Race Condition – Bankkonto");
        Console.WriteLine("==========================================\n");
        
        // Bankkonto mit Startwert 1000 EUR erstellen
        BankAccount account = new BankAccount(1000);
        Console.WriteLine($"Startkontostand: {account.GetBalance()} EUR\n");
        
        // 10 Threads erstellen
        Thread[] threads = new Thread[10];
        
        for (int i = 0; i < 10; i++)
        {
            threads[i] = new Thread(() => PerformBankOperations(account));
            threads[i].Start();
        }
        
        // Auf alle Threads warten
        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        
        // Endstand ausgeben
        Console.WriteLine("\n==========================================");
        Console.WriteLine($"Endkontostand: {account.GetBalance()} EUR");
        
        // Erwarteter Wert berechnen und ausgeben
        int expectedBalance = 1000 + (10 * 100) - (10 * 150); // 1000 + 1000 - 1500 = 500
        Console.WriteLine($"Erwarteter Kontostand: {expectedBalance} EUR");
        
        if (account.GetBalance() == expectedBalance)
        {
            Console.WriteLine("✓ Kontostand ist korrekt! Synchronisation funktioniert.");
        }
        else
        {
            Console.WriteLine("✗ Kontostand ist falsch! Race Condition aufgetreten.");
        }
    }
    
    private static void PerformBankOperations(BankAccount account)
    {
        // Jeder Thread zahlt 100 EUR ein
        account.Deposit(100);
        
        // Jeder Thread hebt 150 EUR ab
        account.Withdraw(150);
    }
}

