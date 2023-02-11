using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.Legder.Domain.Shared.Cruds
{
    public class DeleteParameter<TKey>: CrudParameter
    {
        public TKey Id { get; set; }
    }
}
