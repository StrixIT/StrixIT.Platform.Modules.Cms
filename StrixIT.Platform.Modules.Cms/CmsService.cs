using StrixIT.Platform.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsService : ICmsService
    {
        #region Private Fields

        private static ConcurrentBag<EntityType> _entityTypeList;
        private static bool _isInitialized = false;
        private static object _lockObject = new object();
        private static ConcurrentBag<ObjectMap> _objectMaps = new ConcurrentBag<ObjectMap>();

        private IPlatformDataSource _dataSource;

        #endregion Private Fields

        #region Public Constructors

        public CmsService(IPlatformDataSource dataSource) : this(dataSource, null)
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal CmsService(IPlatformDataSource dataSource, IList<EntityType> entityTypes)
        {
            _dataSource = dataSource;

            if (entityTypes != null)
            {
                _entityTypeList = new ConcurrentBag<EntityType>(entityTypes);
            }

            if (!_isInitialized)
            {
                Init();
            }
        }

        #endregion Internal Constructors

        #region Public Methods

        public IList<EntityType> EntityTypes
        {
            get
            {
                return _entityTypeList.ToList();
            }
        }

        public IList<ObjectMap> ObjectMaps
        {
            get
            {
                return _objectMaps.ToList();
            }
        }

        public void Initialize()
        {
            if (!_isInitialized)
            {
                Init();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static void CreateObjectMaps()
        {
            var listModeltypes = new List<Type>();
            var viewModeltypes = new List<Type>();
            var models = new List<object>();
            var assemblies = DependencyInjector.GetLoadedAssemblies();

            foreach (var assembly in assemblies)
            {
                listModeltypes.AddRange(assembly.GetTypes().Where(ty => ty.IsClass
                                                               && !ty.IsAbstract
                                                               && !ty.IsGenericType
                                                               && typeof(IListModel).IsAssignableFrom(ty)));

                viewModeltypes.AddRange(assembly.GetTypes().Where(ty => ty.IsClass
                                               && !ty.IsAbstract
                                               && !ty.IsGenericType
                                               && typeof(IViewModel).IsAssignableFrom(ty)));
            }

            foreach (var type in listModeltypes)
            {
                models.Add(Activator.CreateInstance(type));
            }

            foreach (var type in viewModeltypes)
            {
                models.Add(Activator.CreateInstance(type));
            }

            var entityTypes = models.Select(m => m.GetPropertyValue("EntityType")).Cast<Type>().Distinct();

            foreach (var type in entityTypes)
            {
                var viewModelType = models.Where(m => typeof(IViewModel).IsAssignableFrom(m.GetType()) && m.GetPropertyValue("EntityType").Equals(type)).Select(m => m.GetType()).FirstOrDefault();
                var listModelType = models.Where(m => typeof(IListModel).IsAssignableFrom(m.GetType()) && m.GetPropertyValue("EntityType").Equals(type)).Select(m => m.GetType()).FirstOrDefault();

                viewModelType = viewModelType != null ? viewModelType : listModelType;
                listModelType = listModelType != null ? listModelType : viewModelType;

                if (viewModelType != null || listModelType != null)
                {
                    var existing = _objectMaps.Where(m => m.ViewModelType.FullName == viewModelType.FullName || m.ListModelType.FullName == listModelType.FullName);

                    if (existing.Any())
                    {
                        var message = string.Format("There is a duplicatie object map entry for entity type {0} with view model type {1} and list model type {2}.", type.Name, viewModelType.Name, listModelType.Name);
                        throw new StrixConfigurationException(message);
                    }

                    _objectMaps.Add(new ObjectMap(type, viewModelType, listModelType));
                }
            }
        }

        private static Type GetNonProxyType(Type type)
        {
            if (type.Namespace == PlatformConstants.ENTITYFRAMEWORKPROXYTYPE)
            {
                type = type.BaseType;
            }

            return type;
        }

        private void GetOrCreateEntityTypes()
        {
            var types = new List<Type>();

            // Create or retrieve all entity types.
            foreach (var assembly in DependencyInjector.GetLoadedAssemblies())
            {
                types.AddRange(assembly.GetTypes().Where(ty => ty.IsClass
                                                               && !ty.IsAbstract
                                                               && !ty.IsGenericType
                                                               && typeof(IContent).IsAssignableFrom(ty)));
            }

            if (_entityTypeList.IsEmpty())
            {
                _entityTypeList = new ConcurrentBag<EntityType>();
                var entityTypes = _dataSource.Query<EntityType>().Include(e => e.EntityTypeServiceActions).ToList();
                bool typeAdded = false;

                foreach (Type type in types)
                {
                    var existingType = entityTypes.Where(et => et.Name.Equals(GetNonProxyType(type).FullName)).FirstOrDefault();

                    if (existingType == null)
                    {
                        var newType = new EntityType();
                        newType.Id = Guid.NewGuid();
                        newType.Name = type.FullName;
                        newType.EntityTypeServiceActions = new List<EntityTypeServiceAction>();
                        _dataSource.Save(newType);
                        _entityTypeList.Add(newType);
                        typeAdded = true;
                    }
                    else
                    {
                        _entityTypeList.Add(existingType);
                    }
                }

                if (typeAdded)
                {
                    _dataSource.SaveChanges();
                }
            }
        }

        private void Init()
        {
            lock (_lockObject)
            {
                if (_isInitialized)
                {
                    return;
                }

                GetOrCreateEntityTypes();
                CreateObjectMaps();
                _isInitialized = true;
            }
        }

        #endregion Private Methods
    }
}