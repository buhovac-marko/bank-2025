# Bank 2025

Développer une application simulant la gestion d'une banque.

A1. Faire un "Fork" de ce dépôt
A2. Créer un dossier avec votre "Nom Prénom" à la racine
A3. Développer l'application dans ce dossier
A4. Sauver chaque étape avec un "Commit" 
A5. Faire une "Pull request" pour m'envoyer votre travail

01. Creer une classe Person implementant
    Les proprietes publique
        string FirstName
        string LastName
        DateTime BirthDate

02. Creer une classe CurrentAccount qui permet la gestion d'un compte courant implementant:
    Les proprietes publique
        string Number
        double Balance (lecture seule)
        double CreditLine
        Person Owner
    Les Methid publiques
        void Withdraw(double amount)
        void Deposit(double amount)

03. Creer une classe Bank pour gerer les comptes courants implementant:
    Les proprietes
        Dictionary<string, CurrentAccount> Accounts (lecture seule)
        string Name
    Les Methodes publiques
        void AddAccount(CurrentAccount account)
        void DeleteAccount(string number)

04. Ajouter une methode qui retourne le solde d'un compte courant

05. Permettre a la banque de donner la somme de tous les comptes d'une personne

06. Creer une classe << SavingsAccount >> pour la gestion d'un carnet d'epargne implementant
    - Les propriétés publiques.
        - string Number
        - double Balance (lecture seule)
        - Date Time DateLastWithdraw
        - Person owner
    - Une méthode publique:
        - void Withdraw(double amount)
        - void Deposit (double amount)

07. Définir la classe <Account> reprenant les parties commune aux classes <CurrentAccount>
 et <SavingsAccount> en utilisant les concepts d'héritage, de redéfinition de méthodes et si besoin, de surcharcge
 de méthodes et d'encapsulation.
 Attention le niveau d'accessibilité du mutateur de la propriété Balance doit rester <private>

08.

09.

10. Définir la classe « Account » comme étant abstraite.

11. Ajouter une méthode abstraite « protected » à la classe « Account » ayant le
prototype « double CalculInterets() » en sachant que pour un livret d’épargne le taux
est toujours de 4.5% tandis que pour le compte courant si le solde est positif le taux
sera de 3% sinon de 9.75%.

12. Ajouter une méthode « public » à la classe « Account » appelée « ApplyInterest » qui
additionnera le solde avec le retour de la méthode « CalculInterest ».

13. Définir l’interface « IAccount », afin de limiter l’accès à consulter la propriété «
Balance » et d’utiliser les méthodes « Depotsit » et « Withdraw ».

14. Définir l’interface « IBankAccount » ayant les mêmes fonctionnalités que
« IAccount ».
Elle lui permettra, en plus, d’invoquer la méthode du « ApplyInteret » et offrira
un accès en lecture au « Owner » et au « Number ».

15. Ajoutez, dans la classe « Account », deux constructeurs prenant en paramètre :
• Le numéro et le titulaire
• Le numéro, le titulaire et le solde (pour le cas d’une base de données)

16. Le cas échéant, ajoutez le ou les constructeurs aux classes « CurrentAccount » et
« SavingsAccount ».

17. Changer l’encapsulation des propriétés des classes « Person », « Account » et
« SavingsAccount » afin de spécifier leur mutateur en « private ».

18. Définir ce qu’il manque pour que le programme continue à tourner.

19. Dans la classe « Account » :
• Au niveau de la méthode « Deposit », déclenchez une exception de type
« ArgumentOutOfRangeException » si le montant n’est pas supérieur à 0 (zéro).
• Faites de même au niveau de la méthode « Withdraw » et y ajouter le déclenchement
d’une exception de type « InsufficientBalanceException » si le montant ne peut être
retiré.

20. Au niveau de la classe « CurrentAccount » :
• Au niveau de la propriété « CreditLine », déclenchez une exception de type
« ArgumentOutOfRangeException » si la valeur n’est pas supérieur ou égale à 0 (zéro).

21. Dans la classe << Account >>:
    - Ajoutez un evenement apple << NegativeBalanceEvent >> dont la delegue
        << NegativeBalanceDelegate >> devra recevoir en parametre un objet de type << Account >> et ne rien renvoyer

22. Au nivo de la classe << CurrentAccount >> :
    Declencher l'evenement << NegativeBalanceEvent >> si le compte passe en negatif et uniquement dans ce cas.

23. Au niveau de la classe << Bank >>
    Ajouter une methode qui traitera l'evenement << NegativeBalanceAction >> en affichant dans ma console
    << Le numero de compte {Number} vient de passer en negatif >>
    

