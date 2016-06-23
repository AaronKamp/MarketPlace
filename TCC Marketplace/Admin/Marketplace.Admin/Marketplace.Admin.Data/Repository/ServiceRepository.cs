using System.Linq;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;
using System.Data.Entity;

namespace Marketplace.Admin.Data.Repository
{

    public interface IServiceRepository : IRepository<Service>
    {
        ServicePaginationDTO GetServices(string country, string state, string keywords, string thermostats, string SCFs, string zipCodes, int? page);
        void UpdateServiceStatus(Service service);
        Service GetDetailsById(int id);
    }

    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        public Service GetDetailsById(int id)
        {
            var service = DbContext.Services.Where(ser => ser.Id == id)
                                .Include(s => s.SCFs.Select(st => st.State.Country))
                                .Include(p => p.ServiceProducts.Select(pd => pd.Product.ProductCategory));

            return service.FirstOrDefault();
        }

        public ServicePaginationDTO GetServices(string country, string state, string keywords, string thermostats, string SCFs, string zipCodes, int? page)
        {
            var pageNo = page ?? 1;

            var sgvm = new ServicePaginationDTO { PageSize = 10 };

            var services = DbContext.ServicesViews.Where(o =>
                                 (string.IsNullOrEmpty(country) || o.Countries.Contains(country))
                              && (string.IsNullOrEmpty(state) || o.States.Contains(state))
                              && (string.IsNullOrEmpty(keywords) || (o.Tilte.Contains(keywords) || o.ShortDescription.Contains(keywords)))
                              && (string.IsNullOrEmpty(SCFs) || o.SCFCodes.Contains(SCFs))
                              && (string.IsNullOrEmpty(zipCodes) || o.ZipCodes.Contains(zipCodes))
            );

            sgvm.TotalRecord = services.Count();
            sgvm.NoOfPages = (sgvm.TotalRecord / sgvm.PageSize) + ((sgvm.TotalRecord % sgvm.PageSize) > 0 ? 1 : 0);
            sgvm.CurrentPage = pageNo;
            sgvm.Services = services
                            .Select(x => new ServiceListDTO
                            {
                                ServiceId = x.Id,
                                Countries = x.Countries,
                                EndDate = x.EndDate,
                                ProductId = x.InAppPurchaseId,
                                ShortDescription = x.ShortDescription,
                                StartDate = x.StartDate,
                                States = x.States,
                                IsActive = x.IsActive,
                                Title = x.Tilte,
                                Type = x.ServiceType,
                                IconImage = x.IconImage,
                                UpdatedDate = x.UpdatedDate
                            }
            ).OrderByDescending(x => x.UpdatedDate).Skip((pageNo - 1) * sgvm.PageSize).Take(sgvm.PageSize).ToList();

            return sgvm;
        }

        public void UpdateServiceStatus(Service service)
        {
            base.Update(service);
        }

        public override void Update(Service service)
        {
            foreach (var serProd in service.ServiceProducts)
            {
                DbContext.Entry<Product>(serProd.Product).State = EntityState.Unchanged;
            }

            foreach (var scf in service.SCFs)
            {
                DbContext.Entry<SCF>(scf).State = EntityState.Unchanged;
            }

            DbContext.Set<Service>().Attach(service);
            DbContext.Entry(service).State = EntityState.Modified;
        }

        public override void Add(Service service)
        {
            foreach (var serProd in service.ServiceProducts)
            {
                DbContext.Entry<Product>(serProd.Product).State = EntityState.Unchanged;
            }

            foreach (var scf in service.SCFs)
            {
                DbContext.Entry<SCF>(scf).State = EntityState.Unchanged;
            }

            DbContext.Services.Add(service);
        }
    }
}
