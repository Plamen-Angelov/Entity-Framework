using FastFoodServices.DTO.Category;
using System.Collections.Generic;


namespace FastFoodServices.Interfaces
{
    public interface ICategoryService
    {
        void Create(CreateCategoryDTO dto);

        ICollection<ListAllCategoriesDTO> All();
    }
}
