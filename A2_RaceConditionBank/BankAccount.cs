using System;
using System.Threading;

namespace A2_RaceConditionBank;
public class BankAccount
{
    private int balance;
    private readonly object lockObject = new object();
    
    public BankAccount(int initial) 
    { 
        balance = initial; 
    }
    
    public void Deposit(int amount) 
    { 
        lock (lockObject)
        {
            int temp = balance;
            Thread.Sleep(1); // Simuliert Verzögerung für Race Condition
            balance = temp + amount;
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Einzahlung: {amount} EUR -> Neuer Stand: {balance} EUR");
        }
    }
    
    public void Withdraw(int amount) 
    { 
        lock (lockObject)
        {
            int temp = balance;
            Thread.Sleep(1); // Simuliert Verzögerung für Race Condition
            balance = temp - amount;
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Abhebung: {amount} EUR -> Neuer Stand: {balance} EUR");
        }
    }
    
    public int GetBalance() 
    { 
        lock (lockObject)
        {
            return balance; 
        }
    }
}
