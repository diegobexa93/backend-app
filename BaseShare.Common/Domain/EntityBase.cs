﻿namespace BaseShare.Common.Domain
{
    public abstract class EntityBase
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
