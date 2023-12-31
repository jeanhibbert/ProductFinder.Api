﻿using System.Text.Json.Serialization;

namespace ProductFinder.Api.Models;

public enum ProductColor : int
{
    White = 0, Green = 1, Blue = 2
}

public record Product
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; } = Guid.NewGuid();

    [JsonPropertyName("productName")]
    public string ProductName { get; init; } = default!;

    [JsonPropertyName("productColor")]
    public ProductColor ProductColor { get; init; } = ProductColor.White;
}