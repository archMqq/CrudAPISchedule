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
    public class TeacherService
    {
        private readonly ScheduleContext _context;

        public TeacherService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Teacher>>> GetAllTeachers()
        {
            var res = await _context.Teachers
                     .Include(t => t.Classes)
                         .ThenInclude(c => c.Subject)
                     .Include(t => t.Classes)
                         .ThenInclude(c => c.Group)
                     .Include(t => t.Classes)
                         .ThenInclude(c => c.Room)
                     .Include(t => t.Classes)
                         .ThenInclude(c => c.Day)
                             .ThenInclude(d => d.DayOfWeek)
                     .Include(t => t.Classes)
                         .ThenInclude(c => c.ClassTime)
                     .Where(t => t.IsActive) 
                     .ToListAsync();

            if (res is null)
                return new ServiceResult<List<Teacher>>
                {
                    NotFound = true,
                    Error = "Табдица пуста"
                };
            return new ServiceResult<List<Teacher>>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<Teacher>> CreateNewTeacher(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<Teacher>
                {
                    Ok = true,
                    Result = teacher
                };
            }
            catch (DbUpdateException dbEx)
            {
                return new ServiceResult<Teacher>
                {
                    InternalServerError = true,
                    Error = dbEx.InnerException?.Message ?? dbEx.Message
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Teacher>
                {
                    InternalServerError = true,
                    Error = ex.InnerException?.Message ?? ex.Message
                };
            }
        }


        public async Task<ServiceResult<Teacher>> UpdateTeacher(Teacher teacher)
        {

            if (teacher == null)
            {
                return new ServiceResult<Teacher>
                {
                    InternalServerError = true,
                    Error = "Передан неверный объект для обновления"
                };
            }

            try
            {
                if (string.IsNullOrEmpty(teacher.Name) || string.IsNullOrEmpty(teacher.Surname) || string.IsNullOrEmpty(teacher.Patronymic))
                {
                    return new ServiceResult<Teacher>
                    {
                        InternalServerError = true,
                        Error = "Имя группы не может быть пустым"
                    };
                }

                await _context.Teachers.AddAsync(teacher);
                await _context.SaveChangesAsync();

                return new ServiceResult<Teacher>
                {
                    Ok = true,
                    Result = teacher
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Teacher>
                {
                    InternalServerError = true,
                    Error = $"Ошибка при добавлении группы: {ex.Message}, {ex.InnerException?.Message}"
                };
            }
        }

        public async Task<ServiceResult<bool>> DeleteTeacher(Teacher teacher)
        {
            try
            {
                _context.Teachers.Remove(teacher);
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
