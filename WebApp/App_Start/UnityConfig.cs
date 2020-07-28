using BLL;
using BLL.Interface;
using DAL.Infrastructure;
using DAL.Infrastructure.Contract;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace WebApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IPageBussiness, PageBussiness>();
            container.RegisterType<ISidebarBussiness, SidebarBussiness>();
            container.RegisterType<ICategoryBussiness, CategoryBussiness>();
            container.RegisterType<IProductBussiness, ProductBussiness>();
            container.RegisterType<IUserBussiness, UserBussiness>();
            container.RegisterType<IOrderBussiness, OrderBussiness>();
            container.RegisterType<IOrderDetailBussiness, OrderDetailBussiness>();



            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}