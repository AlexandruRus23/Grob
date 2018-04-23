using System;
using System.Collections.Generic;
using System.Text;

namespace Grob.ServiceFabric.Scheduler.Schedule
{
    public class CronCalculator
    {
        private string _expression;

        public CronCalculator(string cronExpression)
        {
            _expression = cronExpression;
        }

        //public DateTime GetNextExecutionTime()
        //{
        //    return null;
        //}
    }
}
