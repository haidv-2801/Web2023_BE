using Web2023_BE.Domain.Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Entities
{
    public class BaseEntity<TKey>: IRecordKey<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
