using System;
using System.Collections.Generic;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;

public partial class T_BUSINESS_ACTION
{
    public Guid ID { get; set; }

    public string? NAME { get; set; }

    public string? RAW_DESCRIPTION { get; set; }

    public Guid? CONTEXT_ID { get; set; }

    public DateTime CREATED_UTC { get; set; }

    public Guid? REQUIREMENT_ID { get; set; }

    public Guid? PARENT_BUSINESS_ACTION_ID { get; set; }

    public virtual T_BUSINESS_CONTEXT? CONTEXT { get; set; }

    public virtual T_REQUIREMENT? REQUIREMENT { get; set; }
}
