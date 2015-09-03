using StrixIT.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrixIT.Platform.Modules.Cms
{
    public interface ICmsServices
    {
        #region Public Properties

        ICommentService CommentService { get; }
        IEntityHelper EntityHelper { get; }
        IEnvironment Environment { get; }
        IPageRegistrator PageRegistrator { get; }
        ITaxonomyService TaxonomyService { get; }

        #endregion Public Properties
    }
}