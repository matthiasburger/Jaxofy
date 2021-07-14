using DasTeamRevolution.Data.Models;
using DasTeamRevolution.Models.Dto.ApplicationUser;
using DasTeamRevolution.Models.Dto.Assignment;
using DasTeamRevolution.Models.Dto.Client;
using DasTeamRevolution.Models.Dto.ClientGroup;
using DasTeamRevolution.Models.Dto.ClientHeader;
using DasTeamRevolution.Models.Dto.ClientSupplier;
using DasTeamRevolution.Models.Dto.ClientUser;
using DasTeamRevolution.Models.Dto.ClientUserSetting;
using DasTeamRevolution.Models.Dto.Employee;
using DasTeamRevolution.Models.Dto.JobProfile;
using DasTeamRevolution.Models.Dto.Order;
using DasTeamRevolution.Models.Dto.PoolEmployee;
using DasTeamRevolution.Models.Dto.Supplier;
using DasTeamRevolution.Models.Dto.SupplierHeader;
using DasTeamRevolution.Models.Dto.SupplierUser;
using DasTeamRevolution.Models.Dto.SupplierUserSettings;
using DasTeamRevolution.Models.Dto.TimeRecord;
using DasTeamRevolution.Models.Dto.Vacancy;
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

            builder.EntitySet<ClientHeader>("ClientHeaders");
            builder.EntityType<ClientHeader>().HasKey(x => x.Id);
            builder.EntityType<ClientHeaderResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<ClientGroup>("ClientGroups");
            builder.EntityType<ClientGroup>().HasKey(x => x.Id);
            builder.EntityType<ClientGroupResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<Client>("Clients");
            builder.EntityType<Client>().HasKey(x => x.Id);
            builder.EntityType<ClientResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<ClientUser>("ClientUsers");
            builder.EntityType<ClientUser>().HasKey(x => x.Id);
            builder.EntityType<ClientUserResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<ClientUserSetting>("ClientUserSettings");
            builder.EntityType<ClientUserSetting>().HasKey(x => x.Id);
            builder.EntityType<ClientUserSettingResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<Vacancy>("Vacancies");
            builder.EntityType<Vacancy>().HasKey(x => x.Id);
            builder.EntityType<VacancyResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<PoolEmployee>("PoolEmployees");
            builder.EntityType<PoolEmployee>().HasKey(x => x.Id);
            builder.EntityType<PoolEmployeeResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<Employee>("Employees");
            builder.EntityType<Employee>().HasKey(x => x.Id);
            builder.EntityType<EmployeeResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<SupplierUser>("SupplierUsers");
            builder.EntityType<SupplierUser>().HasKey(x => x.Id);
            builder.EntityType<SupplierUserResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<SupplierUserSetting>("SupplierUserSettings");
            builder.EntityType<SupplierUserSetting>().HasKey(x => x.Id);
            builder.EntityType<SupplierUserSettingsResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<Supplier>("Suppliers");
            builder.EntityType<Supplier>().HasKey(x => x.Id);
            builder.EntityType<SupplierResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<ClientSupplier>("ClientSuppliers");
            builder.EntityType<ClientSupplier>().HasKey(x => x.Id);
            builder.EntityType<ClientSupplierResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<SupplierHeader>("SupplierHeaders");
            builder.EntityType<SupplierHeader>().HasKey(x => x.Id);
            builder.EntityType<SupplierHeaderResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<Order>("Orders");
            builder.EntityType<Order>().HasKey(x => x.Id);
            builder.EntityType<OrderResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<Assignment>("Assignments");
            builder.EntityType<Assignment>().HasKey(x => x.Id);
            builder.EntityType<AssignmentResponseDto>().HasKey(x => x.Id);

            builder.EntitySet<JobProfile>("JobProfiles");
            builder.EntityType<JobProfile>().HasKey(x => x.Id);
            builder.EntityType<JobProfileResponseDto>().HasKey(x => x.Id);
            
            builder.EntitySet<TimeRecord>("TimeRecords");
            builder.EntityType<TimeRecord>().HasKey(x => x.Id);
            builder.EntityType<TimeRecordResponseDto>().HasKey(x => x.Id);

            return builder.GetEdmModel();
        }
    }
}