﻿using System.Text.Json;

namespace VisualVibes.Api.Contracts
{
    public class Error
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
