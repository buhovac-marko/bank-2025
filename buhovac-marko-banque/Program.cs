using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;

Console.WriteLine("--- DÉMARRAGE DE LA SIMULATION BANCAIRE ---");

// Création des Personnes
Person client1 = new Person("Marko", "Buhovac", new DateTime(1867, 11, 7));
Person client2 = new Person("Max", "Verstappen", new DateTime(1879, 3, 14));

// Création des Comptes
CurrentAccount account1 = new CurrentAccount("FR12345", 500.00, 1000.00, client1);
CurrentAccount account2 = new CurrentAccount("FR67890", 2500.50, 500.00, client1);

SavingsAccount savings1 = new SavingsAccount("EP00123", 15000.00, client1);
SavingsAccount savings2 = new SavingsAccount("EP00456", 50.00, client2);

CurrentAccount account3 = new CurrentAccount("FR99999", 500.00, client1); 

// Création de la Banque
Bank bnp = new Bank("BNP Paribas");
bnp.AddAccount(account1);
bnp.AddAccount(account2);
bnp.AddAccount(savings1);
bnp.AddAccount(savings2);
bnp.AddAccount(account3);

Console.WriteLine($"\nLa banque '{bnp.Name}' gère un total de {bnp.Accounts.Count} comptes.");
Console.WriteLine($"Novi račun {account3.Number} starta sa solde: {account3.GetBalance():C2}");

Console.WriteLine("\n--- OPÉRATIONS BANCAIRES ---");

Console.WriteLine($"Solde initial Courant {account1.Number}: {account1.GetBalance():C2}");
account1.Deposit(100.00); 
account1.Withdraw(300.00); 

Console.WriteLine($"Appliquer Intérêts sur {savings1.Number}...");
savings1.ApplyInterest(); 
Console.WriteLine($"Novi solde après intérêts: {savings1.GetBalance():C2}");

// Test de la méthode de rapport global
Console.WriteLine("\n--- RAPPORT GLOBAL DE LA BANQUE ---");
double totalMarko = bnp.GetTotalBalanceForPerson(client1);
Console.WriteLine($"Solde total pour {client1.FirstName} {client1.LastName}: {totalMarko:C2}");


public class Person
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime BirthDate { get; private set; } 

    public Person(string firstName, string lastName, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }
}

public interface IAccount
{
    void Deposit(double amount);
    void Withdraw(double amount);
    double GetBalance();
}

public interface IBankAccount : IAccount
{
    void ApplyInterest();
    Person Owner { get; }
    string Number { get; }
}   

public abstract class BankAccount : IBankAccount
{
    public string Number { get; private set; }
    public double Balance { get; protected set; } 
    public Person Owner { get; private set; } 

    public BankAccount(string number, Person owner) : this(number, 0.0, owner) { }
    public BankAccount(string number, double initialBalance, Person owner)
    {
        Number = number;
        Balance = initialBalance;
        Owner = owner;
    }

    public virtual void Deposit(double amount) 
    {
        if (amount > 0)
        {
            Balance += amount;
            Console.WriteLine($"Compte {Number}: + {amount:C2} (Dépôt {this.GetType().Name}). Nouveau solde: {Balance:C2}");
        }
        else
        {
            Console.WriteLine($"Compte {Number}: Erreur de dépôt. Le montant doit être positif.");
        }
    }
    
    public abstract void Withdraw(double amount); 
    
    public double GetBalance()
    {
        return Balance;
    }

    public void ApplyInterest() 
    {
        double interest = CalculateInterest();
        Balance += interest;
        Console.WriteLine($"Compte {Number}: + {interest:C2} (Intérêts appliqués). Nouveau solde: {Balance:C2}");
    }
    
    protected abstract double CalculateInterest();
}

public class CurrentAccount : BankAccount
{
    public double CreditLine { get; private set; }

    public CurrentAccount(string number, double creditLine, Person owner)
        : base(number, owner) 
    {
        CreditLine = creditLine;
    }

    public CurrentAccount(string number, double initialBalance, double creditLine, Person owner)
        : base(number, initialBalance, owner)
    {
        CreditLine = creditLine;
    }

    public override void Withdraw(double amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine($"Compte {Number}: Erreur de retrait. Le montant doit être positif.");
            return;
        }

        double allowedThreshold = -CreditLine; 
        
        if (Balance - amount >= allowedThreshold)
        {
            Balance -= amount; 
            Console.WriteLine($"Compte {Number}: - {amount:C2} (Retrait Courant). Nouveau solde: {Balance:C2}");
        }
        else
        {
            Console.WriteLine($"Compte {Number}: Retrait de {amount:C2} refusé. Le solde disponible ({Balance + CreditLine:C2}) est insuffisant.");
        }
    }
    
    protected override double CalculateInterest()
    {
        return Balance * 0.001; 
    }
}

public class SavingsAccount : BankAccount
{
    public DateTime DateLastWithdraw { get; private set; }

    public SavingsAccount(string number, Person owner)
        : base(number, owner) 
    {
        DateLastWithdraw = DateTime.MinValue; 
    }

    public SavingsAccount(string number, double initialBalance, Person owner)
        : base(number, initialBalance, owner)
    {
        DateLastWithdraw = DateTime.MinValue; 
    }

    public override void Withdraw(double amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine($"Compte Épargne {Number}: Erreur de retrait. Le montant doit être positif.");
            return;
        }

        if (Balance >= amount)
        {
            Balance -= amount;
            DateLastWithdraw = DateTime.Now; 
            Console.WriteLine($"Compte Épargne {Number}: - {amount:C2} (Retrait Épargne). Nouveau solde: {Balance:C2}");
        }
        else
        {
            Console.WriteLine($"Compte Épargne {Number}: Retrait de {amount:C2} refusé. Solde insuffisant.");
        }
    }

    protected override double CalculateInterest()
    {
        return Balance * 0.02;
    }
}

public class Bank
{
    public Dictionary<string, IBankAccount> Accounts { get; } 
    public string Name { get; private set; }
    public Bank(string name)
    {
        Name = name;
        Accounts = new Dictionary<string, IBankAccount>();
    }

    public void AddAccount(IBankAccount account)
    {
        if (Accounts.ContainsKey(account.Number))
        {
            Console.WriteLine($"Erreur: Le compte {account.Number} existe déjà dans la banque.");
        }
        else
        {
            Accounts.Add(account.Number, account);
            Console.WriteLine($"Compte {account.Number} ajouté à la banque {Name} (Type: {account.GetType().Name})."); 
        }
    }

    public void DeleteAccount(string number)
    {
        if (Accounts.Remove(number))
        {
            Console.WriteLine($"Compte {number} a été supprimé.");
        }
        else
        {
            Console.WriteLine($"Erreur: Le compte {number} n'existe pas dans la banque.");
        }
    }

    public double GetTotalBalanceForPerson(Person owner)
    {
        double total = Accounts.Values 
            .Where(account => account.Owner == owner) 
            .Sum(account => account.GetBalance()); 
            
        return total;
    }
}