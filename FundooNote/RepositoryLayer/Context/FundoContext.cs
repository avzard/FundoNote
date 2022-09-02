using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;


namespace RepositoryLayer.Context
{
    public class FundoContext : DbContext
    {
        
            public FundoContext(DbContextOptions options)
                : base(options)
            {
            }
            public DbSet<UserEntity> UserTable { get; set; }
            public DbSet<NotesEntity> NotesTable { get; set; }

            public DbSet<Collaboration> CollabTable { get; set; }
            public DbSet<LabelEntity> LabelTable { get; set; }
    }
}
