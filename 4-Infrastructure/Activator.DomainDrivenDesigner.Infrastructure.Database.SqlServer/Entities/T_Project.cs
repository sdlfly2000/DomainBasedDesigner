using System;
using System.Collections.Generic;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;

public partial class T_PROJECT
{
    public Guid ID { get; set; }

    public string NAME { get; set; } = null!;

    public string? DESCRIPTION { get; set; }

    public DateTime CREATED_UTC { get; set; }

    public virtual ICollection<T_REQUIREMENT> T_REQUIREMENTs { get; set; } = new List<T_REQUIREMENT>();
}
