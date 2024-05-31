using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHubs.Idams.Maintenance.Core.Services;

public interface ICorrelationService
{
    public string CorrelationId { get; }
}

public class CorrelationService : ICorrelationService
{
    public CorrelationService()
    {
        CorrelationId = Guid.NewGuid().ToString();
    }

    public string CorrelationId { get; }
}