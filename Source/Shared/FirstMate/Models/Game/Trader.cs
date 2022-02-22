using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMate
{
    public record Trader(string LoginName, string Alias)
    {
        public DateTime Created { get; set; }
    };
    public record Player(Guid Id, string LN, string LoginName, string Alias) 
        : Trader(LoginName, Alias);

    public class Trader1
    {
        public string FirstName { get; }
        public string LastName { get; }

        public Trader1(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

    }
}
