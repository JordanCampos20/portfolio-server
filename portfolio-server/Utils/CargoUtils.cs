using finance_control_server.Models;
using Microsoft.AspNetCore.Identity;

public class CargoUtils
{
    private readonly RoleManager<Cargo> _roleManager;

    public CargoUtils(RoleManager<Cargo> roleManager)
    {
        _roleManager = roleManager;
    }

    public void CreateCargos()
    {
        try
        {
            foreach (var cargoNome in new List<string>() { "membro", "administrador" })
            {
                if (!_roleManager.RoleExistsAsync(cargoNome).Result)
                {
                    IdentityResult roleResult = _roleManager.CreateAsync(new Cargo(cargoNome)).Result;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public static class CargoWebApplication
{
    public static void CreateCargos(this WebApplication app)
    {
        var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

        using (var scope = scopedFactory!.CreateScope())
        {
            CargoUtils? _cargoUtils = scope.ServiceProvider.GetService<CargoUtils>();

            if (_cargoUtils != null)
                _cargoUtils!.CreateCargos();
        }
    }
}