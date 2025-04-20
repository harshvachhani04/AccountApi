using AccountApi.Common;
using System.ComponentModel.DataAnnotations;

namespace AccountApi.Models
{
    public class Account
    {
        public string AccountHolderName { get; set; }
        public int AccountNumber { get; set; }
        public double Balance { get; set; }
        public AccountTypes AccountType { get; set; }
    }
}
