using Core.Model.NovaPoshta.City;
using Core.Model.NovaPoshta.Department;

namespace Core.Interfaces
{
    public interface INovaPoshtaService
    {
        Task<List<CityItemResponse>> FetchCitiesAsync();
        Task<List<DepartmentItemResponse>> FetchDepartmentsAsync();
    }
}
