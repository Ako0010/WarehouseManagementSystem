using WarehouseManagementSystem.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwagger()
                .AddFluentValidation()
                .AddWarehouseDbContext(builder.Configuration)
                .AddIdentityAndDb(builder.Configuration)
                .AddJwtAuthenticationAndAuthorization(builder.Configuration)
                .AddCorsPolicy()
                .AddAutoMapperAndOtherServices();

var app = builder.Build();

app.UseWarehouseManagementPipeline();

await app.EnsureRolesSeededAsync();

app.Run();
