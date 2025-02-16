using Labb2WebbUtveckling.Data;
using Labb2WebbUtveckling.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Labb2WebbUtveckling
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Konfigurera DbContext med SQLite
            builder.Services.AddDbContext<BlommorDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Lägg till grundläggande tjänster
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Korrekt CORS-konfiguration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowExpressApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

         /*   // Säkerställ att databasen skapas
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BlommorDbContext>();
                db.Database.EnsureCreated();
            }*/

            // Konfigurera HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Viktigt att CORS kommer före andra middleware
            app.UseCors("AllowExpressApp");
            app.UseHttpsRedirection();
            app.UseAuthorization();

            // CRUD Endpoints för blommor

            // CREATE - Skapa en ny blomma
            app.MapPost("/Blommor", async (BlommorDbContext db, Blomma blomma) =>
            {
                db.Blommor.Add(blomma);
                await db.SaveChangesAsync();
                return Results.Created($"/blommor/{blomma.Id}", blomma);
            });

            // READ - Hämta alla blommor
            app.MapGet("/Blommor", async (BlommorDbContext db) =>
            {
                return await db.Blommor.ToListAsync();
            });

            // READ - Hämta en specifik blomma med ID
            app.MapGet("/Blommor/{id}", async (BlommorDbContext db, int id) =>
            {
                var blomma = await db.Blommor.FindAsync(id);
                if (blomma == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(blomma);
            });

            // UPDATE - Uppdatera en befintlig blomma
            app.MapPut("/Blommor/{id}", async (BlommorDbContext db, int id, Blomma updatedBlomma) =>
            {
                var blomma = await db.Blommor.FindAsync(id);
                if (blomma == null)
                {
                    return Results.NotFound();
                }

                blomma.Name = updatedBlomma.Name;
                blomma.Color = updatedBlomma.Color;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE - Ta bort en blomma
            app.MapDelete("/Blommor/{id}", async (BlommorDbContext db, int id) =>
            {
                var blomma = await db.Blommor.FindAsync(id);
                if (blomma == null)
                {
                    return Results.NotFound();
                }

                db.Blommor.Remove(blomma);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.Run();
        }
    }
}