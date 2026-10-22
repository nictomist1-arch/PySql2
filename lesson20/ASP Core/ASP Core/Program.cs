using college_DB.Data;
using college_DB.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CollegeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/students/all", async (CollegeDBContext db) =>
    Results.Ok(await db.Students.AsNoTracking().ToListAsync()));

app.MapGet("/groups/all", async (CollegeDBContext db) =>
    Results.Ok(await db.Groups.AsNoTracking().ToListAsync()));

app.MapPost("/students/add", async (Student student, CollegeDBContext db) =>
{
    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created("/students/add", new { studentId = student.StudentId });
});

app.MapPost("/groups/add", async (Group group, CollegeDBContext db) =>
{
    db.Groups.Add(group);
    await db.SaveChangesAsync();
    return Results.Created("/groups/add", new { groupId = group.GroupId });
});

app.MapPut("/students/{id:int}", async (int id, Student student, CollegeDBContext db) =>
{
    student.StudentId = id;
    db.Students.Update(student);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/students/{id:int}", async (int id, CollegeDBContext db) =>
{
    await db.Students.Where(s => s.StudentId == id).ExecuteDeleteAsync();
    return Results.NoContent();
});

app.Run();
