using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ApplicationUser;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace DasTeamRevolution.Data
{
    internal static class EdmModelBuilder
    {
        internal static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new(new DefaultAssemblyResolver());
            builder.EntitySet<ApplicationUser>("ApplicationUsers");
            builder.EntityType<ApplicationUser>().HasKey(x => x.Id);
            builder.EntityType<ApplicationUserResponseDto>().HasKey(x => x.Id);
            return builder.GetEdmModel();
        }
    }
}