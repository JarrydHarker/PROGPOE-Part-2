using System;
using System.Collections.Generic;

namespace Part2Library.Models;

public partial class TblProduct
{
    public string ProductId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? CategoryId { get; set; }

    public DateOnly ProductionDate { get; set; }

    public virtual TblCategory? Category { get; set; }

    public virtual TblUser User { get; set; } = null!;
}
