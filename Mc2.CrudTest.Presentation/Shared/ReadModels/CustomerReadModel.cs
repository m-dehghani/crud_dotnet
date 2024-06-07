using Mc2.CrudTest.Presentation.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mc2.CrudTest.Presentation.Shared.ReadModels
{
    public class CustomerReadModel
    {
        [Column("firstname")]
        public string _firstName { get; set; }
        public string FirstName { get => _firstName.Trim('"'); }

        [Column("lastname")]
        public string _lastName { get; set; }
        public string LastName { get => _lastName.Trim('"'); }

        [Column("phonenumber")]
        public string PhoneNumber { get; set; }
        
        [Column("email")]
        public string _email { get; set; }
        public string Email { get => _email.Trim('"'); }

        [Column("bankaccount")]
        public string _bankAccount { get; set; }
        public string BankAccount { get => _bankAccount.Trim('"'); }

        [Column("dateofbirth")]
        public string _dateOfBirth { get; set; }
        public DateOnly DateOfBirth { get => DateOnly.Parse(_dateOfBirth.Trim(['\"'])); }
        [Column("AggregateId")]
        public Guid AggregateId { get; set; }
        [Column("OccurredOn")]
        public DateTimeOffset OccurredOn { get; set; }
        [Column("event_type")]
        public string EventType { get; set; }
        public Boolean IsDeleted { get => Boolean.Parse(_isDeleted); }
        [Column("isdeleted")]
        public string _isDeleted { get; set; }
    }
}
