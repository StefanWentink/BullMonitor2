using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullMonitor.Ticker.Api.Abstractions.Responses
{
    public record CurrecyListResponse(
        Guid Id,
        string Code,
        string Name)
    { }
}