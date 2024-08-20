namespace FaceitSharp.Api.Internal;

public interface IUserApiService
{
    Task<FaceitUserMe?> Me();

    Task<FaceitUser?> ById(string id);

    Task<FaceitUser?> ByUsername(string username);
}

internal class UserApiService(IInternalApiService _api) : IUserApiService
{
    public Task<FaceitUserMe?> Me() => _api.GetOne<FaceitUserMe>("users/v1/sessions/me");

    public Task<FaceitUser?> ById(string id) => _api.GetOne<FaceitUser>($"users/v1/users/{id}");

    public Task<FaceitUser?> ByUsername(string username) => _api.GetOne<FaceitUser>($"users/v1/nicknames/{username}");
}
