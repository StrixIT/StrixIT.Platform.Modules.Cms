//------------------------------------------------------------------------------
// <auto-generated>
//     This code was not generated by a tool. but for stylecop suppression.
// </auto-generated>
//------------------------------------------------------------------------------
namespace StrixIT.Platform.Modules.Cms.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class initial : DbMigration
    {
        #region Public Methods

        public override void Down()
        {
            DropForeignKey("dbo.News", "UpdatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.News", "PublishedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.News", "LockedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.News", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.News", "DeletedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.News", "CreatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContents", "UpdatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContents", "TemplateId", "dbo.MailContentTemplates");
            DropForeignKey("dbo.MailContentTemplates", "UpdatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContentTemplates", "PublishedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContentTemplates", "LockedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContentTemplates", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.MailContentTemplates", "DeletedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContentTemplates", "CreatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContents", "PublishedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContents", "LockedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContents", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.MailContents", "DeletedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.MailContents", "CreatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Html", "UpdatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Html", "PublishedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Html", "LockedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Html", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.Html", "DeletedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Html", "CreatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Documents", "UpdatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Documents", "PublishedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Documents", "LockedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Documents", "FileId", "dbo.Files");
            DropForeignKey("dbo.Documents", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.Documents", "DeletedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Documents", "CreatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Documents", "AuthorUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.ContentCustomFieldValues", "CustomFieldId", "dbo.EntityCustomFields");
            DropForeignKey("dbo.Comments", "UpdatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.TaxonomyTermEntities", "TermId", "dbo.TaxonomyTerms");
            DropForeignKey("dbo.TaxonomyTermEntities", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.Entities", "OwnerUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Entities", "GroupId", "dbo.GroupNameLookups");
            DropForeignKey("dbo.Entities", "EntityTypeId", "dbo.EntityTypes");
            DropForeignKey("dbo.TaxonomyTerms", "VocabularyId", "dbo.TaxonomyVocabularies");
            DropForeignKey("dbo.Files", "UploadedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.FileTerms", "TermId", "dbo.TaxonomyTerms");
            DropForeignKey("dbo.FileTerms", "FileId", "dbo.Files");
            DropForeignKey("dbo.Files", "GroupId", "dbo.GroupNameLookups");
            DropForeignKey("dbo.TaxonomySynonyms", "TermId", "dbo.TaxonomyTerms");
            DropForeignKey("dbo.TaxonomyParentChildTerms", "ParentTermId", "dbo.TaxonomyTerms");
            DropForeignKey("dbo.TaxonomyParentChildTerms", "TermId", "dbo.TaxonomyTerms");
            DropForeignKey("dbo.TaxonomySiblingTerms", "SiblingTermId", "dbo.TaxonomyTerms");
            DropForeignKey("dbo.TaxonomySiblingTerms", "TermId", "dbo.TaxonomyTerms");
            DropForeignKey("dbo.TaxonomyVocabularies", "GroupId", "dbo.GroupNameLookups");
            DropForeignKey("dbo.TaxonomyVocabularyEntityTypes", "EntityTypeId", "dbo.EntityTypes");
            DropForeignKey("dbo.TaxonomyVocabularyEntityTypes", "VocabularyId", "dbo.TaxonomyVocabularies");
            DropForeignKey("dbo.EntityTypeServiceActions", "GroupId", "dbo.GroupNameLookups");
            DropForeignKey("dbo.EntityTypeServiceActions", "EntityTypeId", "dbo.EntityTypes");
            DropForeignKey("dbo.EntityCustomFields", "EntityTypeId", "dbo.EntityTypes");
            DropForeignKey("dbo.EntityCustomFields", "GroupId", "dbo.GroupNameLookups");
            DropForeignKey("dbo.ContentSharedWithGroups", "GroupId", "dbo.GroupNameLookups");
            DropForeignKey("dbo.ContentSharedWithGroups", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.Comments", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.Comments", "CreatedByUserId", "dbo.UserNameLookups");
            DropForeignKey("dbo.Comments", "ParentId", "dbo.Comments");
            DropIndex("dbo.TaxonomyTermEntities", new[] { "TermId" });
            DropIndex("dbo.TaxonomyTermEntities", new[] { "EntityId" });
            DropIndex("dbo.FileTerms", new[] { "TermId" });
            DropIndex("dbo.FileTerms", new[] { "FileId" });
            DropIndex("dbo.TaxonomyParentChildTerms", new[] { "ParentTermId" });
            DropIndex("dbo.TaxonomyParentChildTerms", new[] { "TermId" });
            DropIndex("dbo.TaxonomySiblingTerms", new[] { "SiblingTermId" });
            DropIndex("dbo.TaxonomySiblingTerms", new[] { "TermId" });
            DropIndex("dbo.TaxonomyVocabularyEntityTypes", new[] { "EntityTypeId" });
            DropIndex("dbo.TaxonomyVocabularyEntityTypes", new[] { "VocabularyId" });
            DropIndex("dbo.News", new[] { "LockedByUserId" });
            DropIndex("dbo.News", new[] { "DeletedByUserId" });
            DropIndex("dbo.News", new[] { "PublishedByUserId" });
            DropIndex("dbo.News", new[] { "UpdatedByUserId" });
            DropIndex("dbo.News", new[] { "CreatedByUserId" });
            DropIndex("dbo.News", new[] { "EntityId" });
            DropIndex("dbo.MailContentTemplates", new[] { "LockedByUserId" });
            DropIndex("dbo.MailContentTemplates", new[] { "DeletedByUserId" });
            DropIndex("dbo.MailContentTemplates", new[] { "PublishedByUserId" });
            DropIndex("dbo.MailContentTemplates", new[] { "UpdatedByUserId" });
            DropIndex("dbo.MailContentTemplates", new[] { "CreatedByUserId" });
            DropIndex("dbo.MailContentTemplates", new[] { "EntityId" });
            DropIndex("dbo.MailContents", new[] { "LockedByUserId" });
            DropIndex("dbo.MailContents", new[] { "DeletedByUserId" });
            DropIndex("dbo.MailContents", new[] { "PublishedByUserId" });
            DropIndex("dbo.MailContents", new[] { "UpdatedByUserId" });
            DropIndex("dbo.MailContents", new[] { "CreatedByUserId" });
            DropIndex("dbo.MailContents", new[] { "TemplateId" });
            DropIndex("dbo.MailContents", new[] { "EntityId" });
            DropIndex("dbo.Html", new[] { "LockedByUserId" });
            DropIndex("dbo.Html", new[] { "DeletedByUserId" });
            DropIndex("dbo.Html", new[] { "PublishedByUserId" });
            DropIndex("dbo.Html", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Html", new[] { "CreatedByUserId" });
            DropIndex("dbo.Html", new[] { "EntityId" });
            DropIndex("dbo.Documents", new[] { "LockedByUserId" });
            DropIndex("dbo.Documents", new[] { "DeletedByUserId" });
            DropIndex("dbo.Documents", new[] { "PublishedByUserId" });
            DropIndex("dbo.Documents", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Documents", new[] { "CreatedByUserId" });
            DropIndex("dbo.Documents", new[] { "AuthorUserId" });
            DropIndex("dbo.Documents", new[] { "FileId" });
            DropIndex("dbo.Documents", new[] { "EntityId" });
            DropIndex("dbo.ContentCustomFieldValues", new[] { "CustomFieldId" });
            DropIndex("dbo.Files", new[] { "UploadedByUserId" });
            DropIndex("dbo.Files", new[] { "GroupId" });
            DropIndex("dbo.TaxonomySynonyms", new[] { "TermId" });
            DropIndex("dbo.TaxonomyTerms", new[] { "VocabularyId" });
            DropIndex("dbo.TaxonomyVocabularies", new[] { "GroupId" });
            DropIndex("dbo.EntityTypeServiceActions", new[] { "GroupId" });
            DropIndex("dbo.EntityTypeServiceActions", new[] { "EntityTypeId" });
            DropIndex("dbo.EntityCustomFields", new[] { "GroupId" });
            DropIndex("dbo.EntityCustomFields", new[] { "EntityTypeId" });
            DropIndex("dbo.ContentSharedWithGroups", new[] { "GroupId" });
            DropIndex("dbo.ContentSharedWithGroups", new[] { "EntityId" });
            DropIndex("dbo.Entities", new[] { "OwnerUserId" });
            DropIndex("dbo.Entities", new[] { "GroupId" });
            DropIndex("dbo.Entities", new[] { "EntityTypeId" });
            DropIndex("dbo.Comments", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Comments", new[] { "CreatedByUserId" });
            DropIndex("dbo.Comments", new[] { "ParentId" });
            DropIndex("dbo.Comments", new[] { "EntityId" });
            DropTable("dbo.TaxonomyTermEntities");
            DropTable("dbo.FileTerms");
            DropTable("dbo.TaxonomyParentChildTerms");
            DropTable("dbo.TaxonomySiblingTerms");
            DropTable("dbo.TaxonomyVocabularyEntityTypes");
            DropTable("dbo.News");
            DropTable("dbo.MailContentTemplates");
            DropTable("dbo.MailContents");
            DropTable("dbo.Html");
            DropTable("dbo.Documents");
            DropTable("dbo.ContentCustomFieldValues");
            DropTable("dbo.Files");
            DropTable("dbo.TaxonomySynonyms");
            DropTable("dbo.TaxonomyTerms");
            DropTable("dbo.TaxonomyVocabularies");
            DropTable("dbo.EntityTypeServiceActions");
            DropTable("dbo.EntityCustomFields");
            DropTable("dbo.EntityTypes");
            DropTable("dbo.GroupNameLookups");
            DropTable("dbo.ContentSharedWithGroups");
            DropTable("dbo.Entities");
            DropTable("dbo.UserNameLookups");
            DropTable("dbo.Comments");
        }

        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EntityId = c.Guid(nullable: false),
                    EntityCulture = c.String(nullable: false, maxLength: 5),
                    EntityVersion = c.Int(nullable: false),
                    ParentId = c.Long(),
                    CommentStatus = c.Int(nullable: false),
                    Text = c.String(nullable: false),
                    CreatedByUserId = c.Guid(nullable: false),
                    CreatedOn = c.DateTime(nullable: false),
                    UpdatedByUserId = c.Guid(nullable: false),
                    UpdatedOn = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.ParentId)
                .ForeignKey("dbo.UserNameLookups", t => t.CreatedByUserId)
                .ForeignKey("dbo.Entities", t => t.EntityId, cascadeDelete: true)
                .ForeignKey("dbo.UserNameLookups", t => t.UpdatedByUserId)
                .Index(t => t.EntityId)
                .Index(t => t.ParentId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);

            CreateTable(
                "dbo.UserNameLookups",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                    Email = c.String(nullable: false, maxLength: 250),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Entities",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Url = c.String(nullable: false, maxLength: 300),
                    EntityTypeId = c.Guid(nullable: false),
                    GroupId = c.Guid(nullable: false),
                    OwnerUserId = c.Guid(nullable: false),
                    IsPrivate = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityTypes", t => t.EntityTypeId, cascadeDelete: true)
                .ForeignKey("dbo.GroupNameLookups", t => t.GroupId)
                .ForeignKey("dbo.UserNameLookups", t => t.OwnerUserId)
                .Index(t => t.EntityTypeId)
                .Index(t => t.GroupId)
                .Index(t => t.OwnerUserId);

            CreateTable(
                "dbo.ContentSharedWithGroups",
                c => new
                {
                    EntityId = c.Guid(nullable: false),
                    GroupId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.EntityId, t.GroupId })
                .ForeignKey("dbo.Entities", t => t.EntityId, cascadeDelete: true)
                .ForeignKey("dbo.GroupNameLookups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.EntityId)
                .Index(t => t.GroupId);

            CreateTable(
                "dbo.GroupNameLookups",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EntityTypes",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EntityCustomFields",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    EntityTypeId = c.Guid(nullable: false),
                    GroupId = c.Guid(nullable: false),
                    FieldType = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 100),
                    Section = c.String(maxLength: 100),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupNameLookups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.EntityTypes", t => t.EntityTypeId)
                .Index(t => t.EntityTypeId)
                .Index(t => t.GroupId);

            CreateTable(
                "dbo.EntityTypeServiceActions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    EntityTypeId = c.Guid(nullable: false),
                    GroupId = c.Guid(nullable: false),
                    Action = c.String(nullable: false, maxLength: 250),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityTypes", t => t.EntityTypeId, cascadeDelete: true)
                .ForeignKey("dbo.GroupNameLookups", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.EntityTypeId)
                .Index(t => t.GroupId);

            CreateTable(
                "dbo.TaxonomyVocabularies",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    GroupId = c.Guid(nullable: false),
                    Culture = c.String(nullable: false, maxLength: 5),
                    Name = c.String(nullable: false, maxLength: 250),
                    Url = c.String(nullable: false, maxLength: 300),
                    UseTermRelations = c.Boolean(nullable: false),
                    UseTermHierarchy = c.Boolean(nullable: false),
                    IsSystemVocabulary = c.Boolean(nullable: false),
                    UserExtensible = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupNameLookups", t => t.GroupId)
                .Index(t => t.GroupId);

            CreateTable(
                "dbo.TaxonomyTerms",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    VocabularyId = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                    Url = c.String(nullable: false, maxLength: 300),
                    TagCount = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaxonomyVocabularies", t => t.VocabularyId, cascadeDelete: true)
                .Index(t => t.VocabularyId);

            CreateTable(
                "dbo.TaxonomySynonyms",
                c => new
                {
                    TermId = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                })
                .PrimaryKey(t => new { t.TermId, t.Name })
                .ForeignKey("dbo.TaxonomyTerms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.TermId);

            CreateTable(
                "dbo.Files",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    GroupId = c.Guid(nullable: false),
                    Folder = c.String(nullable: false, maxLength: 200),
                    Path = c.String(nullable: false, maxLength: 300),
                    FileName = c.String(nullable: false, maxLength: 200),
                    Extension = c.String(nullable: false, maxLength: 5),
                    OriginalName = c.String(maxLength: 300),
                    Size = c.Long(),
                    UploadedOn = c.DateTime(nullable: false),
                    UploadedByUserId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupNameLookups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.UserNameLookups", t => t.UploadedByUserId)
                .Index(t => t.GroupId)
                .Index(t => t.UploadedByUserId);

            CreateTable(
                "dbo.ContentCustomFieldValues",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ContentId = c.Guid(nullable: false),
                    CustomFieldId = c.Guid(nullable: false),
                    NumberValue = c.Double(),
                    StringValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityCustomFields", t => t.CustomFieldId, cascadeDelete: true)
                .Index(t => t.CustomFieldId);

            CreateTable(
                "dbo.Documents",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    EntityId = c.Guid(nullable: false),
                    FileId = c.Guid(nullable: false),
                    Description = c.String(),
                    AuthorName = c.String(maxLength: 250),
                    AuthorUserId = c.Guid(),
                    Location = c.String(maxLength: 250),
                    Date = c.DateTime(),
                    DownloadCount = c.Int(nullable: false),
                    Culture = c.String(nullable: false, maxLength: 5),
                    VersionNumber = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                    VersionLog = c.String(maxLength: 1000),
                    IsCurrentVersion = c.Boolean(nullable: false),
                    NumberOfComments = c.Int(nullable: false),
                    LastCommentDate = c.DateTime(),
                    CreatedByUserId = c.Guid(nullable: false),
                    CreatedOn = c.DateTime(nullable: false),
                    UpdatedByUserId = c.Guid(nullable: false),
                    UpdatedOn = c.DateTime(nullable: false),
                    PublishedByUserId = c.Guid(),
                    PublishedOn = c.DateTime(),
                    DeletedByUserId = c.Guid(),
                    DeletedOn = c.DateTime(),
                    LockedByUserId = c.Guid(),
                    LockedOn = c.DateTime(),
                    SortOrder = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserNameLookups", t => t.AuthorUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.CreatedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.DeletedByUserId)
                .ForeignKey("dbo.Entities", t => t.EntityId)
                .ForeignKey("dbo.Files", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.UserNameLookups", t => t.LockedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.PublishedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.UpdatedByUserId)
                .Index(t => t.EntityId)
                .Index(t => t.FileId)
                .Index(t => t.AuthorUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.PublishedByUserId)
                .Index(t => t.DeletedByUserId)
                .Index(t => t.LockedByUserId);

            CreateTable(
                "dbo.Html",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    EntityId = c.Guid(nullable: false),
                    Body = c.String(nullable: false),
                    Culture = c.String(nullable: false, maxLength: 5),
                    VersionNumber = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                    VersionLog = c.String(maxLength: 1000),
                    IsCurrentVersion = c.Boolean(nullable: false),
                    NumberOfComments = c.Int(nullable: false),
                    LastCommentDate = c.DateTime(),
                    CreatedByUserId = c.Guid(nullable: false),
                    CreatedOn = c.DateTime(nullable: false),
                    UpdatedByUserId = c.Guid(nullable: false),
                    UpdatedOn = c.DateTime(nullable: false),
                    PublishedByUserId = c.Guid(),
                    PublishedOn = c.DateTime(),
                    DeletedByUserId = c.Guid(),
                    DeletedOn = c.DateTime(),
                    LockedByUserId = c.Guid(),
                    LockedOn = c.DateTime(),
                    SortOrder = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserNameLookups", t => t.CreatedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.DeletedByUserId)
                .ForeignKey("dbo.Entities", t => t.EntityId)
                .ForeignKey("dbo.UserNameLookups", t => t.LockedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.PublishedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.UpdatedByUserId)
                .Index(t => t.EntityId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.PublishedByUserId)
                .Index(t => t.DeletedByUserId)
                .Index(t => t.LockedByUserId);

            CreateTable(
                "dbo.MailContents",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    EntityId = c.Guid(nullable: false),
                    TemplateId = c.Guid(nullable: false),
                    From = c.String(nullable: false, maxLength: 250),
                    Subject = c.String(nullable: false, maxLength: 250),
                    Body = c.String(nullable: false),
                    Culture = c.String(nullable: false, maxLength: 5),
                    VersionNumber = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                    VersionLog = c.String(maxLength: 1000),
                    IsCurrentVersion = c.Boolean(nullable: false),
                    NumberOfComments = c.Int(nullable: false),
                    LastCommentDate = c.DateTime(),
                    CreatedByUserId = c.Guid(nullable: false),
                    CreatedOn = c.DateTime(nullable: false),
                    UpdatedByUserId = c.Guid(nullable: false),
                    UpdatedOn = c.DateTime(nullable: false),
                    PublishedByUserId = c.Guid(),
                    PublishedOn = c.DateTime(),
                    DeletedByUserId = c.Guid(),
                    DeletedOn = c.DateTime(),
                    LockedByUserId = c.Guid(),
                    LockedOn = c.DateTime(),
                    SortOrder = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserNameLookups", t => t.CreatedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.DeletedByUserId)
                .ForeignKey("dbo.Entities", t => t.EntityId)
                .ForeignKey("dbo.UserNameLookups", t => t.LockedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.PublishedByUserId)
                .ForeignKey("dbo.MailContentTemplates", t => t.TemplateId)
                .ForeignKey("dbo.UserNameLookups", t => t.UpdatedByUserId)
                .Index(t => t.EntityId)
                .Index(t => t.TemplateId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.PublishedByUserId)
                .Index(t => t.DeletedByUserId)
                .Index(t => t.LockedByUserId);

            CreateTable(
                "dbo.MailContentTemplates",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    EntityId = c.Guid(nullable: false),
                    Body = c.String(nullable: false),
                    Culture = c.String(nullable: false, maxLength: 5),
                    VersionNumber = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                    VersionLog = c.String(maxLength: 1000),
                    IsCurrentVersion = c.Boolean(nullable: false),
                    NumberOfComments = c.Int(nullable: false),
                    LastCommentDate = c.DateTime(),
                    CreatedByUserId = c.Guid(nullable: false),
                    CreatedOn = c.DateTime(nullable: false),
                    UpdatedByUserId = c.Guid(nullable: false),
                    UpdatedOn = c.DateTime(nullable: false),
                    PublishedByUserId = c.Guid(),
                    PublishedOn = c.DateTime(),
                    DeletedByUserId = c.Guid(),
                    DeletedOn = c.DateTime(),
                    LockedByUserId = c.Guid(),
                    LockedOn = c.DateTime(),
                    SortOrder = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserNameLookups", t => t.CreatedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.DeletedByUserId)
                .ForeignKey("dbo.Entities", t => t.EntityId)
                .ForeignKey("dbo.UserNameLookups", t => t.LockedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.PublishedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.UpdatedByUserId)
                .Index(t => t.EntityId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.PublishedByUserId)
                .Index(t => t.DeletedByUserId)
                .Index(t => t.LockedByUserId);

            CreateTable(
                "dbo.News",
                c => new
                {
                    Id = c.Guid(nullable: false),
                    EntityId = c.Guid(nullable: false),
                    Summary = c.String(),
                    Body = c.String(nullable: false),
                    ExpireTime = c.DateTime(),
                    Culture = c.String(nullable: false, maxLength: 5),
                    VersionNumber = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 250),
                    VersionLog = c.String(maxLength: 1000),
                    IsCurrentVersion = c.Boolean(nullable: false),
                    NumberOfComments = c.Int(nullable: false),
                    LastCommentDate = c.DateTime(),
                    CreatedByUserId = c.Guid(nullable: false),
                    CreatedOn = c.DateTime(nullable: false),
                    UpdatedByUserId = c.Guid(nullable: false),
                    UpdatedOn = c.DateTime(nullable: false),
                    PublishedByUserId = c.Guid(),
                    PublishedOn = c.DateTime(),
                    DeletedByUserId = c.Guid(),
                    DeletedOn = c.DateTime(),
                    LockedByUserId = c.Guid(),
                    LockedOn = c.DateTime(),
                    SortOrder = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserNameLookups", t => t.CreatedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.DeletedByUserId)
                .ForeignKey("dbo.Entities", t => t.EntityId)
                .ForeignKey("dbo.UserNameLookups", t => t.LockedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.PublishedByUserId)
                .ForeignKey("dbo.UserNameLookups", t => t.UpdatedByUserId)
                .Index(t => t.EntityId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.PublishedByUserId)
                .Index(t => t.DeletedByUserId)
                .Index(t => t.LockedByUserId);

            CreateTable(
                "dbo.TaxonomyVocabularyEntityTypes",
                c => new
                {
                    VocabularyId = c.Guid(nullable: false),
                    EntityTypeId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.VocabularyId, t.EntityTypeId })
                .ForeignKey("dbo.TaxonomyVocabularies", t => t.VocabularyId, cascadeDelete: true)
                .ForeignKey("dbo.EntityTypes", t => t.EntityTypeId, cascadeDelete: true)
                .Index(t => t.VocabularyId)
                .Index(t => t.EntityTypeId);

            CreateTable(
                "dbo.TaxonomySiblingTerms",
                c => new
                {
                    TermId = c.Guid(nullable: false),
                    SiblingTermId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.TermId, t.SiblingTermId })
                .ForeignKey("dbo.TaxonomyTerms", t => t.TermId)
                .ForeignKey("dbo.TaxonomyTerms", t => t.SiblingTermId)
                .Index(t => t.TermId)
                .Index(t => t.SiblingTermId);

            CreateTable(
                "dbo.TaxonomyParentChildTerms",
                c => new
                {
                    TermId = c.Guid(nullable: false),
                    ParentTermId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.TermId, t.ParentTermId })
                .ForeignKey("dbo.TaxonomyTerms", t => t.TermId)
                .ForeignKey("dbo.TaxonomyTerms", t => t.ParentTermId)
                .Index(t => t.TermId)
                .Index(t => t.ParentTermId);

            CreateTable(
                "dbo.FileTerms",
                c => new
                {
                    FileId = c.Guid(nullable: false),
                    TermId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.FileId, t.TermId })
                .ForeignKey("dbo.Files", t => t.FileId, cascadeDelete: true)
                .ForeignKey("dbo.TaxonomyTerms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.FileId)
                .Index(t => t.TermId);

            CreateTable(
                "dbo.TaxonomyTermEntities",
                c => new
                {
                    EntityId = c.Guid(nullable: false),
                    TermId = c.Guid(nullable: false),
                })
                .PrimaryKey(t => new { t.EntityId, t.TermId })
                .ForeignKey("dbo.Entities", t => t.EntityId, cascadeDelete: true)
                .ForeignKey("dbo.TaxonomyTerms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.EntityId)
                .Index(t => t.TermId);
        }

        #endregion Public Methods
    }
}