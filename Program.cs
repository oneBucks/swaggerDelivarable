using Microsoft.EntityFrameworkCore;
using Company;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CompanyDb>(opt => opt.UseInMemoryDatabase("CompanyData"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "Company";
    config.Title = "Company";
    config.Version = "v3";
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "Company";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/employees", async (CompanyDb db) =>
    await db.Employees.ToListAsync());

app.MapGet("/employees/isManager", async (CompanyDb db) =>
    await db.Employees.Where(t => t.IsManager).ToListAsync());

app.MapGet("/employees/{id}", async (int id, CompanyDb db) =>
    await db.Employees.FindAsync(id)
        is Employee user
            ? Results.Ok(user)
            : Results.NotFound());

app.MapPost("/employees", async (Employee user, CompanyDb db) =>
{
    db.Employees.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/employees/{user.Id}", user);
});

app.MapPut("/employees/{id}", async (int id, Employee inputTodo, CompanyDb db) =>
{
    var todo = await db.Employees.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.fName = inputTodo.fName;
    todo.IsManager= inputTodo.IsManager;
    todo.email=inputTodo.email;
    todo.title=inputTodo.title;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/employees/{id}", async (int id, CompanyDb db) =>
{
    if (await db.Employees.FindAsync(id) is Employee employee)
    {
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});


// /********************************************Department********************************************************************/

app.MapGet("/departments", async (CompanyDb db) =>
    await db.Departments.ToListAsync());



app.MapGet("/departments/{id}", async (int id, CompanyDb db) =>
    await db.Departments.FindAsync(id)
        is Department department
            ? Results.Ok(department)
            : Results.NotFound());

app.MapPost("/departments", async (Department department, CompanyDb db) =>
{
    db.Departments.Add(department);
    await db.SaveChangesAsync();

    return Results.Created($"/departments/{department.Id}", department);
});

app.MapPut("/departments/{id}", async (int id, Department inputDepartments, CompanyDb db) =>
{
    var department = await db.Departments.FindAsync(id);

    if (department is null) return Results.NotFound();

    department.name = inputDepartments.name;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/departments/{id}", async (int id, CompanyDb db) =>
{
    if (await db.Departments.FindAsync(id) is Department department)
    {
        db.Departments.Remove(department);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

/*******************************************CAR********************************************/


app.MapGet("/cars", async (CompanyDb db) =>
    await db.Cars.ToListAsync());

app.MapGet("/cars/isNew", async (CompanyDb db) =>
    await db.Cars.Where(t => t.IsNew).ToListAsync());

app.MapGet("/cars/{id}", async (int id, CompanyDb db) =>
    await db.Cars.FindAsync(id)
        is Car car
            ? Results.Ok(car)
            : Results.NotFound());

app.MapPost("/cars", async (Car car, CompanyDb db) =>
{
    db.Cars.Add(car);
    await db.SaveChangesAsync();

    return Results.Created($"/users/{car.Id}", car);
});

app.MapPut("/cars/{id}", async (int id, Car inputCar, CompanyDb db) =>
{
    var car = await db.Cars.FindAsync(id);

    if (car is null) return Results.NotFound();

    car.brand = inputCar.brand;
    car.model = inputCar.model;
    car.IsNew = inputCar.IsNew;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/cars/{id}", async (int id, CompanyDb db) =>
{
    if (await db.Cars.FindAsync(id) is Car car)
    {
        db.Cars.Remove(car);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});


app.Run();