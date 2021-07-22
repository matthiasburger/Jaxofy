using Jaxofy.Data.Models;
using Jaxofy.Models.Dto.ApplicationUser;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Jaxofy.Data
{
    internal static class EdmModelBuilder
    {
        internal static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new(new DefaultAssemblyResolver());
            
            builder.EntitySet<ApplicationUser>("ApplicationUsers");
            builder.EntityType<ApplicationUser>().HasKey(x => x.Id);
            builder.EntityType<ApplicationUserResponseDto>().HasKey(x => x.Id);
            
            builder.EntitySet<Track>("Tracks");
            builder.EntityType<Track>().HasKey(x => x.Id);
            
            return builder.GetEdmModel();
        }
    }
}