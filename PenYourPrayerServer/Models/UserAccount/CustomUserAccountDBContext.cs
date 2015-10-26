using PenYourPrayer.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PenYourPrayer.Models.UserAccount
{
    public class CustomUserAccountDBContext<MyIUser> : DbContext
    {
        public CustomUserAccountDBContext()
            : this("DefaultConnection")
        {
        }

        public CustomUserAccountDBContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}