using BLL.Interface;
using DAL;
using DAL.Infrastructure.Contract;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CategoryBussiness : ICategoryBussiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CategoryRepository categoryRepository;

        public CategoryBussiness(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
            this.categoryRepository = new CategoryRepository(unitOfWork);
        }

        public string AddCategory(CategoryDomainModel model)
        {
            Category categoryDTO = new Category()
            {
                Name = model.Name,
                Slug = model.Slug,
                Sorting = model.Sorting,
            };

            string id = categoryRepository.Insert(categoryDTO).Id.ToString();
            return id;
        }

        public bool CategoryExist(string categoryName)
        {
            if (!categoryRepository.Exists(x => x.Name == categoryName))
                return false;

            else
                return true;
        }

        public IEnumerable<CategoryDomainModel> GetAllCategories()
        {
            IEnumerable<CategoryDomainModel> categoryDM = categoryRepository.GetAll()
                .OrderBy(x => x.Sorting)
                .Select(x => new CategoryDomainModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    Sorting = x.Sorting
                }).AsEnumerable();
            return categoryDM;
        }

        public void SortCategory(int[] id)
        {
            // Set intial count
            int count = 1;

            // Declare DTO
            Category dto;

            //order and save each page
            foreach (int pageId in id)
            {
                dto = categoryRepository.SingleOrDefault(x => x.Id == pageId);
                dto.Sorting = count;
                categoryRepository.Update(dto);
                count++;
            }
        }

        public void RemoveCategory(int id)
        {
            categoryRepository.Delete(x => x.Id == id);
        }

        public void EditCategory(string newCateName, int id)
        {
            Category categoryDTO = categoryRepository.SingleOrDefault(x => x.Id == id);
            categoryDTO.Name = newCateName;
            categoryDTO.Slug = newCateName.Replace(" ", "-");

            categoryRepository.Update(categoryDTO);
        }

        public CategoryDomainModel GetCategoryByName(string name)
        {
            Category catDTO = categoryRepository.SingleOrDefault(x => x.Slug == name);
            
            CategoryDomainModel catDM = new CategoryDomainModel{
                Id = catDTO.Id,
                Name = catDTO.Name,
                Slug = catDTO.Slug,
                Sorting = catDTO.Sorting };

            return catDM;
        }
    }
}
