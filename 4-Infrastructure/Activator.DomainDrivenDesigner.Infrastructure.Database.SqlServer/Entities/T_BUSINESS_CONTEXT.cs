using System;
using System.Collections.Generic;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;

public partial class T_BUSINESS_CONTEXT
{
    public Guid ID { get; set; }

    public string? NAME { get; set; }

    public DateTime CREATED_UTC { get; set; }

    public Guid? T_PROJECT_ID { get; set; }

    public virtual ICollection<T_BUSINESS_ACTION> T_BUSINESS_ACTIONs { get; set; } = new List<T_BUSINESS_ACTION>();

    public virtual ICollection<T_BUSINESS_MODEL> T_BUSINESS_MODELs { get; set; } = new List<T_BUSINESS_MODEL>();

    public virtual T_PROJECT? T_PROJECT { get; set; }
}
