using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using CatalogoFilmes.API.Data;
using CatalogoFilmes.API.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Catálogo de Filmes API", 
        Version = "v1",
        Description = "API para gerenciamento de catálogo de filmes"
    });
});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                     "Data Source=catalogo_filmes.db"));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catálogo de Filmes API v1");
        c.RoutePrefix = "swagger";
    });
}


app.UseCors("AllowAll");


app.Use(async (context, next) =>
{
    Console.WriteLine($"🌐 {context.Request.Method} {context.Request.Path} - Origin: {context.Request.Headers["Origin"]}");
    await next();
    Console.WriteLine($"✅ Response: {context.Response.StatusCode}");
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        context.Database.EnsureCreated();
        
        
        if (!context.Movies.Any())
        {
            var filmesTeste = new List<Movie>
            {
                new Movie
                {
                    Titulo = "Vingadores: Ultimato",
                    Ano = 2019,
                    Genero = "Ação/Aventura",
                    Diretor = "Anthony e Joe Russo",
                    Sinopse = "Os heróis mais poderosos da Terra enfrentam Thanos em uma batalha épica pelo destino do universo.",
                    Avaliacao = 9,
                    DataLancamento = new DateTime(2019, 4, 26),
                    Duracao = 181,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                },
                new Movie
                {
                    Titulo = "Parasita",
                    Ano = 2019,
                    Genero = "Drama/Thriller",
                    Diretor = "Bong Joon-ho",
                    Sinopse = "Uma família pobre se infiltra na vida de uma família rica com consequências inesperadas.",
                    Avaliacao = 10,
                    DataLancamento = new DateTime(2019, 5, 21),
                    Duracao = 132,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                },
                new Movie
                {
                    Titulo = "Coringa",
                    Ano = 2019,
                    Genero = "Drama/Crime",
                    Diretor = "Todd Phillips",
                    Sinopse = "A origem sombria do icônico vilão do Batman em Gotham City.",
                    Avaliacao = 8,
                    DataLancamento = new DateTime(2019, 10, 4),
                    Duracao = 122,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                }
            };
            
            context.Movies.AddRange(filmesTeste);
            context.SaveChanges();
            Console.WriteLine($"✅ {filmesTeste.Count} filmes de teste adicionados!");
        }
        
        Console.WriteLine("✅ Banco de dados criado com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Erro ao criar banco de dados: {ex.Message}");
    }
}

Console.WriteLine("🚀 API do Catálogo de Filmes iniciada!");
Console.WriteLine($"📖 Swagger UI: http://localhost:5042/swagger");
Console.WriteLine($"🌐 API Base: http://localhost:5042/api");
Console.WriteLine($"🧪 Teste: http://localhost:5042/api/movies/test");
Console.WriteLine($"🎬 Filmes: http://localhost:5042/api/movies");

app.Run();
