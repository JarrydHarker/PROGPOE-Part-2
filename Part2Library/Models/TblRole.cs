using System;
using System.Collections.Generic;

namespace Part2Library.Models;

public partial class TblRole
{
    public string RoleId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();
}
