using API.DTOs.Designation;
using API.Helpers.Pagination;

namespace API.Interfaces.Service
{
    public interface IDesignationService
    {
        Task<IEnumerable<DesignationDto>> GetAllAsync();
        Task<DesignationDto?> GetByIdAsync(int id);
        Task<DesignationDto> CreateAsync(CreateDesignationDto dto);
        Task<bool> UpdateAsync(int id, UpdateDesignationDto dto);
        Task<bool> DeleteAsync(int id);
        Task<PagedResult<DesignationListItemDto>> GetPagedAsync(DesignationQueryParams queryParams);
    }
}
