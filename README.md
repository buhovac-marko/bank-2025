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

f. Creer une classe << SavingsAccount >> pour la gestion d'un carnet d'epargne implementant
    - Les propriétés publiques.
        - string Number
        - double Balance (lecture seule)
        - Date Time DateLastWithdraw
        - Person owner
    - Une méthode publique:
        - void Withdraw(double amount)
        - void Deposit (double amount)

g. Définir la classe <Account> reprenant les parties commune aux classes <CurrentAccount>
 et <SavingsAccount> en utilisant les concepts d'héritage, de redéfinition de méthodes et si besoin, de surcharcge
 de méthodes et d'encapsulation.
 Attention le niveau d'accessibilité du mutateur de la propriété Balance doit rester <private>

h.

i.

j. Définir la classe « Account » comme étant abstraite.

k. Ajouter une méthode abstraite « protected » à la classe « Account » ayant le
prototype « double CalculInterets() » en sachant que pour un livret d’épargne le taux
est toujours de 4.5% tandis que pour le compte courant si le solde est positif le taux
sera de 3% sinon de 9.75%.

l. Ajouter une méthode « public » à la classe « Account » appelée « ApplyInterest » qui
additionnera le solde avec le retour de la méthode « CalculInterest ».

12. Définir l’interface « IAccount », afin de limiter l’accès à consulter la propriété «
Balance » et d’utiliser les méthodes « Depotsit » et « Withdraw ».

13. Définir l’interface « IBankAccount » ayant les mêmes fonctionnalités que
« IAccount ».
Elle lui permettra, en plus, d’invoquer la méthode du « ApplyInteret » et offrira
un accès en lecture au « Owner » et au « Number ».