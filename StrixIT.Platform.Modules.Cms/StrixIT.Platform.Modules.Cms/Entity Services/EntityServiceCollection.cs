//-----------------------------------------------------------------------
// <copyright file="EntityServiceCollection.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// A class to wrap the entity service record tuple list.
    /// </summary>
    public class EntityServiceCollection : List<Tuple<string, Guid, IList<ServiceActionRecord>>>
    {
        public EntityServiceCollection() : base() { }
    }
}