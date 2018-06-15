using System;

namespace GrapeLEAF.DataSyncService.Event
{
    public class CampusEvent
    {
        public Guid CampusId { get; set; }

        public CampusEvent(Guid campusId) => CampusId = campusId;
    }
}
