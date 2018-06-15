using GrapeLEAF.DataSyncService.Model;
using System;

namespace GrapeLEAF.DataSyncService.Service
{
    public class SchoolSyncService
    {
        [Subscribe("school", Group = "school")]
        public void CreateSchool(SchoolModel school)
        {
            throw new NotImplementedException();
        }
    }
}
