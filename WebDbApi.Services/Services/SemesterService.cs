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
    public class SemesterService
    {
        private readonly ScheduleContext _context;
        public SemesterService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Semester>>> GetAllSemesters()
        {
            var res = await _context.Semesters
                    .Include(s => s.Weeks) // Загружаем связанные Weeks
                        .ThenInclude(w => w.Days) // Загружаем Days для каждого Week
                            .ThenInclude(d => d.DayOfWeek) // Загружаем DayOfWeek для каждого Day
                    .Include(s => s.Weeks)
                        .ThenInclude(w => w.Days)
                            .ThenInclude(d => d.Classes) // Загружаем Classes для каждого Day
                                .ThenInclude(c => c.Teacher) // Загружаем Teacher для каждого Class
                    .Include(s => s.Weeks)
                        .ThenInclude(w => w.Days)
                            .ThenInclude(d => d.Classes)
                                .ThenInclude(c => c.Subject) // Загружаем Subject для каждого Class
                    .ToListAsync();

            if (res == null)
                return new ServiceResult<List<Semester>>
                {
                    NotFound = true,
                    Error = "Таблица пуста"
                };
            return new ServiceResult<List<Semester>>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<Semester>> CreateNewSemester(Semester semester)
        {
            await _context.Semesters.AddAsync(semester);
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<Semester>
                {
                    Ok = true,
                    Result = semester
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Semester>
                {
                    InternalServerError = true,
                    Error = ex.Message
                };
            }
        }

        public async Task<ServiceResult<Semester>> UpdateSemester(Semester semester)
        {
            try
            {
                _context.Semesters.Update(semester);
                await _context.SaveChangesAsync();
                return new ServiceResult<Semester>
                {
                    Ok = true,
                    Result = semester
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Semester>
                {
                    InternalServerError = true,
                    Error = "Ошибка при обновлении группы"
                };
            }
        }

        public async Task<ServiceResult<bool>> DeleteSemester(Semester semester)
        {
            try
            {
                _context.Semesters.Remove(semester);
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
