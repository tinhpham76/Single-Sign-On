using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManager.Services
{
    public interface IDateTracking
    {
        DateTime CreateDate { get; set; }
        DateTime? LastModifiedDate { get; set; }
    }
}
