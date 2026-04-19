using System;
using System.Collections.Generic;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;

public partial class T_BUSINESS_MODEL
{
    public Guid ID { get; set; }

    public string? NAME { get; set; }

    public Guid? REQUIREMENT_ID { get; set; }

    public DateTime CREATED_UTC { get; set; }

    public virtual T_REQUIREMENT? REQUIREMENT { get; set; }

    public virtual ICollection<T_BUSINESS_MODEL_PROPERTY> T_BUSINESS_MODEL_PROPERTies { get; set; } = new List<T_BUSINESS_MODEL_PROPERTY>();
}
