using BLL.Interface;
using DAL;
using DAL.Infrastructure.Contract;
using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductBussiness : IProductBussiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ProductRepository productRepository;

        public ProductBussiness(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
            this.productRepository = new ProductRepository(unitOfWork);
        }

        public IEnumerable<ProductDomainModel> GetAllProducts()
        {
            IEnumerable<ProductDomainModel> productDM = productRepository.GetAll()
                .Select(x => new ProductDomainModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryName = x.CategoryName,
                    CategoryId = x.CategoryId,
                    ImageName = x.ImageName
                }).AsEnumerable();

            return productDM;
        }

        public bool IsProductNameExist(string productName)
        {
            bool response;

            if (productRepository.Exists(x => x.Name == productName))
            {
                response = true;
            }
            else
            {
                response = false;
            }
            return response;
        }

        public int SaveNewProduct(ProductDomainModel model)
        {
            Product productDTO = new Product
            {
                Name = model.Name,
                Slug = model.Name.Replace(" ", "-").ToLower(),
                Description = model.Description,
                //Price = model.Price,
                //CategoryId = model.CategoryId,
                CategoryName = model.CategoryName
                
            };

            int id = productRepository.Insert(productDTO).Id;

            return id;
        }

        public void UpDateImageName(int id, string imageName)
        {
            Product productDTO = productRepository.SingleOrDefault(x => x.Id == id);
            productDTO.ImageName = imageName;
            productRepository.Update(productDTO);

        }

        public ProductDomainModel GetProductById(int id)
        {
            Product productDTO = productRepository.SingleOrDefault(x => x.Id == id);

            ProductDomainModel productDM = new ProductDomainModel
                {
                    Id = productDTO.Id,
                    Name = productDTO.Name,
                    Slug = productDTO.Slug,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    CategoryName = productDTO.CategoryName,
                    CategoryId = productDTO.CategoryId,
                    ImageName = productDTO.ImageName
                };

                return productDM;
            
        }

        public bool IsRowNameUnique(int id, string name)
        {
            bool feedback;

            if (productRepository.IsRowNameUniqueRepo(id, name))
                feedback = true;
            else
                feedback = false;

            return feedback;
        }

        public void UpdateProduct(ProductDomainModel model)
        {
            Product productDTO = productRepository.SingleOrDefault(x => x.Id == model.Id);

            productDTO.Name = model.Name;
            productDTO.Slug = model.Name.Replace(" ", "-").ToLower();
            productDTO.Description = model.Description;
            productDTO.Price = model.Price;
            productDTO.CategoryId = model.CategoryId;
            productDTO.ImageName = model.ImageName;

            productRepository.Update(productDTO);


        }

        public void DeleteProduct(int id)
        {
            productRepository.Delete(x => x.Id == id);
        }

        public ProductDomainModel GetProductBySlug(string name)
        {
            Product productDTO = productRepository.SingleOrDefault(x => x.Slug == name);

            ProductDomainModel productDM = new ProductDomainModel {
                 Id = productDTO.Id,
                 Name = productDTO.Name,
                 Slug = productDTO.Slug,
                CategoryId = productDTO.CategoryId,
                CategoryName = productDTO.CategoryName,
                 Description = productDTO.Description,
                 ImageName = productDTO.ImageName,
                 Price = productDTO.Price
            };

            return productDM;
        }

        public bool IsProductSlugExist(string SlugName)
        {
            bool response;

            if (productRepository.Exists(x => x.Slug == SlugName))
            {
                response = true;
            }
            else
            {
                response = false;
            }
            return response;
        }

    }
}
