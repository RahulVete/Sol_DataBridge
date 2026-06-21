using Sol_DataBridge.Data;
using Sol_DataBridge.Repositories;
using Sol_DataBridge.Repositories.Interfaces;
using Sol_DataBridge.Services;
using Sol_DataBridge.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IJsonStreamingParserService, JsonStreamingParserService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<ISqlConnectionFactory,
                           SqlConnectionFactory>();

builder.Services.AddScoped<IImportRepository,
                           ImportRepository>();

builder.Services.AddScoped<IBulkCopyService,
                           BulkCopyService>();

builder.Services.AddScoped<
    IValidationService,
    ValidationService>();

builder.Services.AddScoped<
    IErrorLoggerService,
    ErrorLoggerService>();

builder.Services.AddScoped<
    IImportStatusService,
    ImportStatusService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();