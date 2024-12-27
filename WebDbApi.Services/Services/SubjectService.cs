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
    public class SubjectService
    {
        private readonly ScheduleContext _context;
        public SubjectService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Subject>>> GetAllSubjects()
        {
            var res = await _context.Subjects
                .Include(s => s.Classes) // Загрузка связанных Classes
                    .ThenInclude(c => c.Teacher) // Загрузка Teacher для каждого Class
                .Include(s => s.Classes)
                    .ThenInclude(c => c.Group) // Загрузка Group для каждого Class
                .Include(s => s.Classes)
                    .ThenInclude(c => c.Room) // Загрузка Room для каждого Class
                .Include(s => s.Classes)
                    .ThenInclude(c => c.Day) // Загрузка Day для каждого Class
                        .ThenInclude(d => d.DayOfWeek) // Загрузка DayOfWeek через Day
                .Include(s => s.Classes)
                    .ThenInclude(c => c.ClassTime) // Загрузка ClassTime для каждого Class
                .ToListAsync();

            if (res == null)
                return new ServiceResult<List<Subject>>
                {
                    NotFound = true,
                    Error = "Таблица пуста"
                };
            return new ServiceResult<List<Subject>>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<Subject>> CreateNewSubject(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<Subject>
                {
                    Ok = true,
                    Result = subject
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Subject>
                {
                    InternalServerError = true,
                    Error = ex.Message
                };
            }
        }

        public async Task<ServiceResult<Subject>> UpdateSubject(Subject subject)
        {
            try
            {
                _context.Subjects.Update(subject);
                await _context.SaveChangesAsync();
                return new ServiceResult<Subject>
                {
                    Ok = true,
                    Result = subject
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Subject>
                {
                    InternalServerError = true,
                    Error = "Ошибка при обновлении группы"
                };
            }
        }

        public async Task<ServiceResult<bool>> DeleteSubject(Subject subject)
        {
            try
            {
                _context.Subjects.Remove(subject);
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
