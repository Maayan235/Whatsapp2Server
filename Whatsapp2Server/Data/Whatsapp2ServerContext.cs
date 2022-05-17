using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whatsapp2Server.Models;

namespace Whatsapp2Server.Data
{
    public class Whatsapp2ServerContext : DbContext
    {
        public Whatsapp2ServerContext (DbContextOptions<Whatsapp2ServerContext> options)
            : base(options)
        {
        }

        public DbSet<Whatsapp2Server.Models.Message>? Message { get; set; }

        public DbSet<Whatsapp2Server.Models.User>? User { get; set; }

        public DbSet<Whatsapp2Server.Models.Chat> Chat { get; set; }
    }
}
