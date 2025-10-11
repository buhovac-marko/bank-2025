/* 
1. Creer une classe Person implementant
Les proprietes publique
string FirstName
string LastName
DateTime BirthDate
2. Creer une classe CurrentAccount qui permet la gestion d'un compte courant implementant:
Les proprietes publique
string Number
double Balance (lecture seule)
double CreditLine
Person Owner
Les Methid publiques
void Withdraw(double amount)
void Deposit(double amount)
3. Creer une classe Bank pour gerer les comptes courants implementant:
Les proprietes
Dictionary<string, CurrentAccount> Accounts (lecture seule)
string Name
Les Methodes publiques
void AddAccount(CurrentAccount account)
void DeleteAccount(string number)
4. Ajouter une methode qui retourne le solde d'un compte courant
5. Permettre a la banque de donner la somme de tous les comptes d'une personne
*/
