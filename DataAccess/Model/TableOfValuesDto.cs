/****************************************
 * 
 * © 2019, Marek Powichrowski, Poland, Białystok
 * 
 * *************************************/

 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.VariousExtensions.TableOfValues.Test
{
    public class TableOfValuesDto<T>
    {
        public T Value { get; internal set; }
    }
}
