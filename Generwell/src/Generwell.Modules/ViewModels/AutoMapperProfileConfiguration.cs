using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Generwell.Core.Model;

namespace Generwell.Modules.ViewModels
{
    public class AutoMapperProfileConfiguration : Profile
    {
        protected override void Configure()
        {
            CreateMap<WellModel, WellViewModel>();
            CreateMap<FilterModel, FilterViewModel>();
            CreateMap<WellLineReportModel, WellLineReportViewModel>();
            CreateMap<WellDetailsModel, WellDetailsViewModel>();
            CreateMap<MapModel, MapViewModel>();
            CreateMap<LineReportsModel, LineReportsViewModel>();
            CreateMap<FieldsModel, FieldsViewModel>();


        }
    }
}
