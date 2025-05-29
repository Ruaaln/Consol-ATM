using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consol_ATM
{
    internal class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public Card CreditCard { get; set; }
        public List<string> Operations { get; set; }

        public User(int id, string name, string surname, decimal salary, Card card)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Salary = salary;
            CreditCard = card;
            Operations = new List<string>();
        }
    }
}
