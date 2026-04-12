using System;
using System.Collections.Generic;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;

public partial class T_REQUIREMENT
{
    public Guid ID { get; set; }

    public string? DESCRIPTION { get; set; }

    public DateTime CREATE_UTC { get; set; }

    public virtual ICollection<T_BUSINESS_MODEL> T_BUSINESS_MODELs { get; set; } = new List<T_BUSINESS_MODEL>();
}
