using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Meta.Numerics;

namespace Psychex.Logic.Helpers
{
    public static class VirtualTableEx
    {
        public static IEnumerable<string> GetHeaders<TItem>(this VirtualTable<TItem> table)
        {
            return table.Columns.Select(column => column.Header);
        }

        public static IEnumerable<object> GetData<TItem>(this VirtualTable<TItem> table, TItem dataSource)
        {
            return table.Columns.Select(column => column.DataFetcher(dataSource));
        }

        public static void ExportCsv<TItem>(this VirtualTable<TItem> table, IEnumerable<TItem> dataSources, string filename, Encoding encoding = null)
        {
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(fs, encoding ?? Encoding.UTF8))
                {
                    ExportCsv<TItem>(table, dataSources, writer);
                }
            }
        }

        private static void ExportCsv<T>(VirtualTable<T> table, IEnumerable<T> dataSources, StreamWriter target)
        {
            ExportCsvLine(GetHeaders(table), target);
            foreach (var dataSource in dataSources)
            {
                ExportCsvLine(GetData(table, dataSource), target);
            }
        }

        public static void ExportCsvInverted<TItem>(this VirtualTable<TItem> table, IEnumerable<TItem> dataSources, string filename, Encoding encoding = null)
        {
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(fs, encoding ?? Encoding.UTF8))
                {
                    ExportCsvInverted<TItem>(table, dataSources, writer);
                }
            }
        }

        private static void ExportCsvInverted<T>(VirtualTable<T> table, IEnumerable<T> dataSources, StreamWriter target)
        {
            foreach (var column in table.Columns)
            {
                ExportCsvLine(GetColumnHeaderAndData(table, column, dataSources), target);
            }
        }

        private static IEnumerable<object> GetColumnHeaderAndData<T>(VirtualTable<T> table, VirtualTable<T>.Column column, IEnumerable<T> dataSources)
        {
            yield return column.Header;
            foreach (var dataSource in dataSources)
            {
                object data = null;
                try
                {
                    data = column.DataFetcher(dataSource);
                }
                catch
                {
                }
                yield return data;
            }
        }

        private static void ExportCsvLine(IEnumerable<object> lineItems, StreamWriter target)
        {
            var isFirst = true;
            foreach (var lineItem in lineItems)
            {
                if (!isFirst)
                {
                    target.Write("\t");
                }
                else
                {
                    isFirst = false;
                }
                var itemString = ConvertToString(lineItem);
                if (itemString.Contains('\t'))
                {
                    target.Write('"');
                    target.Write(itemString);
                    target.Write('"');
                }
                else
                {
                    target.Write(itemString);
                }
            }
            target.Write("\r\n");
        }

        public static string ConvertToString(object item)
        {
            if (item == null) return string.Empty;
            if (item is UncertainValue)
            {
                var uncertainValue = (UncertainValue) item;
                return string.Format("{0:#0.00}±{1:#0.00}", uncertainValue.Value, uncertainValue.Uncertainty, uncertainValue.ConfidenceInterval(0.95).LeftEndpoint, uncertainValue.ConfidenceInterval(0.95).RightEndpoint).Replace('.', ',');
            }
            return item.ToString();
        }
    }
}