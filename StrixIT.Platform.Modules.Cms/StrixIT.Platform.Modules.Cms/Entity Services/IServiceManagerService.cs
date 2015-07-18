//-----------------------------------------------------------------------
// <copyright file="IServiceManagerService.cs" company="StrixIT">
//     Author: R.G. Schurgers MA MSc. Copyright (c) StrixIT. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms
{
    /// <summary>
    /// The interface for the service manager service for the Cms.
    /// </summary>
    public interface IServiceManagerService
    {
        /// <summary>
        /// Gets all service action records.
        /// </summary>
        /// <returns>A collection of service action records</returns>
        EntityServiceCollection GetServiceActionRecords();

        /// <summary>
        /// Saves the service action records to the data source.
        /// </summary>
        /// <param name="records">The records to save</param>
        /// <returns>True if the save was successful, false otherwise.</returns>
        bool SaveActionRecords(EntityServiceCollection records);
    }
}