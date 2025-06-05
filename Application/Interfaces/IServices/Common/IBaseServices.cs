namespace Application.Interfaces.IServices.Common
{
    public interface IBaseServices<dto>
    {
        Task<List<dto>> GetAllAsync();
        Task<dto?> GetByIdAsync(int id);
        Task CreateAsync(dto dto);
        Task UpdateAsync(dto dto);
        Task DeleteAsync(int id);
    }
}
