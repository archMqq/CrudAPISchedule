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
    public class ClassService
    {
        private readonly ScheduleContext _context;
        public ClassService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Class>>> GetAllClasses()
        {
            var res = await _context.Classes
                .Include(c => c.Teacher)
                .Include(c => c.Group)
                .Include(c => c.Subject)
                .Include(c => c.Room)
                .Include(c => c.Day)
                    .ThenInclude(c => c.DayOfWeek)
                .Include(c => c.ClassTime)
                .ToListAsync();

            if (res == null)
                return new ServiceResult<List<Class>>
                {
                    NotFound = true,
                    Error = "Таблица пуста"
                };
            return new ServiceResult<List<Class>>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<Class>> CreateNewClass(Class clas)
        {
            await _context.Classes.AddAsync(clas);
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<Class>
                {
                    Ok = true,
                    Result = clas
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Class>
                {
                    InternalServerError = true,
                    Error = ex.Message
                };
            }
        }

        public async Task<ServiceResult<Class>> UpdateClass(Class clas)
        {
            try
            {
                _context.Classes.Update(clas);
                await _context.SaveChangesAsync();
                return new ServiceResult<Class>
                {
                    Ok = true,
                    Result = clas
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Class>
                {
                    InternalServerError = true,
                    Error = "Ошибка при обновлении группы"
                };
            }
        }

        public async Task<ServiceResult<List<Class>>> UpdateClassesAsList(List<Class> classes)
        {
            try
            {
                foreach (var clas in classes)
                {
                    var existingClass = await _context.Classes.FindAsync(clas.Id);
                    if (existingClass != null)
                    {
                        _context.Entry(existingClass).CurrentValues.SetValues(clas);
                    }
                    else
                    {
                        _context.Classes.Add(clas);
                    }
                }

                await _context.SaveChangesAsync();

                return new ServiceResult<List<Class>>
                {
                    Ok = true,
                    Result = (await GetAllClasses()).Result ?? classes
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<Class>>
                {
                    InternalServerError = true,
                    Error = "Ошибка при обновлении группы: " + ex.Message
                };
            }
        }

        public async Task<ServiceResult<bool>> DeleteClass(Class clas)
        {
            try
            {
                _context.Classes.Remove(clas);
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
