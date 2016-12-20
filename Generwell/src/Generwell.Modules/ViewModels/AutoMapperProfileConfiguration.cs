using AutoMapper;
using Generwell.Core.Model;
using System;

namespace Generwell.Modules.ViewModels
{    
    public class AutoMapperProfileConfiguration : Profile
    {
        [Obsolete("Create a constructor and configure inside of your profile's constructor instead. Will be removed in 6.0")]
        protected override void Configure()
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

            CreateMap<TaskFieldsModel, TaskFieldsViewModel>();
            CreateMap<TaskFieldsUpdateModel, TaskFieldsUpdateViewModel>();
            CreateMap<LookupFieldsModel, LookupFieldsViewModel>();
            CreateMap<LookupFieldsItemsModel, LookupFieldsItemsViewModel>();

            CreateMap<DictionaryModel, DictionaryViewModel>();
            CreateMap<DictionaryItemsModel, DictionaryItemsViewModel>();
            CreateMap<PictureModel, PictureViewModel>();
            CreateMap<AlbumModel, AlbumViewModel>();

            CreateMap<ContactInformationModel, ContactInformationViewModel>();
            CreateMap<ContactGroupModel, ContactGroupViewModel>();

            CreateMap<FieldsModel, FieldsViewModel>();
        }
    }
}
