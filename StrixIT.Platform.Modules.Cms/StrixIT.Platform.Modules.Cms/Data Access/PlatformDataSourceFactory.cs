//-----------------------------------------------------------------------
// <copyright file="PlatformDataSourceFactory.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Data.Entity.Infrastructure;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// This class creates a PlatformDataSource instance for code first migrations.
    /// </summary>
    public class PlatformDataSourceFactory : IDbContextFactory<PlatformDataSource>
    {
        public PlatformDataSource Create()
        {
            return PlatformDataSource.CreateForMigrations();
        }
    }
}