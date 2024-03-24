using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAuditable
    {
        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }

        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
