using Microsoft.EntityFrameworkCore;
using TodoList;
using Microsoft.OpenApi.Models;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using AuthServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthServer.CustomAuth;


var builder = WebApplication.CreateBuilder(args); 

builder.Services.Configure<Application>(builder.Configuration.GetSection(nameof(Application)));

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationHandler>();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
        Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()  
              .AllowAnyMethod()  
              .AllowAnyHeader(); 
    });
});

//builder.Services.AddSingleton<ToDoDbContext>();

builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"), 
                     Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.0-mysql")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; 
    });
}

app.MapGet("/", () => "Welcome to the manager tasks✏️");

app.MapGet("/tasks", async (ToDoDbContext dbContext) =>
{
    var tasks = await dbContext.Items.ToListAsync();
    return Results.Ok(tasks);
});

app.MapPost("/tasks", async (Item item, ToDoDbContext dbContext) =>
{
    dbContext.Items.Add(item);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/tasks/{item.Id}", item); 
});

app.MapPut("/tasks/{id}", async (int id, IsCompleteRequest request, ToDoDbContext dbContext) =>
{
    var item = await dbContext.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }

    item.IsComplete = request.IsComplete; 
    
    await dbContext.SaveChangesAsync();
    return Results.Ok(item); 
});

app.MapDelete("/tasks/{id}", async (int id, ToDoDbContext dbContext) =>
{
    var item = await dbContext.Items.FindAsync(id);
    if (item == null)
    {
        return Results.NotFound();
    }
    
    dbContext.Items.Remove(item);
    await dbContext.SaveChangesAsync();
    return Results.NoContent(); 
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run(); 
public class IsCompleteRequest
{
    public bool IsComplete { get; set; }
}

// using Microsoft.EntityFrameworkCore;
// using TodoApi;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using AuthServer.CustomAuth;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.HttpOverrides;
// using Microsoft.OpenApi.Models;
// using System.IdentityModel.Tokens.Jwt;
// using AuthServer;
// //
// var builder = WebApplication.CreateBuilder(args);
// builder.Services.Configure<Application>(builder.Configuration.GetSection(nameof(Application)));

// builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationHandler>();

// JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// builder.Services.AddControllers();

// builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddSwaggerGen(options =>
// {
//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Scheme = "Bearer",
//         BearerFormat = "JWT",
//         In = ParameterLocation.Header,
//         Name = "Authorization",
//         Description = "Bearer Authentication with JWT Token",
//         Type = SecuritySchemeType.Http
//     });
//     options.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//         Reference = new OpenApiReference
//                 {
//                     Id = "Bearer",
//                     Type = ReferenceType.SecurityScheme
//                 }
//             },
//             new List<string>()
//         }
//     });
// });

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["JWT:Issuer"],
// ValidAudience = builder.Configuration["JWT:Audience"], // קהל היעד של הטוקן
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
//         };
//     });

// var devCorsPolicy = "devCorsPolicy";

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(devCorsPolicy, builder => {
//         builder.AllowAnyOrigin()
//         .AllowAnyMethod().
//         AllowAnyHeader();

//     });
// });

// builder.Services.AddDbContext<ToDoDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
//                      Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.0-mysql")));


// var app = builder.Build();

// app.UseCors(devCorsPolicy);
// app.MapGet("/", () => "Welcome to the manager tasks✏️");

// app.MapGet("/tasks", async (ToDoDbContext dbContext) =>
// {
//     var tasks = await dbContext.Items.ToListAsync();

//     return Results.Ok(tasks);
// });

// app.MapPost("/tasks",async (ToDoDbContext dbContext,Item taskDto) =>
// {
//     dbContext.Items.Add(taskDto);
//     await dbContext.SaveChangesAsync();
//     return Results.Ok(taskDto);
// });
//  app.MapPut("/tasks/{id}", async (ToDoDbContext dbContext,int id,Item taskDto) =>
// {
//     var items = await dbContext.Items.FindAsync(id);
//     // var items =  dbContext.Items.First(a => a.Id == id);
//     if(taskDto.Name!=null)
//       items.Name = taskDto.Name;
//       if(taskDto.IsComplete!=null)
//       items.IsComplete=taskDto.IsComplete;
//      dbContext.SaveChanges();
//     return Results.Ok(new { message = $"המשימה '{items.Name}' עודכנה בהצלחה"});
// });
//      app.MapDelete("/tasks/{id}",async (ToDoDbContext dbContext,int id) =>
// {
//     var task = await dbContext.Items.FindAsync(id);

//     if (task == null)
//     {
//         return Results.NotFound(new { message = "משימה לא נמצאה" });
//     }
//      dbContext.Items.Remove(task);
//     await dbContext.SaveChangesAsync();
//     return Results.Ok(new { message = $"המשימה '{task.Name}' נמחקה בהצלחה" });
// }); 


// // Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger(); 

//     app.UseSwaggerUI(options =>
//     {
//         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
//         options.RoutePrefix = string.Empty; 
//     });
// }
// app.UseHttpsRedirection();


// app.UseForwardedHeaders(new ForwardedHeadersOptions
// {
//     ForwardedHeaders = ForwardedHeaders.XForwardedFor |
//     ForwardedHeaders.XForwardedProto
// });
// app.UseAuthentication();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();