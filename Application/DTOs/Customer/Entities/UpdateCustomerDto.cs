﻿namespace Application.DTOs.Customer.Entities;

public class UpdateCustomerDto:BaseDto
{
    public UpdateCustomerDto()
    {

    }
    public UpdateCustomerDto(int id,string firstName, string lastName, DateTime dateOfBirth,
        ulong phoneNumber, string email, string bankAccountNumber)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        Email = email;
        BankAccountNumber = bankAccountNumber;

    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; }=null!;
    public DateTime DateOfBirth { get; set; }
    public ulong PhoneNumber { get; set; }
    public string Email { get; set; }
    public string BankAccountNumber { get; set; }
}
