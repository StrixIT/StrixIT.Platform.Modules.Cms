using StrixIT.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsData : ICmsData
    {
        #region Private Fields

        private IPlatformDataSource _dataSource;
        private IEntityHelper _entityHelper;
        private IEntityManager _entityManager;
        private IEnvironment _environment;
        private IPlatformHelper _platformHelper;
        private ITaxonomyManager _taxonomyManager;

        #endregion Private Fields

        #region Public Constructors

        public CmsData(IPlatformDataSource dataSource, IEntityManager entityManager, ITaxonomyManager taxonomyManager, IEntityHelper entityHelper, IPlatformHelper platformHelper, IEnvironment environment)
        {
            _dataSource = dataSource;
            _entityManager = entityManager;
            _taxonomyManager = taxonomyManager;
            _entityHelper = entityHelper;
            _platformHelper = platformHelper;
            _environment = environment;
        }

        #endregion Public Constructors

        #region Public Properties

        public IPlatformDataSource DataSource
        {
            get
            {
                return _dataSource;
            }
        }

        public IEntityHelper EntityHelper
        {
            get
            {
                return _entityHelper;
            }
        }

        public IEntityManager EntityManager
        {
            get
            {
                return _entityManager;
            }
        }

        public IEnvironment Environment
        {
            get
            {
                return _environment;
            }
        }

        public IPlatformHelper PlatformHelper
        {
            get
            {
                return _platformHelper;
            }
        }

        public ITaxonomyManager TaxonomyManager
        {
            get
            {
                return _taxonomyManager;
            }
        }

        #endregion Public Properties
    }
}