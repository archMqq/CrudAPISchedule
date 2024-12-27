using Microsoft.EntityFrameworkCore;
using Schedule.Models;
using WebDbApi.Services.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ScheduleContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<GroupService>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<DayService>();
builder.Services.AddScoped<DayOfWeekService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<SemesterService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<WeekService>();
builder.Services.AddScoped<QrCodeService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
