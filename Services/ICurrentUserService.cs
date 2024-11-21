namespace WarehouseSystem.Services
{
    public interface ICurrentUserService
    {
        string GetCurrentUsername();
        int GetCurrentUserId();
        bool IsAdmin();
    }
}