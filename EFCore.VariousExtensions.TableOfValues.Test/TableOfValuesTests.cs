/****************************************
 * 
 * © 2019, Marek Powichrowski, Poland, Bia³ystok
 * 
 * *************************************/

 using NUnit.Framework;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System;

namespace EFCore.VariousExtensions.TableOfValues.Test
{
    public class TableOfValuesTests
    {
        private DbConnection dbConnection;
        private DbContextOptions<DbTestContext> dbOptions;

        [SetUp]
        public void Setup()
        {
            dbConnection = new SqlConnection(@"Server=.;Database=VariousExtensions;Trusted_Connection=True;");
            dbConnection.Open();

            dbOptions = new DbContextOptionsBuilder<DbTestContext>()
                    .UseSqlServer(dbConnection)
                    .Options;

            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                bool created = context.Database.EnsureCreated();
            }
        }

        [Test]
        public void TableOfIntValuesTest()
        {
            List<int> list = new List<int>() { 1, 2, 3 };

            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                List<TableOfValuesDto<int>> result = context.TableOfIntValues(list).ToList();

                Assert.AreEqual(result.Count(), 3);
                Assert.AreEqual(result[0].Value, 1);
                Assert.AreEqual(result[1].Value, 2);
                Assert.AreEqual(result[2].Value, 3);
            }
        }

        [Test]
        public void TableOfStringValuesTest()
        {
            List<string> list = new List<string>() { "a", "c", "d" };

            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                List<TableOfValuesDto<string>> result = context.TableOfStringValues(list).ToList();

                Assert.AreEqual(result.Count(), 3);
                Assert.AreEqual(result[0].Value, "a");
                Assert.AreEqual(result[1].Value, "c");
                Assert.AreEqual(result[2].Value, "d");
            }
        }

        [Test]
        public void TableOfGuidValuesTest()
        {
            Guid guid1 = Guid.NewGuid();
            Guid guid2 = Guid.NewGuid();
            Guid guid3 = Guid.NewGuid();

            List<Guid> list = new List<Guid>() { guid1, guid2, guid3 };

            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                List<TableOfValuesDto<Guid>> result = context.TableOfGuidValues(list).ToList();

                Assert.AreEqual(result.Count(), 3);
                Assert.AreEqual(result[0].Value, guid1);
                Assert.AreEqual(result[1].Value, guid2);
                Assert.AreEqual(result[2].Value, guid3);
            }
        }

        [Test]
        public void TableOfEnumIntValues()
        {
            List<ColorEnums> list = new List<ColorEnums>() { ColorEnums.Blue, ColorEnums.Blue, ColorEnums.Red };

            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                List<TableOfValuesDto<ColorEnums>> result = context.TableOfColorEnumValues(list).ToList();

                Assert.AreEqual(result.Count(), 3);
                Assert.AreEqual(((ColorEnums)result[0].Value), ColorEnums.Blue);
                Assert.AreEqual(((ColorEnums)result[1].Value), ColorEnums.Blue);
                Assert.AreEqual(((ColorEnums)result[2].Value), ColorEnums.Red);
            }
        }

        [Test]
        public void JoinTableOfIntValues()
        {
            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.TestDbSet;");

                TestDbSet entity = new TestDbSet()
                {
                    IntItem = 1
                };

                context.Add(entity);

                context.SaveChanges();

                List<int> list = new List<int>() { 1, 2, 3 };

                var result = (from toiv in context.TableOfIntValues(list)
                             join ddbs in context.TestDbSet on toiv.Value equals ddbs.IntItem
                             select ddbs.IntItem).ToList();

                Assert.AreEqual(result.Count(), 1);

                Assert.AreEqual(result[0], 1);
            }
        }

        [Test]
        public void JoinTableOfStringValues()
        {
            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.TestDbSet;");

                TestDbSet entity = new TestDbSet()
                {
                    StringItem = "a"
                };

                context.Add(entity);

                context.SaveChanges();

                List<string> list = new List<string>() { "a", "b", "c" };

                var result = (from toiv in context.TableOfStringValues(list)
                              join ddbs in context.TestDbSet on toiv.Value equals ddbs.StringItem
                              select ddbs.StringItem).ToList();

                Assert.AreEqual(result.Count(), 1);

                Assert.AreEqual(result[0], "a");
            }
        }

        [Test]
        public void JoinTableOfGuidValues()
        {
            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.TestDbSet;");

                Guid guid = Guid.NewGuid();

                TestDbSet entity = new TestDbSet()
                {
                    Id = guid
                };

                context.Add(entity);

                context.SaveChanges();

                List<Guid> list = new List<Guid>() { guid, Guid.NewGuid(), Guid.NewGuid() };

                var result = (from toiv in context.TableOfGuidValues(list)
                              join ddbs in context.TestDbSet on toiv.Value equals ddbs.Id
                              select ddbs.Id).ToList();

                Assert.AreEqual(result.Count(), 1);

                Assert.AreEqual(result[0], guid);
            }
        }

        [Test]
        public void JoinTableOfEnumValues()
        {
            using (DbTestContext context = new DbTestContext(dbOptions))
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE dbo.TestDbSet;");

                TestDbSet entity = new TestDbSet()
                {
                    EnumItem = ColorEnums.Blue
                };

                context.Add(entity);

                context.SaveChanges();

                List<ColorEnums> list = new List<ColorEnums>() { ColorEnums.Blue, ColorEnums.Green, ColorEnums.Red };

                var result = (from toiv in context.TableOfColorEnumValues(list)
                              join ddbs in context.TestDbSet on toiv.Value equals ddbs.EnumItem
                              select ddbs.EnumItem).ToList();

                Assert.AreEqual(result.Count(), 1);

                Assert.AreEqual(result[0], ColorEnums.Blue);
            }
        }

        [TearDown]
        public void TestCleanup()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Close();  
            }
        }
    }
}