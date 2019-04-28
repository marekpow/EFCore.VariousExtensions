/****************************************
 * 
 * © 2019, Marek Powichrowski, Poland, Białystok
 * 
 * *************************************/

 using Microsoft.EntityFrameworkCore;
using System;

namespace EFCore.VariousExtensions.TableOfValues.Test
{
    public class DbTestContext : DbContext
    {
        public DbTestContext(DbContextOptions<DbTestContext> options)
          : base(options)
        {
        }

        public DbSet<TestDbSet> TestDbSet { get; set; }

        public DbQuery<TableOfValuesDto<int>> TableOfIntQuery { get; set; }

        public DbQuery<TableOfValuesDto<string>> TableOfStringQuery { get; set; }

        public DbQuery<TableOfValuesDto<Guid>> TableOfGuidQuery { get; set; }

        public DbQuery<TableOfValuesDto<ColorEnums>> TableOfColorEnumQuery { get; set; }
    }
}
