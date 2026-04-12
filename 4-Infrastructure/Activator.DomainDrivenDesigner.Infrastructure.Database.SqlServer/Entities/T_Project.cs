using System;
using System.Collections.Generic;

namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Entities;

public partial class T_PROJECT
{
    public Guid ID { get; set; }

    public string NAME { get; set; } = null!;

    public string? DESCRIPTION { get; set; }

    public DateTime CREATED_UTC { get; set; }
}
