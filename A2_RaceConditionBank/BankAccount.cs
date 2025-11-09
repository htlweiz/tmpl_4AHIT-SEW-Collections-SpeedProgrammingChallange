using System;
using System.Threading;

namespace A2_RaceConditionBank;
public class BankAccount
{
    private int balance;
   
    
    public BankAccount(int initial) 
    { 
        balance = initial; 
    }
    
    public void Deposit(int amount) 
    { 
       
    }
    
    public void Withdraw(int amount) 
    { 
        
    }
    
    public int GetBalance() 
    {
        return balance;
    }
}
