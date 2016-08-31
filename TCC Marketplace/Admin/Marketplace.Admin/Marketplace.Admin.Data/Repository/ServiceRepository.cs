using System.Linq;
using Marketplace.Admin.Core.DTO;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;
using System.Data.Entity;

namespace Marketplace.Admin.Data.Repository
{
    /// <summary>
    /// Interface to ServiceRepository.
    /// </summary>
    public interface IServiceRepository : IRepository<Service>
    {
        /// <summary>
        /// Gets list of Services filtered by constraints in the argument list.
        /// </summary>
        ServicePaginationDTO GetServices(string country, string state, string keywords, string thermostats, string SCFs, string zipCodes, int? page);

        /// <summary>
        /// Update service activation status.
        /// </summary>
        /// <param name="service"> Service</param>
        void UpdateServiceStatus(Service service);

        /// <summary>
        /// Get service details by Id.
        /// </summary>
        /// <param name="id"> Service id.</param>
        Service GetDetailsById(int id);
    }

    /// <summary>
    /// Handles database operations for Service entity.
    /// </summary>
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        /// <summary>
        /// Get service details by Id.
        /// </summary>
        /// <param name="id"> Service id.</param>
        /// <returns>Service Entity</returns>
        public Service GetDetailsById(int id)
        {
            var service = DbContext.Services.Where(ser => ser.Id == id)
                                .Include(s => s.SCFs.Select(st => st.State.Country))
                                .Include(p => p.ServiceProducts.Select(pd => pd.Product.ProductCategory));

            return service.FirstOrDefault();
        }

        /// <summary>
        /// Gets list of Services filtered by following constraints.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <param name="keywords"></param>
        /// <param name="thermostats"></param>
        /// <param name="SCFs"></param>
        /// <param name="zipCodes"></param>
        /// <param name="page"></param>
        /// <returns>ServicePaginationDTO </returns>
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

        /// <summary>
        /// Updates service active status.
        /// </summary>
        /// <param name="service"> Service </param>
        public void UpdateServiceStatus(Service service)
        {
            base.Update(service);
        }

        /// <summary>
        /// Updates service in database.
        /// </summary>
        /// <param name="service">Service</param>
        public override void Update(Service service)
        {
            DbContext.Configuration.AutoDetectChangesEnabled = false;
            foreach (var serProd in service.ServiceProducts)
            {
                DbContext.Entry(serProd.Product).State = EntityState.Unchanged;
            }

            foreach (var scf in service.SCFs)
            {
                DbContext.Entry(scf).State = EntityState.Unchanged;
            }

            DbContext.Set<Service>().Attach(service);
            DbContext.Entry(service).State = EntityState.Modified;

            DbContext.Configuration.AutoDetectChangesEnabled = true;
        }

        /// <summary>
        /// Adds new service in database.
        /// </summary>
        /// <param name="service">Service</param>
        public override void Add(Service service)
        {
            DbContext.Configuration.AutoDetectChangesEnabled = false;

            foreach (var serProd in service.ServiceProducts)
            {
                DbContext.Entry(serProd.Product).State = EntityState.Unchanged;
            }

            foreach (var scf in service.SCFs)
            {
               DbContext.Entry(scf).State = EntityState.Unchanged;
            }
            
            DbContext.Services.Add(service);

            DbContext.Configuration.AutoDetectChangesEnabled = true;
        }
    }
}
