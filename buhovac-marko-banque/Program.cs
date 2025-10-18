using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

Console.WriteLine("--- DÉMARRAGE DE LA SIMULATION BANCAIRE ---");

// 1. Création des Personnes
Person client1 = new Person("Marko", "Buhovac", new DateTime(1867, 11, 7));
Person client2 = new Person("Max", "Verstappen", new DateTime(1879, 3, 14));
Person client3 = new Person("Nikola", "Tesla", new DateTime(1856, 7, 10));

Console.WriteLine($"\nCréation du client 1: {client1.FirstName} {client1.LastName}");
Console.WriteLine($"Création du client 2: {client2.FirstName} {client2.LastName}");

// 2. Création des Comptes
CurrentAccount account1 = new CurrentAccount("FR12345", 500.00, 1000.00, client1);
CurrentAccount account2 = new CurrentAccount("FR67890", 2500.50, 500.00, client1);

SavingsAccount savings1 = new SavingsAccount("EP00123", 15000.00, client1);
SavingsAccount savings2 = new SavingsAccount("EP00456", 50.00, client2);


// 3. Création de la Banque et ajout des comptes
Bank bnp = new Bank("BNP Paribas");
bnp.AddAccount(account1);
bnp.AddAccount(account2);
bnp.AddAccount(savings1);
bnp.AddAccount(savings2);

Console.WriteLine($"\nLa banque '{bnp.Name}' gère un total de {bnp.Accounts.Count} comptes.");

// 4. Test des opérations sur les comptes
Console.WriteLine("\n--- OPÉRATIONS BANCAIRES ---");

// Test Compte Courant
Console.WriteLine($"Solde initial Compte Courant {account1.Number}: {account1.GetBalance():C2}");
account1.Deposit(100.00); 
account1.Withdraw(300.00); 

// Test Compte Épargne
Console.WriteLine($"\nSolde initial Compte Épargne {savings1.Number}: {savings1.Balance:C2}");
savings1.Deposit(500.00);
savings1.Withdraw(20000.00);
savings1.Withdraw(500.00); 
Console.WriteLine($"Date du dernier retrait (Épargne): {savings1.DateLastWithdraw:yyyy-MM-dd HH:mm}");


// 5. Test de la méthode de rapport global
Console.WriteLine("\n--- RAPPORT GLOBAL DE LA BANQUE ---");

// Somme des comptes
double totalMarko = bnp.GetTotalBalanceForPerson(client1);
Console.WriteLine($"Solde total pour {client1.FirstName} {client1.LastName}: {totalMarko:C2}");

// Somme des comptes
double totalMax = bnp.GetTotalBalanceForPerson(client2);
Console.WriteLine($"Solde total pour {client2.FirstName} {client2.LastName}: {totalMax:C2}");


// Test de suppression de compte
Console.WriteLine("\n--- GESTION DES COMPTES ---");
bnp.DeleteAccount(account2.Number);
bnp.DeleteAccount(savings2.Number);
bnp.DeleteAccount("NonExistant");
Console.WriteLine($"La banque gère maintenant {bnp.Accounts.Count} comptes.");

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }

    public Person(string firstName, string lastName, DateTime birthDate)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
    }
}
public class BankAccount
{
    public string Number { get; private set; }
    public double Balance { get; protected set; } 
    public Person Owner { get; private set; }

    public BankAccount(string number, double initialBalance, Person owner)
    {
        Number = number;
        Balance = initialBalance;
        Owner = owner;
    }

    // Méthode de dépôt commune à tous les comptes
    public virtual void Deposit(double amount)
    {
        if (amount > 0)
        {
            Balance += amount;
            Console.WriteLine($"Compte {Number}: + {amount:C2} (Dépôt). Nouveau solde: {Balance:C2}");
        }
        else
        {
            Console.WriteLine($"Compte {Number}: Erreur de dépôt. Le montant doit être positif.");
        }
    }

    public void Withdraw(double amount) 
    {
        Console.WriteLine($"Erreur: La méthode Withdraw doit être appelée sur un type de compte spécifique (Courant ou Épargne).");
    }
    
    // Méthode pour obtenir le solde
    public double GetBalance()
    {
        return Balance;
    }
}
public class CurrentAccount : BankAccount
{
    public double CreditLine { get; set; }

    public CurrentAccount(string number, double initialBalance, double creditLine, Person owner)
        : base(number, initialBalance, owner)
    {
        CreditLine = creditLine;
    }

    // Retrait spécifique (avec ligne de crédit)
    public new void Withdraw(double amount)
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
}
public class SavingsAccount : BankAccount
{
    public DateTime DateLastWithdraw { get; private set; }

    public SavingsAccount(string number, double initialBalance, Person owner)
        : base(number, initialBalance, owner)
    {
        DateLastWithdraw = DateTime.MinValue; 
    }

    // Retrait spécifique
    public new void Withdraw(double amount)
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
}
public class Bank
{
    public Dictionary<string, BankAccount> Accounts { get; } 
    public string Name { get; set; }

    public Bank(string name)
    {
        Name = name;
        Accounts = new Dictionary<string, BankAccount>();
    }

    public void AddAccount(BankAccount account)
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

    // 5. Méthode pour donner la somme de tous les comptes d'une personne (tous les types)
    public double GetTotalBalanceForPerson(Person owner)
    {
        double total = Accounts.Values 
            .Where(account => account.Owner == owner) 
            .Sum(account => account.Balance);
            
        return total;
    }
}
