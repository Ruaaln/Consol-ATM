using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consol_ATM
{
    internal class Card
    {
        public string PAN { get; set; }          // Kartın üzərindəki 16 rəqəmli nömrə (Primary Account Number)
        public string PIN { get; set; }          // Bankomatda istifadəçi tərəfindən daxil edilən 4 rəqəmli gizli kod
        public string CVC { get; set; }          // Kartın arxasındakı 3 rəqəmli təhlükəsizlik kodu (Card Verification Code)
        public string ExpireDate { get; set; }   // Kartın etibarlılıq müddəti (ay/il formatında, məsələn: 06/2026)
        public decimal Balance { get; set; }     // Kartdakı mövcud məbləğ (pul balansı)


        public Card(string pan, string pin, string cvc, string expireDate, decimal balance)
        {
            PAN = pan;
            PIN = pin;
            CVC = cvc;
            ExpireDate = expireDate;
            Balance = balance;
        }

        //kartın balansını göstərir
        public void ShowBalance()
        {
            Console.WriteLine($"Balans: {Balance:C}");
        }

        //kartın balansindan cixaris
        public void Withdraw(decimal amount)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                Console.WriteLine($"Uğurla {amount:C} məbləğində pul çıxarıldı. Yeni balans: {Balance:C}");
            }
            else
            {
                Console.WriteLine("Balans kifayət etmir!");
            }
        }

        // kartın balansindan pul köçürülməsi
        public void Transfer(Card recipientCard, decimal amount)
        {
            
            if (amount <= Balance)
            {
                Balance -= amount;
                recipientCard.Balance += amount;
                Console.WriteLine($"Uğurla {amount:C} məbləğində pul köçürüldü. Yeni balans: {Balance:C}");
            }
            else
            {
                Console.WriteLine("Balans kifayət etmir!");
            }
        }
    }
}
