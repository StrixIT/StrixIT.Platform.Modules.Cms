using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsServices : ICmsServices
    {
        #region Private Fields

        private ICommentService _commentService;
        private IEntityHelper _entityHelper;
        private IEnvironment _environment;
        private IPageRegistrator _pageRegistrator;
        private ITaxonomyService _taxonomyService;

        #endregion Private Fields

        #region Public Constructors

        public CmsServices(ICommentService commentService, ITaxonomyService taxonomyService, IEntityHelper entityHelper, IPageRegistrator pageRegistrator, IEnvironment environment)
        {
            _commentService = commentService;
            _taxonomyService = taxonomyService;
            _entityHelper = entityHelper;
            _pageRegistrator = pageRegistrator;
            _environment = environment;
        }

        #endregion Public Constructors

        #region Public Properties

        public ICommentService CommentService
        {
            get
            {
                return _commentService;
            }
        }

        public IEntityHelper EntityHelper
        {
            get
            {
                return _entityHelper;
            }
        }

        public IEnvironment Environment
        {
            get
            {
                return _environment;
            }
        }

        public IPageRegistrator PageRegistrator
        {
            get
            {
                return _pageRegistrator;
            }
        }

        public ITaxonomyService TaxonomyService
        {
            get
            {
                return _taxonomyService;
            }
        }

        #endregion Public Properties
    }
}