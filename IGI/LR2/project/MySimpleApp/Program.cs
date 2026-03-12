using Microsoft.EntityFrameworkCore;
using MySimpleApp.Data;
using MySimpleApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контекст базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Автоматически создаем базу данных при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Content("""
    <!DOCTYPE html>
    <html>
    <head>
        <title>MySimpleApp API</title>
        <style>
            body { font-family: Arial, sans-serif; margin: 40px; line-height: 1.6; }
            h1 { color: #333; }
            .endpoint { background: #f4f4f4; padding: 10px; border-radius: 5px; margin: 10px 0; }
            code { background: #ddd; padding: 2px 5px; border-radius: 3px; }
            a { color: #0066cc; text-decoration: none; }
            a:hover { text-decoration: underline; }
        </style>
    </head>
    <body>
        <h1> MySimpleApp API is running!</h1>
        <p>Welcome to the API. Server successfully started and connected to the database.</p>
        
        <div class="endpoint">
            <h3> API Documentation</h3>
            <p>Go to <a href="/swagger">Swagger UI</a> to test all endpoints</p>
        </div>
        
        <div class="endpoint">
            <h3> Available endpoints:</h3>
            <ul>
                <li><code>GET /items</code> - get all items</li>
                <li><code>GET /items/{id}</code> - get item by ID</li>
                <li><code>POST /items</code> - create new item</li>
                <li><code>PUT /items/{id}</code> - update item</li>
                <li><code>DELETE /items/{id}</code> - delete item</li>
            </ul>
        </div>
        
        <div class="endpoint">
            <h3> Status:</h3>
            <ul>
                <li> Server: running</li>
                <li> Database: connected</li>
                <li> Swagger: available</li>
            </ul>
        </div>
    </body>
    </html>
    """, "text/html"));

// API эндпоинты (без изменений)
app.MapGet("/items", async (AppDbContext db) => await db.Items.ToListAsync());

app.MapGet("/items/{id}", async (int id, AppDbContext db) =>
    await db.Items.FindAsync(id) is Item item ? Results.Ok(item) : Results.NotFound());

app.MapPost("/items", async (Item item, AppDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();
    return Results.Created($"/items/{item.Id}", item);
});

app.MapPut("/items/{id}", async (int id, Item inputItem, AppDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();
    
    item.Name = inputItem.Name;
    item.IsCompleted = inputItem.IsCompleted;
    
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async (int id, AppDbContext db) =>
{
    var item = await db.Items.FindAsync(id);
    if (item is null) return Results.NotFound();
    
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();