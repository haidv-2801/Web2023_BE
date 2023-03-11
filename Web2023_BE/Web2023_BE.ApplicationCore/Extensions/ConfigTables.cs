using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace Web2023_BE.ApplicationCore.Extensions
{
    public class ConfigTables : Attribute
    {
        public bool HasDeletedColumn { get; set; } = false;

        public string UniqueColumns { get; set; } = string.Empty;

        public ConfigTables(bool hasDeletedColumn = false, string uniqueColumns = "")
        {
            HasDeletedColumn = hasDeletedColumn;

            UniqueColumns = uniqueColumns;
        }
    }
}
