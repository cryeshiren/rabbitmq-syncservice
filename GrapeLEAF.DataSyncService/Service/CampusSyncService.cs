using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrapeLEAF.DataSyncService.Service
{
    public class CampusSyncService
    {
        [Subscribe("campus", Group = "campus")]
        public void CreateCampus()
        {
            throw new NotImplementedException();
        }
    }
}
