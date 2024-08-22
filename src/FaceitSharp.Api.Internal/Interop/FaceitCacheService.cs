namespace FaceitSharp.Api.Internal.Interop;

internal interface IFaceitCacheService : ICacheService { }

internal class FaceitCacheService(IFaceitJsonService _json) 
    : DiskCacheService(_json), IFaceitCacheService { }
