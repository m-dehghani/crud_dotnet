﻿namespace Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerRequest
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BankAccountNumber { get; set; } = string.Empty;
    }
}