using StrixIT.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrixIT.Platform.Modules.Cms
{
    public interface ICmsData
    {
        #region Public Properties

        IPlatformDataSource DataSource { get; }
        IEntityHelper EntityHelper { get; }
        IEntityManager EntityManager { get; }
        IEnvironment Environment { get; }
        IPlatformHelper PlatformHelper { get; }
        ITaxonomyManager TaxonomyManager { get; }

        #endregion Public Properties
    }
}