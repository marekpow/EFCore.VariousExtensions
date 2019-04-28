/****************************************
 * 
 * © 2019, Marek Powichrowski, Poland, Białystok
 * 
 * *************************************/

 using System;

namespace EFCore.VariousExtensions.TableOfValues.Test
{
    public class TestDbSet
    {
        public Guid Id { get; set; }

        public int IntItem { get; set; }

        public string StringItem { get; set; }

        public ColorEnums EnumItem { get; set; }
    }
}
