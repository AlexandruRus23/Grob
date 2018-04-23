using Grob.Entities.Grob;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public static class SchedulerFactory
    {
        public static IScheduleRunner GetScheduler(GrobTask grobTask)
        {
            switch (grobTask.ScheduleType)
            {
                case ScheduleTypesEnum.Timer:
                    return new TimerSchedulerImpl(grobTask);
            }

            return null;
        }
    }
}
