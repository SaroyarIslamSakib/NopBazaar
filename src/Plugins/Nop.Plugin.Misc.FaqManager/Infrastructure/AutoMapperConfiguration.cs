using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Nop.Plugin.Misc.FaqManager.Admin.Models;
using Nop.Plugin.Misc.FaqManager.Domain;

namespace Nop.Plugin.Misc.FaqManager.Infrastructure;

public class AutoMapperConfiguration : Profile, IOrderedMapperProfile
{
    #region Ctor

    public AutoMapperConfiguration()
    {
        CreateMap<FaqGroup, FaqGroupModel>()
            .ForMember(model => model.ProductName, options => options.Ignore())
            .ForMember(model => model.AvailableProducts, options => options.Ignore())
            .ForMember(model => model.Locales, options => options.Ignore())
            .ForMember(model => model.FaqItemSearchModel, options => options.Ignore());
        CreateMap<FaqGroupModel, FaqGroup>();

        CreateMap<FaqItem, FaqItemModel>()
            .ForMember(model => model.GroupName, options => options.Ignore())
            .ForMember(model => model.Locales, options => options.Ignore());
        CreateMap<FaqItemModel, FaqItem>();

        CreateMap<FaqManagerSettings, ConfigurationModel>()
            .ForMember(model => model.ActiveStoreScopeConfiguration, options => options.Ignore())
            .ForMember(model => model.ShowFaqCount_OverrideForStore, options => options.Ignore());
        CreateMap<ConfigurationModel, FaqManagerSettings>();
    }

    #endregion

    #region Properties

    public int Order => 0;

    #endregion
}