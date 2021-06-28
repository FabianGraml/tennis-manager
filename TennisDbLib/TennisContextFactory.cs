using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisDbLib
{
    public class TennisContextFactory : IDesignTimeDbContextFactory<TennisContext>
    {
        public TennisContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<TennisContext>();
            optionBuilder.UseSqlServer(@"server=(LocalDB)\\mssqllocaldb; attachdbfilename=D:\\Temp\\TennisDb.mdf;integrated security=True");

            return new TennisContext(optionBuilder.Options);
        }
    }


}
