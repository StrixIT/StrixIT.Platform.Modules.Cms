////------------------------------------------------------------------------------
//// <auto-generated>
////     This code was not generated by a tool. but for stylecop suppression.
//// </auto-generated>
////------------------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms.Migrations
{
    using StrixIT.Platform.Core;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<StrixIT.Platform.Modules.Cms.PlatformDataSource>
    {
        #region Public Constructors

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Seed(StrixIT.Platform.Modules.Cms.PlatformDataSource context)
        {
            new MailBuilder(DependencyInjector.Get<IFileSystemWrapper>(), DependencyInjector.TryGet(typeof(IMembershipService)) as IMembershipService).InitMails(context);
        }

        #endregion Protected Methods
    }
}