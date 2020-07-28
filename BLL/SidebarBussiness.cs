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
    public class SidebarBussiness : ISidebarBussiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly SidebarRepository sidebarRepository;
        public SidebarBussiness(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
            this.sidebarRepository = new SidebarRepository(unitOfWork);
        }

        public bool EditSidebar(SidebarDomainModel model)
        {
            Sidebar sidebarDTO = sidebarRepository.SingleOrDefault(x => x.Id == model.Id);

            sidebarDTO.Id = model.Id;
            sidebarDTO.Body = model.Body;

            try
            {
                sidebarRepository.Update(sidebarDTO);
                return true;
            }
            catch 
            {

                return false;
            }
            



        }

        public SidebarDomainModel GetSidebarById(int id)
        {
            Sidebar sidebarDTO = sidebarRepository.SingleOrDefault(x => x.Id == id);

            SidebarDomainModel sidebarDM = new SidebarDomainModel
            {
                Id = sidebarDTO.Id,
                Body = sidebarDTO.Body
            };

            return sidebarDM;
        }
    }
}
