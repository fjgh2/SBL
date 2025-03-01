﻿using System.Text.Json.Serialization;

namespace SBL.Domain.Entities;

public class Feedback
{
    public int Id { get; set; }
    
    public int Rating { get; set; }
    
    public string Comment { get; set; }
    
    public int ProductId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    [JsonIgnore]
    public Product Product { get; set; }
}
