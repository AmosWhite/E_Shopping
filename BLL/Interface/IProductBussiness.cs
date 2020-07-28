using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
   public interface IProductBussiness
    {
        bool IsProductNameExist(string productName);

        int SaveNewProduct(ProductDomainModel model);

        void UpDateImageName(int id, string imageName);

        IEnumerable<ProductDomainModel> GetAllProducts();

        ProductDomainModel GetProductById(int id);

        /// <summary>
        /// Check if a row name is unique from others.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsRowNameUnique(int id, string name);

        void UpdateProduct(ProductDomainModel model);

        /// <summary>
        /// ...Delete product with the matching id...
        /// </summary>
        /// <param name="id"></param>
        void DeleteProduct(int id);

        /// <summary>
        ///...Retrieve product by slug name...
        /// </summary>
        /// <returns></returns>
        ProductDomainModel GetProductBySlug(string name);

        bool IsProductSlugExist(string SlugName);
    }
}
