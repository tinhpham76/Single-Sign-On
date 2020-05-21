using System;

namespace SSO.Backend.Services
{
    public interface IDateTracking
    {
        DateTime CreateDate { get; set; }
        DateTime? LastModifiedDate { get; set; }
    }
}
