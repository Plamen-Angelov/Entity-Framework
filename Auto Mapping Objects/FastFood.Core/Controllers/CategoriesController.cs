namespace FastFood.Core.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using FastFood.Models;
    using FastFoodServices.DTO.Category;
    using FastFoodServices.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICategoryService categoryService;

        public CategoriesController(IMapper mapper, ICategoryService categoryService)
        {
            this.mapper = mapper;
            this.categoryService = categoryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Create");
            }

            CreateCategoryDTO categoruDTO = mapper.Map<CreateCategoryDTO>(model);
            categoryService.Create(categoruDTO);

            return this.RedirectToAction("All");
        }

        public IActionResult All()
        {
            ICollection<ListAllCategoriesDTO> categoriesDTO = categoryService.All();

            List<CategoryAllViewModel> categoryViewModels = mapper
                .Map<ICollection<ListAllCategoriesDTO>, ICollection<CategoryAllViewModel>>(categoriesDTO)
                .ToList();


            return this.View("All", categoryViewModels);
        }
    }
}
