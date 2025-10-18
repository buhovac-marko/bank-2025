# Bank 2025

Développer une application simulant la gestion d'une banque.

1. Faire un "Fork" de ce dépôt
2. Créer un dossier avec votre "Nom Prénom" à la racine
3. Développer l'application dans ce dossier
4. Sauver chaque étape avec un "Commit" 
5. Faire une "Pull request" pour m'envoyer votre travail

a. Creer une classe Person implementant
    Les proprietes publique
        string FirstName
        string LastName
        DateTime BirthDate

b. Creer une classe CurrentAccount qui permet la gestion d'un compte courant implementant:
    Les proprietes publique
        string Number
        double Balance (lecture seule)
        double CreditLine
        Person Owner
    Les Methid publiques
        void Withdraw(double amount)
        void Deposit(double amount)

c. Creer une classe Bank pour gerer les comptes courants implementant:
    Les proprietes
        Dictionary<string, CurrentAccount> Accounts (lecture seule)
        string Name
    Les Methodes publiques
        void AddAccount(CurrentAccount account)
        void DeleteAccount(string number)

d. Ajouter une methode qui retourne le solde d'un compte courant

e. Permettre a la banque de donner la somme de tous les comptes d'une personne
