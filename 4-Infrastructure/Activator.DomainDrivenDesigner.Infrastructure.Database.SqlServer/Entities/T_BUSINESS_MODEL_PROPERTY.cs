using System;
using System.Collections.Generic;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;

public partial class T_BUSINESS_MODEL_PROPERTY
{
    public Guid ID { get; set; }

    public string? NAME { get; set; }

    public DateTime CREATED_UTC { get; set; }

    public byte? TYPE { get; set; }

    public Guid? BUSINESS_MODEL_ID { get; set; }

    public virtual T_BUSINESS_MODEL? BUSINESS_MODEL { get; set; }
}
