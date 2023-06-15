using System;
using System.Collections.Generic;
using System.Text;

namespace Web2023_BE.Domain.Shared.Entities
{
    public interface IRecordHistory
    {
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
