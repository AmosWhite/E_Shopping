using BLL.Interface;
using DAL;
using DAL.Infrastructure.Contract;
using DL;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class PageBussiness : IPageBussiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PageRepository pageRepository;
        public PageBussiness(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
            this.pageRepository = new PageRepository(unitOfWork);
        }

        public List<PageDomainModel> GetAllPages()
        {
            List<PageDomainModel> pages = new List<PageDomainModel>();

            pages = pageRepository.GetAll().OrderBy(x => x.Sorting)
                .Select(x => new PageDomainModel
                {
                    Id = x.Id,
                    Body = x.Body,
                    Slug = x.Slug,
                    HasSidebar = x.HasSidebar,
                    Sorting = x.Sorting,
                    Title = x.Title

                }).ToList();

            return pages;
        }

        public bool SlugTitleExist(string slug, string title)
        {
            bool response = pageRepository.Exists(x => x.Slug == slug || x.Title == title);

            return response;
        }


        public bool InsertPage(PageDomainModel page)
        {
            bool feedback = false;
            dynamic pageDTO = new Page
            {
                Body = page.Body,
                Slug = page.Slug,
                HasSidebar = page.HasSidebar,
                Sorting = page.Sorting,
                Title = page.Title

            };
            try
            {
                pageRepository.Insert(pageDTO);
                feedback = true;
                return feedback;
            }
            catch
            {

                return feedback;
            }
        }

        public PageDomainModel GetPageById(int id)
        {
            PageDomainModel model;

            Page page = pageRepository.SingleOrDefault(x => x.Id == id);

            model = new PageDomainModel
            {
                Id = page.Id,
                Title = page.Title,
                Slug = page.Slug,
                Body = page.Body,
                HasSidebar = page.HasSidebar,
                Sorting = page.Sorting
            };

            return model;

        }

        public bool EditPage(PageDomainModel page)
        {
            bool feedback = false;


            Page pageDTO = new Page
            {
                Id = page.Id,
                Body = page.Body,
                Slug = page.Slug,
                HasSidebar = page.HasSidebar,
                Sorting = page.Sorting,
                Title = page.Title

            };
            try
            {
                pageRepository.Update(pageDTO);
                feedback = true;
                return feedback;
            }
            catch
            {

                return feedback;
            }
        }
        public bool IsRowSlugTitleUnique(int id, string slug, string title)
        {
            bool response = pageRepository.IsRowSlugTitleUniqueRepo(id, slug, title);

            return response;
        }

        public void RemovePage(int id)
        {
            pageRepository.Delete(x => x.Id == id);
        }

        public void SortPages(int[] id)
        {
            // Set intial count
            int count = 1;

            // Declare DTO
            Page dto;

            //order and save each page
            foreach (int pageId in id)
            {
                dto = pageRepository.SingleOrDefault(x => x.Id == pageId);
                dto.Sorting = count;
                pageRepository.Update(dto);
                count++;
            }
        }

        public bool PageExist(string slug)
        {
            bool response = pageRepository.Exists(x => x.Slug == slug);

            return response;
        }

        public PageDomainModel GetPageBySlug(string slug)
        {

            PageDomainModel model;

            Page page = pageRepository.SingleOrDefault(x => x.Slug == slug);

            model = new PageDomainModel
            {
                Id = page.Id,
                Title = page.Title,
                Slug = page.Slug,
                Body = page.Body,
                HasSidebar = page.HasSidebar,
                Sorting = page.Sorting
            };

            return model;
        }
    }
}
