using System;
using System.Collections.Generic;

namespace Psychex.Logic.Helpers
{
    /// <summary>
    /// Table consisting of items of type {TItem}
    /// </summary>
    /// <typeparam name="TItem">Type of items in the table</typeparam>
    public class VirtualTable <TItem>
    {
        public VirtualTable(List<Column> columns)
        {
            Columns = columns;
        }

        public VirtualTable() : this (new List<Column>())
        {
        }

        public List<Column> Columns { get; private set; }

        public class Column
        {
            public string Header { get; set; }

            public Func<TItem, object> DataFetcher { get; set; }
        }
    }
}