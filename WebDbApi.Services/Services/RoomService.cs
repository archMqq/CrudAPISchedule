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
    public class RoomService
    {
        private readonly ScheduleContext _context;
        public RoomService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<List<Room>>> GetAllRooms()
        {
            var res = await _context.Rooms
    .Include(r => r.RoomType)
    .Include(r => r.Classes)
        .ThenInclude(c => c.Teacher)
    .Include(r => r.Classes)
        .ThenInclude(c => c.Group)
    .Include(r => r.Classes)
        .ThenInclude(c => c.Subject)
    .Include(r => r.Classes)
        .ThenInclude(c => c.Day)
            .ThenInclude(d => d.DayOfWeek)
    .Include(r => r.Classes)
        .ThenInclude(c => c.ClassTime)
    .ToListAsync();

            if (res == null)
                return new ServiceResult<List<Room>>
                {
                    NotFound = true,
                    Error = "Таблица пуста"
                };
            return new ServiceResult<List<Room>>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<Room>> CreateNewRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<Room>
                {
                    Ok = true,
                    Result = room
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Room>
                {
                    InternalServerError = true,
                    Error = ex.Message
                };
            }
        }

        public async Task<ServiceResult<Room>> UpdateRoom(Room room)
        {
            try
            {
                _context.Rooms.Update(room);
                await _context.SaveChangesAsync();
                return new ServiceResult<Room>
                {
                    Ok = true,
                    Result = room
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<Room>
                {
                    InternalServerError = true,
                    Error = "Ошибка при обновлении группы"
                };
            }
        }
        public async Task<ServiceResult<bool>> DeleteRoom(Room room)
        {
            try
            {
                _context.Rooms.Remove(room);
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
