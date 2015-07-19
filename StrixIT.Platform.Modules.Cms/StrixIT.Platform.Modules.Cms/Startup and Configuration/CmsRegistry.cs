﻿#region Apache License
//-----------------------------------------------------------------------
// <copyright file="CmsRegistry.cs" company="StrixIT">
// Copyright 2015 StrixIT. Author R.G. Schurgers MA MSc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------
#endregion

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