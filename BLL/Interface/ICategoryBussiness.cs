using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICategoryBussiness
    {
        IEnumerable<CategoryDomainModel> GetAllCategories();

        bool CategoryExist(string categoryName);

        string AddCategory(CategoryDomainModel model);

        void SortCategory(int[] id);

        void RemoveCategory(int id);

        void EditCategory(string newCateName, int id);

        CategoryDomainModel GetCategoryByName(string name);
    }
    
}
