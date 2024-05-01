using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.Messages.SubjectMessages
{
    public record class SubjectEditMessage
    {
        public required Guid SubjectId { get; init; }
    }
}
