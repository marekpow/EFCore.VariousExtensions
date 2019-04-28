/****************************************
 * 
 * © 2019, Marek Powichrowski, Poland, Białystok
 * 
 * *************************************/

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCore.VariousExtensions.TableOfValues.Test
{
    public static class DbTestContextExtensions
    {
        public static IQueryable<TableOfValuesDto<int>> TableOfIntValues(this DbTestContext context, IEnumerable<int> list)
        {
            TableOfSimpleValues<int, TableOfValuesDto<int>> valuesList = new TableOfSimpleValues<int, TableOfValuesDto<int>>(list);

            return context.TableOfIntQuery.FromSql(valuesList.ToQuery());
        }

        public static IQueryable<TableOfValuesDto<string>> TableOfStringValues(this DbTestContext context, IEnumerable<string> list)
        {
            TableOfSimpleValues<string, TableOfValuesDto<string>> valuesList = new TableOfSimpleValues<string, TableOfValuesDto<string>>(list);

            return context.TableOfStringQuery.FromSql(valuesList.ToQuery());
        }

        public static IQueryable<TableOfValuesDto<Guid>> TableOfGuidValues(this DbTestContext context, IEnumerable<Guid> list)
        {
            TableOfSimpleValues<Guid, TableOfValuesDto<Guid>> valuesList = new TableOfSimpleValues<Guid,TableOfValuesDto<Guid>>(list);

            return context.TableOfGuidQuery.FromSql(valuesList.ToQuery());
        }

        public static IQueryable<TableOfValuesDto<ColorEnums>> TableOfColorEnumValues(this DbTestContext context, IEnumerable<ColorEnums> list)
        {
            TableOfSimpleValues<ColorEnums, TableOfValuesDto<ColorEnums>> valuesList = new TableOfSimpleValues<ColorEnums, TableOfValuesDto<ColorEnums>>(list);

            return context.TableOfColorEnumQuery.FromSql(valuesList.ToQuery());
        }
    }
}

