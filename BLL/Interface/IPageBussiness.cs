using DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IPageBussiness
    {

        /// <summary>
        /// ....To retrive all pages 
        /// </summary>
        /// <returns></returns>
        List<PageDomainModel> GetAllPages();

        bool SlugTitleExist(string slug, string title);

        bool InsertPage(PageDomainModel page);

        PageDomainModel GetPageById(int id);

        bool EditPage(PageDomainModel page);


        /// <summary>
        /// ....Check if a specific page  row 'slug, title' is unique from others
        /// </summary>
        /// <returns></returns>
        bool IsRowSlugTitleUnique(int id, string slug, string title);

        void RemovePage(int id);

        /// <summary>
        /// ....order and Save Pages in the database......
        /// </summary>
        void SortPages(int[] id);

        /// <summary>
        /// .....Check if page exist.....
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        bool PageExist(string slug);

        /// <summary>
        /// retrive page by slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        PageDomainModel GetPageBySlug(string slug);

    }
}
