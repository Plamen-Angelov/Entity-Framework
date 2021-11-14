
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFoodServices.DTO.Category;
using FastFoodServices.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace FastFoodServices
{
    public class CategoryService : ICategoryService
    {
        private readonly FastFoodContext dbContext;
        private readonly IMapper mapper;

        public CategoryService(FastFoodContext context, IMapper mapper)
        {
            this.dbContext = context;
            this.mapper = mapper;
        }

        public void Create(CreateCategoryDTO dto)
        {
            Category category = mapper.Map<Category>(dto);

            this.dbContext.Categories.Add(category);
            this.dbContext.SaveChanges();
        }


        public ICollection<ListAllCategoriesDTO> All()
            => dbContext
            .Categories
            .ProjectTo<ListAllCategoriesDTO>(this.mapper.ConfigurationProvider)
            .ToList();
    }
}
