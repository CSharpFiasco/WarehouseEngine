﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using WarehouseEngine.Domain.Entities;
using WarehouseEngine.Domain.Exceptions;
using WarehouseEngine.Domain.ValueObjects;

namespace WarehouseEngine.Domain.Tests;
public class CustomerTests
{
    private readonly ITestOutputHelper _output;
    public CustomerTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void GivenIdIsGiven_WhenDeserialized_ThenSerializationIsSuccessful()
    {
        //SETUP
        var json = """
            {
                "Id": "d3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b",
                "Name": "Test Customer",
                "ShippingAddress": {
                    "Address1": "123 Main St",
                    "City": "Anytown",
                    "State": "NY",
                    "ZipCode": "12345"
                },
                "DateCreated": "2021-01-01T00:00:00",
                "CreatedBy": "Test User"
            }
            """;
        _output.WriteLine(json);

        //ATTEMPT
        var customer = JsonSerializer.Deserialize<Customer>(json);
        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(customer!, new ValidationContext(customer!), errors, true);


        //VERIFY
        Assert.NotNull(customer);
        Assert.Empty(errors);
        Assert.True(isValid);
        Assert.Equal("d3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b", customer.Id.ToString());
    }

    [Fact]
    public void GivenNameIsNull_WhenDeserialized_ThenSerializationIsNotSuccessful()
    {
        //SETUP
        var json = """
            {
                "Id": "d3b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b",
                "Name": null,
                "ShippingAddress": {
                    "Address1": "123 Main St",
                    "City": "Anytown",
                    "State": "NY",
                    "ZipCode": "12345"
                },
                "DateCreated": "2021-01-01T00:00:00",
                "CreatedBy": "Test User"
            }
            """;
        _output.WriteLine(json);

        //ATTEMPT
        var customer = JsonSerializer.Deserialize<Customer>(json);
        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(customer!, new ValidationContext(customer!), errors, true);


        //VERIFY
        Assert.NotNull(customer);
        Assert.False(isValid);
        var error = Assert.Single(errors);
        Assert.Equal("The Name field is required.", error.ErrorMessage);
    }

    [Fact]
    public void GivenPostCustomerDto_WhenConvertedToCustomer_ThenConversionIsSuccessful()
    {
        //SETUP
        var dto = new PostCustomerDto
        {
            Id = Guid.NewGuid(),
            Name = "Test Customer",
            ShippingAddress = new Address
            {
                Address1 = "123 Main St",
                City = "Anytown",
                State = "NY",
                ZipCode = "12345"
            },
            DateCreated = DateTime.MinValue,
            CreatedBy = "a"
        };

        //ATTEMPT
        var customer = (Customer)dto;

        //VERIFY
        Assert.NotNull(customer);
        Assert.Equal(dto.Name, customer.Name);
        Assert.Equal(dto.ShippingAddress, customer.ShippingAddress);
    }

    [Fact]
    public void GivenPostCustomerDtoWithNullShippingAddress_WhenConvertedToCustomer_ThenThrowEntityConversionException()
    {
        //SETUP
        var dto = new PostCustomerDto
        {
            Name = "Test Customer",
            ShippingAddress = null!,
            DateCreated = DateTime.MinValue,
            CreatedBy = "a"
        };

        //ATTEMPT
        var ex = Assert.Throws<EntityConversionException<Customer, PostCustomerDto>>(() => (Customer)dto);


    }
}
