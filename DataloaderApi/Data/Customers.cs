using System;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace DataloaderApi.Data
{
    public class Customers
    {
        [Key]
        [Name("Index")]
        public int ID { get; set; }  

        [Name("Customer Id")]
        public string CustomerID { get; set; }

        [Name("First Name")]
        public string FirstName { get; set; }

        [Name("Last Name")]
        public string LastName { get; set; }

        [Name("Company")]
        public string Company { get; set; }

        [Name("City")]
        public string City { get; set; }

        [Name("Country")]
        public string Country { get; set; }

        [Name("Phone 1")]
        public string PhoneOne { get; set; }

        [Name("Phone 2")]
        public string PhoneTwo { get; set; }

        [Name("Email")]
        public string Email { get; set; }

        [Name("Subscription Date")]
        public DateTime SubscriptionDate { get; set; }

        [Name("Website")]
        public string Website { get; set; }
    }
}
