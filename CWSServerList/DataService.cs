using CWSServerList.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataService
{
    private readonly IServiceProvider _serviceProvider;

    public DataService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<List<Cwsserver>> GetServersAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClinicalIntranetAlphaContext>();
            return await context.Cwsservers.ToListAsync();
        }
    }

    public async Task<List<Cwsenvironment>> GetEnvironmentsAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ClinicalIntranetAlphaContext>();
            return await context.Cwsenvironments.ToListAsync();
        }
    }

}
