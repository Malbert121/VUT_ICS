using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICS.Messages;

public record SubjectStudentAddMessage
{
    public required Guid SubjectId { get; set; }
}
