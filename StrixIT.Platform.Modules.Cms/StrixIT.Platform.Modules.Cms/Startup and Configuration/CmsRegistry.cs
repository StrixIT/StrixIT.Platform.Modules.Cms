//-----------------------------------------------------------------------
// <copyright file="CmsRegistry.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Web;
using StructureMap.Configuration.DSL;
using StrixIT.Platform.Core;

namespace StrixIT.Platform.Modules.Cms
{
    public class CmsRegistry : Registry
    {
        public CmsRegistry()
        {
            For<IPlatformDataSource>()
                .AddInstances(inst => inst.ConstructedBy(() => new PlatformDataSource(new FileSystemWrapper(), new CacheService()))
                .Named(PlatformConstants.STRUCTUREMAPPRIVATE))
                .Use<PlatformDataSource>();

            For(typeof(IEntityService<>)).Use(typeof(EntityService<>));

            For<IPlatformHelper>().Use<DefaultPlatformHelper>();
            For<IEntityHelper>().Use<DefaultEntityHelper>();
        }
    }
}