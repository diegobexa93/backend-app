﻿using System.ComponentModel.DataAnnotations;

namespace BaseShare.Common.Domain
{
    public sealed class MessageBrokerLogSettings
    {
        public const string ConfigurationSection = "MessageBrokerLog";

        [Required, Url]
        public string BaseAddress { get; init; } = string.Empty;

        public string AccessToken { get; init; } = string.Empty;

        public string UserAgent { get; init; } = string.Empty;
    }
}
