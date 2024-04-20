﻿using System.ComponentModel.DataAnnotations;

namespace AnimalAPI.DTOs;

public record ReplaceAnimalRequest(
    [Required][MaxLength(200)] string Name,
    [MaxLength(200)] string Description,
    [Required][MaxLength(200)] string Category,
    [Required][MaxLength(200)] string Area
    );