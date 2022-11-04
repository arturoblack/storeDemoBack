using Microsoft.EntityFrameworkCore;
using otroProyecto5.Middlewares;
using ReservasDAL.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string tiendaCs = builder.Configuration.GetConnectionString("tiendaDb");
//builder.Services.AddDbContext<ReservasContext>(
//    options => options.UseMySql(reservasConec, ServerVersion.AutoDetect(reservasConec))
//);
builder.Services.AddDbContext<ReservasContext>(
    options => options.UseNpgsql(tiendaCs,
        b => b.MigrationsAssembly("otroProyecto5"))
);
// CORS configuration
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
// expone los archivos en la carpeta
// wwwroot a traves del sitio web
// para que C:\x\y\z\wwwroot  -> http://misitio.com/
// C:\x\y\z\wwwroot\hola.png -> http://misitio.com/hola.png
// C:\x\y\z\wwwroot\carpeta\otro.png -> http://misitio.com/carpeta/otro.png
app.UseStaticFiles();
app.MapControllers();
app.UseMiddleware<JwtLoadTokenDataMiddleware>();
app.Run();

