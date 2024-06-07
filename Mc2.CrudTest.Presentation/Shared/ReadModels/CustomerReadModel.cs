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
        public string FirstName { get; set; }
        [Column("lastname")]
        public string LastName { get; set; }
        [Column("phonenumber")]
        public string PhoneNumber { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("bankaccount")]
        public string BankAccount { get; set; }
        [Column("dateofbirth")]
        public string DateOfBirth { get; set; }
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
