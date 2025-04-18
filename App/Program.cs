using App.Application.Service.Concrete;
using App.Application.Service.Contracts;
using App.Persistence.Configuration;
using App.Persistence.Contexts;
using App.Persistence.Endpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateSlimBuilder(args);

var services = builder.Services;

//Configure Service Extension
services.ConfigureSettings(builder.Configuration);

services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

services.AddEndpointsApiExplorer();
services.AddOpenApi();

#region Singleton
builder.Services.AddSingleton<Fluid.FluidParser>();
builder.Services.AddSingleton<Fluid.TemplateOptions>();
services.AddSingleton<IRabbitMqService, RabbitMqService>();
services.AddSingleton<IRabbitMqService, RabbitMqService>();
services.AddSingleton<IRabbitMqPublishService, RabbitMqPublishService>();
services.AddSingleton<IRabbitMqConsumerService, RabbitMqConsumerService>();
services.AddSingleton<ISmtpService, SmtpService>();
#endregion

#region Scoped
services.AddScoped<ITemplateEngine, TemplateEngine>();
services.AddScoped<IMailService, MailService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapMailSendEndpoints();

app.Run();



