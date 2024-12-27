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
    public class QrCodeService
    {
        private readonly ScheduleContext _context;
        public QrCodeService(ScheduleContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<QrCode>> CreateNewQrCode(QrCode qr)
        {
            await _context.QrCodes.AddAsync(qr);
            try
            {
                await _context.SaveChangesAsync();
                return new ServiceResult<QrCode>
                {
                    Ok = true,
                    Result = qr
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<QrCode>
                {
                    InternalServerError = true,
                    Error = ex.Message
                };
            }
        }

        public async Task<ServiceResult<QrCode>> FindQrCode(string code)
        {
            var res = await _context.QrCodes.FirstOrDefaultAsync(x => x.Code == code);
            if (res == null)
                return new ServiceResult<QrCode>
                {
                    NotFound = true,
                    Error = "Такого Qr кода не существует!"
                };

            return new ServiceResult<QrCode>
            {
                Ok = true,
                Result = res
            };
        }

        public async Task<ServiceResult<QrCode>> DeleteQrCode(int id)
        {
            try
            {
                _context.QrCodes.Remove(_context.QrCodes.FirstOrDefault(x => x.Id == id));
                await _context.SaveChangesAsync();
                return new ServiceResult<QrCode>
                {
                    Ok = true,
                    Result = _context.QrCodes.FirstOrDefault(x => x.Id == id)
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<QrCode>
                {
                    InternalServerError = true,
                    Error = "Ошибка при удалении Qr-кода"
                };
            }
        }
    }
}
