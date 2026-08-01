using API.DTOs.Designation;
using API.Helpers.Pagination;
using API.Models.Entities;

namespace API.Interfaces.Repository;

public interface IDesignationRepository
{
    Task<IEnumerable<Designation>> GetAllAsync();
    Task<Designation?> GetByIdAsync(int id);
    Task AddAsync(Designation Designation);
    Task UpdateAsync(Designation Designation);
    Task DeleteAsync(Designation Designation);
    Task<bool> ExistsByNameAsync(string name);
    Task<PagedResult<DesignationListItemDto>> GetPagedAsync(DesignationQueryParams queryParams);
}