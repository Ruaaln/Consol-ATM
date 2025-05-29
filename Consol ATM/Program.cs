using System;
using System.Security.Cryptography.X509Certificates;
using Consol_ATM;


//[PIN daxil et]
//       |
//       |--- PIN səhv → [Yenidən PIN daxil et]
//       |
//       |--- PIN doğru →
//               |
//               |--- 1. Balans göstər
//               |
//               |--- 2. Nağd pul çıxart
//               |         |
//               |         |--- Məbləği seç → Balans kifayət edirmi?
//               |                     |--- Yoxsa: Exception
//               |                     |--- Bəli: Balansdan çıx
//               |
//               |--- 3. Əməliyyatlar tarixi → Tarixlərə görə süzgəc
//               |
//               |--- 4. Köçürmə
//                         |
//                         |--- Digər PIN daxil et
//                         |--- Məbləği yaz
//                         |--- Balans kifayət edirmi?
//                                     |--- Yoxsa: Exception
//                                     |--- Bəli: Transfer et




class Program
{
    static void Main(string[] args)
    {
        // Defolt olaraq random Azerbaycan adlari ile 5 useri user listine elave et
        var users = new List<User>
        {
            new User(1, "Tural", "Məmmədov", 1400, new Card("1234567812345678", "1234", "321", "06/2026", 1000)),
            new User(2, "Aygün", "Əliyeva", 1700, new Card("8765432187654321", "5678", "654", "09/2027", 2000)),
            new User(3, "Rəşad", "Həsənov", 900, new Card("1111222233334444", "9101", "987", "12/2025", 3000)),
            new User(4, "Nigar", "Quliyeva", 2500, new Card("4444333322221111", "1121", "159", "04/2024", 4000)),
            new User(5, "Elvin", "Cəfərov", 3000, new Card("9999888877776666", "3141", "753", "01/2028", 5000))
        };

        // Programın başlanğıcı. baslangicde ekranin ust orta hisselerinden atm yazisi gosterilir. be simbollarla ezetilmis xos gorunum vreiili
        while (true)
        {

            Console.Clear();
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("------------------<< ATM >>----------------------");
            Console.WriteLine("-------------------------------------------------\n");


            Console.WriteLine("ATM sisteminə xoş gəlmisiniz!\n");
            Console.WriteLine("PAN kodunuzu yazın: ");
            string? PAN = Console.ReadLine();

            // yoxlamaq üçün GetUserByPAN metodunu çağırın
            if (GetUserByPAN(users, PAN) is User user)
            {
                Console.Clear();
                Console.WriteLine($"Xoş gəldiniz, {user.Name} {user.Surname}!\n");
                Console.WriteLine("PIN kodunuzu daxil edin: ");
                string? pin = Console.ReadLine();
                if (user.CreditCard.PIN == pin)
                {
                    Console.Clear();
                    Console.WriteLine("Ugurlu giris!\n");
                    ShowMenu(user, users);
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("PIN kodu səhvdir! Yenidən cəhd edin.");
                }
            }
            else
            {
                Console.WriteLine("PAN kodu tapılmadı!");
            }
        }

    }
    //PAN kod emiliyini yoxla
    static User? GetUserByPAN(List<User> users, string? pan)
    {
        foreach (var user in users)
        {
            if (user.CreditCard.PAN == pan)
            {
                return user;
            }
        }
        return null;
    }

    //ShowMenu metodunu User
    //1. Balans
    //2. Nağd pul
    //3. Əməliyyatlar tarixi
    //4. Kartdan karta köçürmə

    static void ShowMenu(User user, List<User> users)
    {
        while (true)
        {
            Console.WriteLine("\nATM Menyusu:");
            Console.WriteLine("1. Balans göstər");
            Console.WriteLine("2. Nağd pul çıxart");
            Console.WriteLine("3. Əməliyyatlar tarixi");
            Console.WriteLine("4. Kartdan karta köçürmə");
            Console.WriteLine("5. Çıxış");
            Console.Write("Seçiminizi edin: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    user.CreditCard.ShowBalance();
                    break;
                case "2":
                    Withdraw(user);
                    break;
                case "3":
                    ShowTransactionHistory(user);
                    break;
                case "4":
                    TransferMoney(user, users);
                    break;
                case "5":
                    Console.WriteLine("ATM-dən çıxış edirsiniz. Sağ olun!");
                    return;
                default:
                    Console.WriteLine("Yanlış seçim! Yenidən cəhd edin.");
                    break;
            }

            Console.WriteLine("\nDavam etmək üçün Enter düyməsini basın...");
            Console.ReadLine();
            Console.Clear();

        }
    }


    //balans cixarisi üçün Withdraw metodunu yazın
    static void Withdraw(User user)
    {
        Console.Write("Çıxarılacaq məbləği daxil edin: ");

        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            if (amount <= user.CreditCard.Balance)
            {
                user.CreditCard.Withdraw(amount);
                user.Operations.Add($"Çıxarış: {amount:C} - {DateTime.Now}");
            }
            else
            {
                Console.WriteLine("Balans kifayət etmir!");
            }
        }
        else
        {
            Console.WriteLine("Yanlış məbləğ daxil etdiniz!");
        }


    }

    //ShowTransactionHistory metodunu yazın
    static void ShowTransactionHistory(User user)
    {
        Console.WriteLine("\nƏməliyyatlar tarixi:");
        if (user.Operations.Count == 0)
        {
            Console.WriteLine("Heç bir əməliyyat yoxdur.");
        }
        else
        {

            foreach (var operation in user.Operations)
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine(operation);
            }
        }
    }

    //TransferMoney metodunu yazın
    static void TransferMoney(User user, List<User> users)
    {
        Console.Write("Köçürmək istədiyiniz kartın PAN kodunu daxil edin: ");
        string? recipientPAN = Console.ReadLine();
        User? recipient = GetUserByPAN(users, recipientPAN);
        if (recipient == null)
        {
            Console.WriteLine("Köçürmək istədiyiniz kart tapılmadı!");
            return;
        }
        Console.Write("Köçürmək istədiyiniz məbləği daxil edin: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            if (amount <= user.CreditCard.Balance)
            {
                user.CreditCard.Transfer(recipient.CreditCard, amount);
                user.Operations.Add($"Köçürmə: {amount:C} - {DateTime.Now}");
                recipient.Operations.Add($"Alınan köçürmə: {amount:C} - {DateTime.Now}");
            }
            else
            {
                Console.WriteLine("Balans kifayət etmir!");
            }
        }
        else
        {
            Console.WriteLine("Yanlış məbləğ daxil etdiniz!");
        }
    }
}



