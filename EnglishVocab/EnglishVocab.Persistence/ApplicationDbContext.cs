﻿using Microsoft.EntityFrameworkCore;

namespace EnglishVocab.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}