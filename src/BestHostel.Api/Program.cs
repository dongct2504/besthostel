using BestHostel.Application;
using BestHostel.Infrastructure;
using BestHostel.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    // builder.Services.AddControllers().AddJsonOptions(options =>
    //     options.JsonSerializerOptions.PropertyNamingPolicy = null);

    // allows for the correct parsing of Patch document using NewtonsoftJson
    builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

    // setting connection string and register DbContext
    var sqlConnectionStringBuilder = new SqlConnectionStringBuilder
    {
        ConnectionString = builder.Configuration.GetConnectionString("BestHostelSqlConnection"),
        UserID = builder.Configuration["UserID"],
        Password = builder.Configuration["Password"]
    };

    builder.Services.AddDbContext<BestHostelDbContext>(options =>
        options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString));

    // register automapper
    builder.Services.AddAutoMapper(typeof(Program));

    // register dependencies from application and infrastructure
    builder.Services
        .AddApplication()
        .AddInfrastructure();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

}

app.Run();
