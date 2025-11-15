using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;

Console.WriteLine("DÉMARRAGE DE LA SIMULATION BANCAIRE");

// 1. Création des Personnes
Person client1 = new Person("Marko", "Buhovac", new DateTime(1867, 11, 7));
Person client2 = new Person("Max", "Verstappen", new DateTime(1879, 3, 14));

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

Console.WriteLine("\n--- TEST DE L'ÉVÉNEMENT NEGATIVE BALANCE ---");

// Compte 1: Solde initial: 500.00, Ligne de crédit: 1000.00
Console.WriteLine($"Solde initial {account1.Number}: {account1.GetBalance():C2}");

// Tentative A: Retrait normal (le compte reste positif)
Console.WriteLine("\nTentative A: Retrait qui maintient le solde positif (500.00 - 100.00 = 400.00)");
try {
    account1.Withdraw(100.00); 
}
catch (Exception ex) { 
    Console.WriteLine($"[Erreur]: {ex.Message}");
}

// Tentative B: Retrait critique (le compte PASSE en négatif)
Console.WriteLine("\nTentative B: Retrait qui fait PASSER le compte en négatif (400.00 - 800.00 = -400.00)");
try {
    account1.Withdraw(800.00); 
}
catch (Exception ex) { 
    Console.WriteLine($"[Erreur]: {ex.Message}");
}

// Tentative C: Retrait en étant DÉJÀ en négatif
Console.WriteLine("\nTentative C: Retrait en étant déjà négatif (-400.00 - 100.00 = -500.00)");
try {
    account1.Withdraw(100.00); 
}
catch (Exception ex) { 
    Console.WriteLine($"[Erreur]: {ex.Message}");
}

// 5. Test final (état général)
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

public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(string message) : base(message) { }
}

public class InvalidCreditLineException : ArgumentOutOfRangeException
{
    public InvalidCreditLineException(string message) : base(message) { }
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
    public event Action<BankAccount> NegativeBalanceEvent;
    
    protected bool IsCurrentlyNegative { get; set; }

    public string Number { get; private set; }
    public double Balance { get; protected set; } 
    public Person Owner { get; private set; } 

    public BankAccount(string number, Person owner) : this(number, 0.0, owner) { }

    public BankAccount(string number, double initialBalance, Person owner)
    {
        Number = number;
        Balance = initialBalance;
        Owner = owner;
        IsCurrentlyNegative = initialBalance < 0; 
    }

    protected virtual void OnNegativeBalance()
    {
        NegativeBalanceEvent?.Invoke(this); 
    }

    public virtual void Deposit(double amount) 
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount), 
                $"Compte {Number}: Le montant du dépôt ({amount:C2}) doit être strictement supérieur à zéro."
            );
        }
        
        double oldBalance = Balance;
        Balance += amount;
        Console.WriteLine($"Compte {Number}: + {amount:C2} (Dépôt {this.GetType().Name}). Nouveau solde: {Balance:C2}");
        
        if (oldBalance < 0 && Balance >= 0)
        {
            IsCurrentlyNegative = false;
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
    private double _creditLine;
    
    public double CreditLine 
    { 
        get => _creditLine; 
        private set 
        {
            if (value < 0)
            {
                 throw new ArgumentOutOfRangeException(
                    nameof(CreditLine), 
                    $"La ligne de crédit ({value:C2}) doit être supérieure ou égale à zéro."
                );
            }
            _creditLine = value;
        }
    } 

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
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                $"Compte {Number}: Le montant du retrait ({amount:C2}) doit être strictement supérieur à zéro."
            );
        }

        double oldBalance = Balance;
        double allowedThreshold = -CreditLine; 
        
        if (Balance - amount >= allowedThreshold)
        {
            Balance -= amount; 
            Console.WriteLine($"Compte {Number}: - {amount:C2} (Retrait Courant). Nouveau solde: {Balance:C2}");
            
            if (Balance < 0 && oldBalance >= 0)
            {
                IsCurrentlyNegative = true;
                OnNegativeBalance(); 
            }
        }
        else
        {
            throw new InsufficientBalanceException(
                $"Compte {Number}: Retrait de {amount:C2} refusé. Le solde disponible ({Balance + CreditLine:C2}) est insuffisant."
            );
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
             throw new ArgumentOutOfRangeException(
                nameof(amount),
                $"Compte Épargne {Number}: Le montant du retrait ({amount:C2}) doit être strictement supérieur à zéro."
            );
        }

        if (Balance >= amount)
        {
            Balance -= amount;
            DateLastWithdraw = DateTime.Now; 
            Console.WriteLine($"Compte Épargne {Number}: - {amount:C2} (Retrait Épargne). Nouveau solde: {Balance:C2}");
        }
        else
        {
            throw new InsufficientBalanceException(
                $"Compte Épargne {Number}: Retrait de {amount:C2} refusé. Solde insuffisant ({Balance:C2})."
            );
        }
        // Note: L'événement NegativeBalanceEvent n'est pas pertinent ici car le solde ne peut pas être négatif.
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

    public void NegativeBalanceAction(BankAccount account)
    {
        Console.WriteLine($"\n[ALERTE BANQUE - URGENT] Le numéro de compte {account.Number} vient de passer en négatif (Solde: {account.GetBalance():C2})");
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
            
            if (account is BankAccount bankAccount)
            {
                bankAccount.NegativeBalanceEvent += NegativeBalanceAction;
                Console.WriteLine($"[Abonnement OK] La banque surveille les changements de solde de {account.Number}.");
            }
        }
    }

    public void DeleteAccount(string number)
    {
        if (Accounts.TryGetValue(number, out IBankAccount accountToRemove) && accountToRemove is BankAccount bankAccount)
        {
            bankAccount.NegativeBalanceEvent -= NegativeBalanceAction;
            Console.WriteLine($"[Désabonnement OK] La banque ne surveille plus {number}.");
        }

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