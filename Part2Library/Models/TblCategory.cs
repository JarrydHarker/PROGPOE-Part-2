using System;
using System.Collections.Generic;

namespace Part2Library.Models;

public partial class TblCategory
{
    public string CategoryId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();
}
