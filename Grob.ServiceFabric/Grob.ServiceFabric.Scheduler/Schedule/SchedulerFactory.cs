using Grob.Entities.Grob;
using Grob.Master.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public static class SchedulerFactory
    {
        public static IScheduleRunner GetRunner(GrobTask grobTask, IGrobMasterService grobMasterService)
        {
            switch (grobTask.ScheduleType)
            {
                case ScheduleTypesEnum.Timer:
                    return new TimerSchedulerImpl(grobTask, grobMasterService);
            }

            return null;
        }
    }
}
