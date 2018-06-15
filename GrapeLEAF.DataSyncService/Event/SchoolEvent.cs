using System;

namespace GrapeLEAF.DataSyncService.Event
{
    public class SchoolEvent
    {
        public Guid SchoolId { get; set; }

        public SchoolEvent(Guid schoolId) => SchoolId = schoolId;
    }
}
