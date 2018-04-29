using Grob.Entities.Grob;
using Grob.Master.Models;
using Grob.ServiceFabric.Scheduler.TaskRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public static class SchedulerFactory
    {
        public static BaseScheduleRunner GetRunner(GrobTask grobTask, IGrobMasterService grobMasterService)
        {
            switch (grobTask.ScheduleType)
            {
                case ScheduleTypesEnum.Timer:
                    return new TimerSchedulerImpl(grobTask, grobMasterService);
                case ScheduleTypesEnum.WebHost:
                    return new WebHostScheduler(grobTask, grobMasterService);
                case ScheduleTypesEnum.WebTrigger:
                    return new WebTriggerScheduler(grobTask, grobMasterService);
            }

            return null;
        }
    }
}
