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
    public class DayService
    {
        private readonly ScheduleContext _context;
        public DayService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Day>>> GetAllDays()
        {
            var res = await _context.Days
                .Include(d => d.DayOfWeek)
                .Include(d => d.Week)
                .ThenInclude(x => x.Semester)
                .ToListAsync();

            if (res == null)
                return new ServiceResult<List<Day>>
                {
                    NotFound = true,
                    Error = "Таблица пуста"
                };
            return new ServiceResult<List<Day>>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<Day>> CreateNewDay(Day day)
        {
            await _context.Days.AddAsync(day);
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<Day>
                {
                    Ok = true,
                    Result = day
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Day>
                {
                    InternalServerError = true,
                    Error = ex.Message
                };
            }
        }

        public async Task<ServiceResult<Day>> UpdateDay(Day day)
        {
            try
            {
                _context.Days.Update(day);
                await _context.SaveChangesAsync();
                return new ServiceResult<Day>
                {
                    Ok = true,
                    Result = day
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Day>
                {
                    InternalServerError = true,
                    Error = "Ошибка при обновлении группы"
                };
            }
        }

        public async Task<ServiceResult<bool>> DeleteDay(Day day)
        {
            try
            {
                _context.Days.Remove(day);
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
