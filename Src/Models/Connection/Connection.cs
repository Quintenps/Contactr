﻿using System;
using System.Collections.Generic;
using Contactr.Models.Cards;
using Contactr.Models.Connection.Resources;

namespace Contactr.Models.Connection
{
    public class Connection
    {
        public Guid Id { get; set; }
        public Resource Resource { get; set; } = null!;
        public Guid SenderUserId { get; set; }
        public virtual User SenderUser { get; set; } = null!;
        public Guid ReceiverUserId { get; set; }
        public virtual User ReceiverUser { get; set; } = null!;
        public DateTime? SynchronizedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}