using Schedule.Models.Models;
using Schedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDbApi.ServiceClasses;
using Microsoft.EntityFrameworkCore;

namespace WebDbApi.Services.Services
{
    public class DayOfWeekService
    {
        private readonly ScheduleContext _context;
        public DayOfWeekService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Schedule.Models.Models.DayOfWeek>>> GetAllDays()
        {
            var res = await _context.DayOfWeeks.ToListAsync();

            if (res == null)
                return new ServiceResult<List<Schedule.Models.Models.DayOfWeek>>
                {
                    NotFound = true,
                    Error = "Таблица пуста"
                };
            return new ServiceResult<List<Schedule.Models.Models.DayOfWeek>>
            {
                Ok = true,
                Result = res
            };
        }
    }
}
