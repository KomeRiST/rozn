using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rozn
{
    class ApplicationContext : DbContext
    {

        public ApplicationContext() : base("RoznConnection")
        {
        }
        public DbSet<tovar> Tovars { get; set; }




    }
}
