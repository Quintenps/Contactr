using System;
using Contactr.Models.Enums;

namespace Contactr.Models.Connection.Resources
{
    public abstract class Resource
    {
        public Guid Id { get; set; }
        public SyncProviders Provider { get; set; }
    }
}