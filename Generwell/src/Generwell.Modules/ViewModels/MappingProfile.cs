using AutoMapper;
using Generwell.Core.Model;

namespace Generwell.Modules.ViewModels
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<WellModel, WellViewModel>();
            CreateMap<FilterModel, FilterViewModel>();
            CreateMap<WellLineReportModel, WellLineReportViewModel>();
            CreateMap<WellDetailsModel, WellDetailsViewModel>();
            CreateMap<MapModel, MapViewModel>();
            CreateMap<LineReportsModel, LineReportsViewModel>();
            CreateMap<AccessTokenModel, AccessTokenViewModel>();
            CreateMap<ContactFieldsModel, ContactFieldsViewModel>();
            CreateMap<TaskModel, TaskViewModel>();
            CreateMap<TaskDetailsModel, TaskDetailsViewModel>();
            CreateMap<SupportModel, SupportViewModel>();
        }
    }
}
