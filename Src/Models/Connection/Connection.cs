using System;
using Contactr.Models.Connection.Resources;

namespace Contactr.Models.Connection
{
    public class Connection
    {
        public Guid Id { get; set; }
        public Resource Resource { get; set; }
        public Guid SenderUserId { get; set; }
        public virtual User SenderUser { get; set; }
        public Guid ReceiverUserId { get; set; }
        public virtual User ReceiverUser { get; set; }
        public DateTime SynchronizedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}