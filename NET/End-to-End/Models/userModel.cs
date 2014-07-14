using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace End_to_End.Models
{
    public class UsersContext : DbContext
    {

        public class UserModel
        {
            public decimal Amount { get; set; }
            public int cvv { get; set; }
            public int expMonth { get; set; }
            public int expYear { get; set; }
            public string number { get; set; }
            public string address { get; set; }
            public string zipcode { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string phoneNumber { get; set; }
            public string Email { get; set; }
            public string city { get; set; }
            public string state { get; set; }

        }

    }


}