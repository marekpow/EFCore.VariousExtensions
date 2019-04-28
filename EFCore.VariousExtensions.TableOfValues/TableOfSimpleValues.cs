/****************************************
 * 
 * © 2019, Marek Powichrowski, Poland, Białystok
 * 
 * *************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace EFCore.VariousExtensions.TableOfValues
{
    public class TableOfSimpleValues<T,U> 
    {
        private readonly IEnumerable<T> ItemsList = new List<T>();
        private readonly string ValueName;

        public TableOfSimpleValues(IEnumerable<T> itemsList)
        {
            Type _valueType = typeof(T);
            Type _dtoType = typeof(U);

            Type[] _genericArguments = _dtoType.GetGenericArguments();

            if(_genericArguments.Count() == 0 || _genericArguments.Count() > 1)
            {
                throw new Exception("U type should be generic type of one argument");
            }

            if(_genericArguments[0].FullName != _valueType.FullName)
            {
                throw new Exception("T type and generic type of U should be the same");
            }

            if (!_valueType.IsPrimitive && !_valueType.IsEnum && !_valueType.Equals(typeof(string)) && !_valueType.Equals(typeof(Guid)))
            {
                throw new Exception("Can not create T-SQL table of simple values for non-simple types");
            }

            if(itemsList.Count() == 0)
            {
                throw new Exception("Can not create T-SQL table of simple values for empty list");
            }

            PropertyInfo[] _pi = _dtoType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (_pi.Count() > 1)
            {
                throw new Exception("Type T should have a maximum of one public property");
            }
            else if (_pi.Count() == 0)

            {
                throw new Exception("Type T should have at least one public property");
            }

            this.ValueName = _pi[0].Name;

            this.ItemsList = itemsList;
        }

        public string ToQuery()
        {
            string _sqlType;
            bool _isEnum = typeof(T).IsEnum;
            string _result;
            string enumeratedItems = string.Empty;

            if (_isEnum)
            {
                _sqlType = "INT";
            }
            else
            {
                int _precision = 0;
                int _scale = 0;
                _sqlType = string.Empty;

                foreach (var item in ItemsList)
                {
                    SqlParameter _p = new SqlParameter("@_", item);

                    _sqlType = _p.SqlDbType.ToString();
                    _precision = _p.Precision > _precision ? _p.Precision : _precision;
                    _scale = _p.Scale > _scale ? _p.Scale : _scale;
                }

                _sqlType += _precision > 0 ? _scale == 0 ? $"({_precision})" : $"({_precision},{_scale})" : string.Empty;
            }

            _result = $"SELECT CAST([tov].[{this.ValueName}] AS " + _sqlType + $") AS [{this.ValueName}] FROM (VALUES ";

            foreach (var item in this.ItemsList)
            {
                if (_isEnum)
                {
                    enumeratedItems += ",(" + (Convert.ToInt32(item)).ToString() + ")";
                }
                else
                {
                    enumeratedItems += ",('" + item.ToString() + "')";
                }
            }

            enumeratedItems = enumeratedItems.Substring(1, enumeratedItems.Length - 1);

            _result += enumeratedItems;

            _result += $") [tov]([{this.ValueName}]) ";

            return _result;
        }
    }
}
