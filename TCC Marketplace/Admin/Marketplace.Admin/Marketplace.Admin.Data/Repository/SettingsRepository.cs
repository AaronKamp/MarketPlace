using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Admin.Data.Infrastructure;
using Marketplace.Admin.Model;
using System.Data.Entity;

namespace Marketplace.Admin.Data.Repository
{
    public interface ISettingsRepository : IRepository<ConfigurationSettings>
    {
    }
    public class SettingsRepository : RepositoryBase<ConfigurationSettings>, ISettingsRepository
    {
        public SettingsRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}
