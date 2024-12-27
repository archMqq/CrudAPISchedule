using Microsoft.EntityFrameworkCore;
using Schedule.Models;
using Schedule.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDbApi.ServiceClasses;

namespace WebDbApi.Services.Services
{
    public class WeekService
    {
        private readonly ScheduleContext _context;
        public WeekService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Week>>> GetAllWeeks()
        {
            var res = await _context.Weeks
                 .Include(w => w.Days) // Загрузка списка Days
                     .ThenInclude(d => d.DayOfWeek) // Загрузка DayOfWeek для каждого Day
                 .ToListAsync();

            if (res == null)
                return new ServiceResult<List<Week>>
                {
                    NotFound = true,
                    Error = "Таблица пуста"
                };
            return new ServiceResult<List<Week>>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<Week>> CreateNewWeek(Week week)
        {
            await _context.Weeks.AddAsync(week);
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<Week>
                {
                    Ok = true,
                    Result = week
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Week>
                {
                    InternalServerError = true,
                    Error = ex.Message
                };
            }
        }

        public async Task<ServiceResult<Week>> UpdateWeek(Week week)
        {
            try
            {
                _context.Weeks.Update(week);
                await _context.SaveChangesAsync();
                return new ServiceResult<Week>
                {
                    Ok = true,
                    Result = week
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Week>
                {
                    InternalServerError = true,
                    Error = "Ошибка при обновлении группы"
                };
            }
        }

        public async Task<ServiceResult<bool>> DeleteWeek(Week week)
        {
            try
            {
                _context.Weeks.Remove(week);
                await _context.SaveChangesAsync();
                return new ServiceResult<bool>
                {
                    Ok = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>
                {
                    InternalServerError = true,
                    Error = "Ошибка при удалении группы"
                };
            }
        }
    }
}
