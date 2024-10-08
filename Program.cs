using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using minimal_api.DependecyInjection;
using minimal_api.Dominio.Dtos;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Enums;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.ModelViews;
using minimal_api.Dominio.Utils;

var builder = WebApplication.CreateBuilder(args);

DependencyInjection.AddInfraestructure(builder);

var app = builder.Build();

#region Home
app.MapGet("/", () => Results.Json(new Home()))
    .AllowAnonymous()
    .WithTags("Home");
#endregion

#region Administradores
app.MapPost("/administradores/login", ([FromBody] LoginDto dto, IAdministradorServico adminServico) =>
{
    try
    {
        var admin = adminServico.Login(dto);
        if (admin == null)
        {
            return Results.Unauthorized();
        }
        string token = Utils.GerarTokenJwt(admin, builder.Configuration.GetSection("Jwt").Value);
        return Results.Ok(new
        {
            admin.Email,
            admin.Perfil,
            Token = token,
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.ToString());
    }
})
.AllowAnonymous()
.WithTags("Administradores");

app.MapGet("/administradores/{id}", (int id, IAdministradorServico administradorServico) =>
{
    try
    {
        var admin = administradorServico.FindOneById(id);
        if (admin == null)
            return Results.NotFound();
        return Results.Ok(new AdministradorDto.BodyResponse
        {
            Email = admin.Email,
            Perfil = admin.Perfil == "Adm" ? Perfil.Adm : Perfil.Editor
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute
{
    Roles = "Adm"
}).WithTags("Administradores");
app.MapGet("/administradores", (IAdministradorServico administradorServico) =>
{
    try
    {
        var admins = administradorServico.FindAll();
        return Results.Ok(admins.Select(adm => new AdministradorDto.BodyResponse
        {
            Email = adm.Email,
            Perfil = adm.Perfil == "Adm" ? Perfil.Adm : Perfil.Editor
        }));
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute
{
    Roles = "Adm"
})
.WithTags("Administradores");

app.MapPost("/administradores", ([FromBody] AdministradorDto.BodyRequest admDto, IAdministradorServico adminServico) =>
{
    try
    {
        var admin = new Administradores
        {
            Email = admDto.Email,
            Senha = admDto.Senha,
            Perfil = admDto.Perfil?.ToString() ?? Perfil.Editor.ToString()
        };
        adminServico.Add(admin);
        return Results.Created($"/administradores/{admin.Id}", admin);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute
{
    Roles = "Adm"
})
.WithTags("Administradores");
#endregion

#region Administradores
app.MapGet("/veiculos", (IVeiculoServico veiculoServico, [FromQuery] string? name, [FromQuery] string? marca, [FromHeader] int offset = 0, [FromHeader] int limit = 100) =>
{
    var veiculos = veiculoServico.FindAll(offset, limit, name, marca);
    return Results.Ok(veiculos);
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute
{
    Roles = "Adm,Editor",
}).WithTags("Veiculo");

app.MapGet("/veiculos/{id}", (IVeiculoServico veiculoServico, int id) =>
{
    var veiculo = veiculoServico.FindOneById(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(veiculo);
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute
{
    Roles = "Adm,Editor",
}).WithTags("Veiculo");

app.MapDelete("/veiculos/{id}", (IVeiculoServico veiculoServico, int id) =>
{
    var veiculo = veiculoServico.FindOneById(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    veiculoServico.Delete(veiculo);
    return Results.NoContent();
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute
{
    Roles = "Adm",
}).WithTags("Veiculo");

app.MapPut("/veiculos/{id}", (IVeiculoServico veiculoServico, int id, [FromBody] VeiculoDto dto) =>
{
    var veiculo = veiculoServico.FindOneById(id);
    if (veiculo == null)
    {
        return Results.NotFound();
    }
    veiculo.Name = dto.Name;
    veiculo.Ano = dto.Ano;
    veiculo.Marca = dto.Marca;

    veiculoServico.Update(veiculo);
    return Results.Ok(veiculo);
}).RequireAuthorization().RequireAuthorization(new AuthorizeAttribute
{
    Roles = "Adm",
}).WithTags("Veiculo");

app.MapPost("/veiculos", ([FromBody] VeiculoDto veiculoDto, IVeiculoServico veiculoServico) =>
{
    try
    {
        var veiculo = new Veiculo
        {
            Name = veiculoDto.Name,
            Ano = veiculoDto.Ano,
            Marca = veiculoDto.Marca,
        };
        veiculoServico.Add(veiculo);
        return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.ToString());
    }
})
    .RequireAuthorization()
    .RequireAuthorization(new AuthorizeAttribute
{
    Roles = "Adm,Editor",
})
    .WithTags("Veiculo");
#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
