using StrixIT.Platform.Core;
using StrixIT.Platform.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrixIT.Platform.Modules.Cms.Web
{
    public class CmsPage<T> : BasePage<T>
    {
        #region Private Fields

        private IEntityHelper _helper;

        #endregion Private Fields

        #region Public Constructors

        public CmsPage()
        {
            _helper = DependencyInjector.Get<IEntityHelper>();
        }

        #endregion Public Constructors

        #region Public Properties

        public IEntityHelper Cms
        {
            get
            {
                return _helper;
            }
        }

        #endregion Public Properties
    }
}