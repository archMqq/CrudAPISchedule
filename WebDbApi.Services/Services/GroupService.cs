using Microsoft.EntityFrameworkCore;
using Schedule.Models;
using Schedule.Models.Models;
using WebDbApi.ServiceClasses;

namespace WebDbApi.Services.Services
{
    public class GroupService
    {
        private readonly ScheduleContext _context;

        public GroupService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Group>>> GetAllGroups()
        {
            var res = await _context.Groups.ToListAsync();

            if (res == null)
                return new ServiceResult<List<Group>>
                {
                    NotFound = true,
                    Error = "Таблица пуста"
                };
            return new ServiceResult<List<Group>>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<Group>> CreateNewGroup(Group group)
        {
            if (group == null)
            {
                return new ServiceResult<Group>
                {
                    InternalServerError = true,
                    Error = "Передан неверный объект для обновления"
                };
            }

            try
            {
                if (string.IsNullOrEmpty(group.Name))
                {
                    return new ServiceResult<Group>
                    {
                        InternalServerError = true,
                        Error = "Имя группы не может быть пустым"
                    };
                }

                await _context.Groups.AddAsync(group);
                await _context.SaveChangesAsync();

                return new ServiceResult<Group>
                {
                    Ok = true,
                    Result = group
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Group>
                {
                    InternalServerError = true,
                    Error = $"Ошибка при добавлении группы: {ex.Message}, {ex.InnerException?.Message}"
                };
            }
        }


        public async Task<ServiceResult<Group>> UpdateGroupNumber(int groupId, string groupNumber)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
                return new ServiceResult<Group>
                {
                    NotFound = true,
                    Error = "Группы с указанным номером не существует!"
                };
            group.Name = groupNumber;
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<Group>
                {
                    Ok = true,
                    Result = group
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Group>
                {
                    InternalServerError = true,
                    Error = ex.Message
                };
            }
        }

        public async Task<ServiceResult<Group>> UpdateGroup(Group group)
        {
            if (group == null)
            {
                return new ServiceResult<Group>
                {
                    InternalServerError = true,
                    Error = "Передан неверный объект для обновления"
                };
            }

            try
            {
                _context.Groups.Update(group);
                await _context.SaveChangesAsync();
                return new ServiceResult<Group>
                {
                    Ok = true,
                    Result = group
                };
            }
            catch(Exception ex)
            {
                return new ServiceResult<Group>
                {
                    InternalServerError = true,
                    Error = $"Ошибка при обновлении группы {ex.Message}"
                };
            }
        }
        public async Task<ServiceResult<bool>> DeleteGroup(Group group)
        {
            try
            {
                _context.Groups.Remove(group);
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
