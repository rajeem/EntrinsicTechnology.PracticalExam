using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntrinsicTechnology.Data.Domains
{
    public class User : Entity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [MaxLength(10)]
        public string UserName { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
