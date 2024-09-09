using ETicaretAPI.Application.Dtos.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services.Configurations
{
    public interface IAplicationService
    {
        List<Menu> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
