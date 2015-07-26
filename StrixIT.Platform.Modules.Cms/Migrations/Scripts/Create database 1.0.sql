/**
 * Copyright 2015 StrixIT
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
CREATE TABLE [dbo].[Comments] (
    [Id] [bigint] NOT NULL IDENTITY,
    [EntityId] [uniqueidentifier] NOT NULL,
    [EntityCulture] [nvarchar](5) NOT NULL,
    [EntityVersion] [int] NOT NULL,
    [ParentId] [bigint],
    [CommentStatus] [int] NOT NULL,
    [Text] [nvarchar](max) NOT NULL,
    [CreatedByUserId] [uniqueidentifier] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedByUserId] [uniqueidentifier] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    CONSTRAINT [PK_dbo.Comments] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityId] ON [dbo].[Comments]([EntityId])
CREATE INDEX [IX_ParentId] ON [dbo].[Comments]([ParentId])
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[Comments]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[Comments]([UpdatedByUserId])
CREATE TABLE [dbo].[UserNameLookups] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [Email] [nvarchar](250) NOT NULL,
    CONSTRAINT [PK_dbo.UserNameLookups] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[Entities] (
    [Id] [uniqueidentifier] NOT NULL,
    [Url] [nvarchar](300) NOT NULL,
    [EntityTypeId] [uniqueidentifier] NOT NULL,
    [GroupId] [uniqueidentifier] NOT NULL,
    [OwnerUserId] [uniqueidentifier] NOT NULL,
    [IsPrivate] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Entities] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityTypeId] ON [dbo].[Entities]([EntityTypeId])
CREATE INDEX [IX_GroupId] ON [dbo].[Entities]([GroupId])
CREATE INDEX [IX_OwnerUserId] ON [dbo].[Entities]([OwnerUserId])
CREATE TABLE [dbo].[ContentSharedWithGroups] (
    [EntityId] [uniqueidentifier] NOT NULL,
    [GroupId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_dbo.ContentSharedWithGroups] PRIMARY KEY ([EntityId], [GroupId])
)
CREATE INDEX [IX_EntityId] ON [dbo].[ContentSharedWithGroups]([EntityId])
CREATE INDEX [IX_GroupId] ON [dbo].[ContentSharedWithGroups]([GroupId])
CREATE TABLE [dbo].[GroupNameLookups] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    CONSTRAINT [PK_dbo.GroupNameLookups] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[EntityTypes] (
    [Id] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    CONSTRAINT [PK_dbo.EntityTypes] PRIMARY KEY ([Id])
)
CREATE TABLE [dbo].[EntityCustomFields] (
    [Id] [uniqueidentifier] NOT NULL,
    [EntityTypeId] [uniqueidentifier] NOT NULL,
    [GroupId] [uniqueidentifier] NOT NULL,
    [FieldType] [int] NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Section] [nvarchar](100),
    CONSTRAINT [PK_dbo.EntityCustomFields] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityTypeId] ON [dbo].[EntityCustomFields]([EntityTypeId])
CREATE INDEX [IX_GroupId] ON [dbo].[EntityCustomFields]([GroupId])
CREATE TABLE [dbo].[EntityTypeServiceActions] (
    [Id] [int] NOT NULL IDENTITY,
    [EntityTypeId] [uniqueidentifier] NOT NULL,
    [GroupId] [uniqueidentifier] NOT NULL,
    [Action] [nvarchar](250) NOT NULL,
    CONSTRAINT [PK_dbo.EntityTypeServiceActions] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityTypeId] ON [dbo].[EntityTypeServiceActions]([EntityTypeId])
CREATE INDEX [IX_GroupId] ON [dbo].[EntityTypeServiceActions]([GroupId])
CREATE TABLE [dbo].[TaxonomyVocabularies] (
    [Id] [uniqueidentifier] NOT NULL,
    [GroupId] [uniqueidentifier] NOT NULL,
    [Culture] [nvarchar](5) NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [Url] [nvarchar](300) NOT NULL,
    [UseTermRelations] [bit] NOT NULL,
    [UseTermHierarchy] [bit] NOT NULL,
    [IsSystemVocabulary] [bit] NOT NULL,
    [UserExtensible] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.TaxonomyVocabularies] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_GroupId] ON [dbo].[TaxonomyVocabularies]([GroupId])
CREATE TABLE [dbo].[TaxonomyTerms] (
    [Id] [uniqueidentifier] NOT NULL,
    [VocabularyId] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [Url] [nvarchar](300) NOT NULL,
    [TagCount] [int] NOT NULL,
    CONSTRAINT [PK_dbo.TaxonomyTerms] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_VocabularyId] ON [dbo].[TaxonomyTerms]([VocabularyId])
CREATE TABLE [dbo].[TaxonomySynonyms] (
    [TermId] [uniqueidentifier] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    CONSTRAINT [PK_dbo.TaxonomySynonyms] PRIMARY KEY ([TermId], [Name])
)
CREATE INDEX [IX_TermId] ON [dbo].[TaxonomySynonyms]([TermId])
CREATE TABLE [dbo].[Files] (
    [Id] [uniqueidentifier] NOT NULL,
    [GroupId] [uniqueidentifier] NOT NULL,
    [Folder] [nvarchar](200) NOT NULL,
    [Path] [nvarchar](300) NOT NULL,
    [FileName] [nvarchar](200) NOT NULL,
    [Extension] [nvarchar](5) NOT NULL,
    [OriginalName] [nvarchar](300),
    [Size] [bigint],
    [UploadedOn] [datetime] NOT NULL,
    [UploadedByUserId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_dbo.Files] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_GroupId] ON [dbo].[Files]([GroupId])
CREATE INDEX [IX_UploadedByUserId] ON [dbo].[Files]([UploadedByUserId])
CREATE TABLE [dbo].[ContentCustomFieldValues] (
    [Id] [bigint] NOT NULL IDENTITY,
    [ContentId] [uniqueidentifier] NOT NULL,
    [CustomFieldId] [uniqueidentifier] NOT NULL,
    [NumberValue] [float],
    [StringValue] [nvarchar](max),
    CONSTRAINT [PK_dbo.ContentCustomFieldValues] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_CustomFieldId] ON [dbo].[ContentCustomFieldValues]([CustomFieldId])
CREATE TABLE [dbo].[Documents] (
    [Id] [uniqueidentifier] NOT NULL,
    [EntityId] [uniqueidentifier] NOT NULL,
    [FileId] [uniqueidentifier] NOT NULL,
    [Description] [nvarchar](max),
    [AuthorName] [nvarchar](250),
    [AuthorUserId] [uniqueidentifier],
    [Location] [nvarchar](250),
    [Date] [datetime],
    [DownloadCount] [int] NOT NULL,
    [Culture] [nvarchar](5) NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [VersionLog] [nvarchar](1000),
    [IsCurrentVersion] [bit] NOT NULL,
    [NumberOfComments] [int] NOT NULL,
    [LastCommentDate] [datetime],
    [CreatedByUserId] [uniqueidentifier] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedByUserId] [uniqueidentifier] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [PublishedByUserId] [uniqueidentifier],
    [PublishedOn] [datetime],
    [DeletedByUserId] [uniqueidentifier],
    [DeletedOn] [datetime],
    [LockedByUserId] [uniqueidentifier],
    [LockedOn] [datetime],
    [SortOrder] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Documents] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityId] ON [dbo].[Documents]([EntityId])
CREATE INDEX [IX_FileId] ON [dbo].[Documents]([FileId])
CREATE INDEX [IX_AuthorUserId] ON [dbo].[Documents]([AuthorUserId])
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[Documents]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[Documents]([UpdatedByUserId])
CREATE INDEX [IX_PublishedByUserId] ON [dbo].[Documents]([PublishedByUserId])
CREATE INDEX [IX_DeletedByUserId] ON [dbo].[Documents]([DeletedByUserId])
CREATE INDEX [IX_LockedByUserId] ON [dbo].[Documents]([LockedByUserId])
CREATE TABLE [dbo].[Html] (
    [Id] [uniqueidentifier] NOT NULL,
    [EntityId] [uniqueidentifier] NOT NULL,
    [Body] [nvarchar](max) NOT NULL,
    [Culture] [nvarchar](5) NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [VersionLog] [nvarchar](1000),
    [IsCurrentVersion] [bit] NOT NULL,
    [NumberOfComments] [int] NOT NULL,
    [LastCommentDate] [datetime],
    [CreatedByUserId] [uniqueidentifier] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedByUserId] [uniqueidentifier] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [PublishedByUserId] [uniqueidentifier],
    [PublishedOn] [datetime],
    [DeletedByUserId] [uniqueidentifier],
    [DeletedOn] [datetime],
    [LockedByUserId] [uniqueidentifier],
    [LockedOn] [datetime],
    [SortOrder] [int] NOT NULL,
    CONSTRAINT [PK_dbo.Html] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityId] ON [dbo].[Html]([EntityId])
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[Html]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[Html]([UpdatedByUserId])
CREATE INDEX [IX_PublishedByUserId] ON [dbo].[Html]([PublishedByUserId])
CREATE INDEX [IX_DeletedByUserId] ON [dbo].[Html]([DeletedByUserId])
CREATE INDEX [IX_LockedByUserId] ON [dbo].[Html]([LockedByUserId])
CREATE TABLE [dbo].[MailContents] (
    [Id] [uniqueidentifier] NOT NULL,
    [EntityId] [uniqueidentifier] NOT NULL,
    [TemplateId] [uniqueidentifier] NOT NULL,
    [From] [nvarchar](250) NOT NULL,
    [Subject] [nvarchar](250) NOT NULL,
    [Body] [nvarchar](max) NOT NULL,
    [Culture] [nvarchar](5) NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [VersionLog] [nvarchar](1000),
    [IsCurrentVersion] [bit] NOT NULL,
    [NumberOfComments] [int] NOT NULL,
    [LastCommentDate] [datetime],
    [CreatedByUserId] [uniqueidentifier] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedByUserId] [uniqueidentifier] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [PublishedByUserId] [uniqueidentifier],
    [PublishedOn] [datetime],
    [DeletedByUserId] [uniqueidentifier],
    [DeletedOn] [datetime],
    [LockedByUserId] [uniqueidentifier],
    [LockedOn] [datetime],
    [SortOrder] [int] NOT NULL,
    CONSTRAINT [PK_dbo.MailContents] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityId] ON [dbo].[MailContents]([EntityId])
CREATE INDEX [IX_TemplateId] ON [dbo].[MailContents]([TemplateId])
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[MailContents]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[MailContents]([UpdatedByUserId])
CREATE INDEX [IX_PublishedByUserId] ON [dbo].[MailContents]([PublishedByUserId])
CREATE INDEX [IX_DeletedByUserId] ON [dbo].[MailContents]([DeletedByUserId])
CREATE INDEX [IX_LockedByUserId] ON [dbo].[MailContents]([LockedByUserId])
CREATE TABLE [dbo].[MailContentTemplates] (
    [Id] [uniqueidentifier] NOT NULL,
    [EntityId] [uniqueidentifier] NOT NULL,
    [Body] [nvarchar](max) NOT NULL,
    [Culture] [nvarchar](5) NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [VersionLog] [nvarchar](1000),
    [IsCurrentVersion] [bit] NOT NULL,
    [NumberOfComments] [int] NOT NULL,
    [LastCommentDate] [datetime],
    [CreatedByUserId] [uniqueidentifier] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedByUserId] [uniqueidentifier] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [PublishedByUserId] [uniqueidentifier],
    [PublishedOn] [datetime],
    [DeletedByUserId] [uniqueidentifier],
    [DeletedOn] [datetime],
    [LockedByUserId] [uniqueidentifier],
    [LockedOn] [datetime],
    [SortOrder] [int] NOT NULL,
    CONSTRAINT [PK_dbo.MailContentTemplates] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityId] ON [dbo].[MailContentTemplates]([EntityId])
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[MailContentTemplates]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[MailContentTemplates]([UpdatedByUserId])
CREATE INDEX [IX_PublishedByUserId] ON [dbo].[MailContentTemplates]([PublishedByUserId])
CREATE INDEX [IX_DeletedByUserId] ON [dbo].[MailContentTemplates]([DeletedByUserId])
CREATE INDEX [IX_LockedByUserId] ON [dbo].[MailContentTemplates]([LockedByUserId])
CREATE TABLE [dbo].[News] (
    [Id] [uniqueidentifier] NOT NULL,
    [EntityId] [uniqueidentifier] NOT NULL,
    [Summary] [nvarchar](max),
    [Body] [nvarchar](max) NOT NULL,
    [ExpireTime] [datetime],
    [Culture] [nvarchar](5) NOT NULL,
    [VersionNumber] [int] NOT NULL,
    [Name] [nvarchar](250) NOT NULL,
    [VersionLog] [nvarchar](1000),
    [IsCurrentVersion] [bit] NOT NULL,
    [NumberOfComments] [int] NOT NULL,
    [LastCommentDate] [datetime],
    [CreatedByUserId] [uniqueidentifier] NOT NULL,
    [CreatedOn] [datetime] NOT NULL,
    [UpdatedByUserId] [uniqueidentifier] NOT NULL,
    [UpdatedOn] [datetime] NOT NULL,
    [PublishedByUserId] [uniqueidentifier],
    [PublishedOn] [datetime],
    [DeletedByUserId] [uniqueidentifier],
    [DeletedOn] [datetime],
    [LockedByUserId] [uniqueidentifier],
    [LockedOn] [datetime],
    [SortOrder] [int] NOT NULL,
    CONSTRAINT [PK_dbo.News] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_EntityId] ON [dbo].[News]([EntityId])
CREATE INDEX [IX_CreatedByUserId] ON [dbo].[News]([CreatedByUserId])
CREATE INDEX [IX_UpdatedByUserId] ON [dbo].[News]([UpdatedByUserId])
CREATE INDEX [IX_PublishedByUserId] ON [dbo].[News]([PublishedByUserId])
CREATE INDEX [IX_DeletedByUserId] ON [dbo].[News]([DeletedByUserId])
CREATE INDEX [IX_LockedByUserId] ON [dbo].[News]([LockedByUserId])
CREATE TABLE [dbo].[TaxonomyVocabularyEntityTypes] (
    [VocabularyId] [uniqueidentifier] NOT NULL,
    [EntityTypeId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_dbo.TaxonomyVocabularyEntityTypes] PRIMARY KEY ([VocabularyId], [EntityTypeId])
)
CREATE INDEX [IX_VocabularyId] ON [dbo].[TaxonomyVocabularyEntityTypes]([VocabularyId])
CREATE INDEX [IX_EntityTypeId] ON [dbo].[TaxonomyVocabularyEntityTypes]([EntityTypeId])
CREATE TABLE [dbo].[TaxonomySiblingTerms] (
    [TermId] [uniqueidentifier] NOT NULL,
    [SiblingTermId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_dbo.TaxonomySiblingTerms] PRIMARY KEY ([TermId], [SiblingTermId])
)
CREATE INDEX [IX_TermId] ON [dbo].[TaxonomySiblingTerms]([TermId])
CREATE INDEX [IX_SiblingTermId] ON [dbo].[TaxonomySiblingTerms]([SiblingTermId])
CREATE TABLE [dbo].[TaxonomyParentChildTerms] (
    [TermId] [uniqueidentifier] NOT NULL,
    [ParentTermId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_dbo.TaxonomyParentChildTerms] PRIMARY KEY ([TermId], [ParentTermId])
)
CREATE INDEX [IX_TermId] ON [dbo].[TaxonomyParentChildTerms]([TermId])
CREATE INDEX [IX_ParentTermId] ON [dbo].[TaxonomyParentChildTerms]([ParentTermId])
CREATE TABLE [dbo].[FileTerms] (
    [FileId] [uniqueidentifier] NOT NULL,
    [TermId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_dbo.FileTerms] PRIMARY KEY ([FileId], [TermId])
)
CREATE INDEX [IX_FileId] ON [dbo].[FileTerms]([FileId])
CREATE INDEX [IX_TermId] ON [dbo].[FileTerms]([TermId])
CREATE TABLE [dbo].[TaxonomyTermEntities] (
    [EntityId] [uniqueidentifier] NOT NULL,
    [TermId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [PK_dbo.TaxonomyTermEntities] PRIMARY KEY ([EntityId], [TermId])
)
CREATE INDEX [IX_EntityId] ON [dbo].[TaxonomyTermEntities]([EntityId])
CREATE INDEX [IX_TermId] ON [dbo].[TaxonomyTermEntities]([TermId])
ALTER TABLE [dbo].[Comments] ADD CONSTRAINT [FK_dbo.Comments_dbo.Comments_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Comments] ([Id])
ALTER TABLE [dbo].[Comments] ADD CONSTRAINT [FK_dbo.Comments_dbo.UserNameLookups_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Comments] ADD CONSTRAINT [FK_dbo.Comments_dbo.Entities_EntityId] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Comments] ADD CONSTRAINT [FK_dbo.Comments_dbo.UserNameLookups_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Entities] ADD CONSTRAINT [FK_dbo.Entities_dbo.EntityTypes_EntityTypeId] FOREIGN KEY ([EntityTypeId]) REFERENCES [dbo].[EntityTypes] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Entities] ADD CONSTRAINT [FK_dbo.Entities_dbo.GroupNameLookups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[GroupNameLookups] ([Id])
ALTER TABLE [dbo].[Entities] ADD CONSTRAINT [FK_dbo.Entities_dbo.UserNameLookups_OwnerUserId] FOREIGN KEY ([OwnerUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[ContentSharedWithGroups] ADD CONSTRAINT [FK_dbo.ContentSharedWithGroups_dbo.Entities_EntityId] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ContentSharedWithGroups] ADD CONSTRAINT [FK_dbo.ContentSharedWithGroups_dbo.GroupNameLookups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[GroupNameLookups] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[EntityCustomFields] ADD CONSTRAINT [FK_dbo.EntityCustomFields_dbo.GroupNameLookups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[GroupNameLookups] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[EntityCustomFields] ADD CONSTRAINT [FK_dbo.EntityCustomFields_dbo.EntityTypes_EntityTypeId] FOREIGN KEY ([EntityTypeId]) REFERENCES [dbo].[EntityTypes] ([Id])
ALTER TABLE [dbo].[EntityTypeServiceActions] ADD CONSTRAINT [FK_dbo.EntityTypeServiceActions_dbo.EntityTypes_EntityTypeId] FOREIGN KEY ([EntityTypeId]) REFERENCES [dbo].[EntityTypes] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[EntityTypeServiceActions] ADD CONSTRAINT [FK_dbo.EntityTypeServiceActions_dbo.GroupNameLookups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[GroupNameLookups] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TaxonomyVocabularies] ADD CONSTRAINT [FK_dbo.TaxonomyVocabularies_dbo.GroupNameLookups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[GroupNameLookups] ([Id])
ALTER TABLE [dbo].[TaxonomyTerms] ADD CONSTRAINT [FK_dbo.TaxonomyTerms_dbo.TaxonomyVocabularies_VocabularyId] FOREIGN KEY ([VocabularyId]) REFERENCES [dbo].[TaxonomyVocabularies] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TaxonomySynonyms] ADD CONSTRAINT [FK_dbo.TaxonomySynonyms_dbo.TaxonomyTerms_TermId] FOREIGN KEY ([TermId]) REFERENCES [dbo].[TaxonomyTerms] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [FK_dbo.Files_dbo.GroupNameLookups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[GroupNameLookups] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Files] ADD CONSTRAINT [FK_dbo.Files_dbo.UserNameLookups_UploadedByUserId] FOREIGN KEY ([UploadedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[ContentCustomFieldValues] ADD CONSTRAINT [FK_dbo.ContentCustomFieldValues_dbo.EntityCustomFields_CustomFieldId] FOREIGN KEY ([CustomFieldId]) REFERENCES [dbo].[EntityCustomFields] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Documents] ADD CONSTRAINT [FK_dbo.Documents_dbo.UserNameLookups_AuthorUserId] FOREIGN KEY ([AuthorUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Documents] ADD CONSTRAINT [FK_dbo.Documents_dbo.UserNameLookups_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Documents] ADD CONSTRAINT [FK_dbo.Documents_dbo.UserNameLookups_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Documents] ADD CONSTRAINT [FK_dbo.Documents_dbo.Entities_EntityId] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([Id])
ALTER TABLE [dbo].[Documents] ADD CONSTRAINT [FK_dbo.Documents_dbo.Files_FileId] FOREIGN KEY ([FileId]) REFERENCES [dbo].[Files] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[Documents] ADD CONSTRAINT [FK_dbo.Documents_dbo.UserNameLookups_LockedByUserId] FOREIGN KEY ([LockedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Documents] ADD CONSTRAINT [FK_dbo.Documents_dbo.UserNameLookups_PublishedByUserId] FOREIGN KEY ([PublishedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Documents] ADD CONSTRAINT [FK_dbo.Documents_dbo.UserNameLookups_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Html] ADD CONSTRAINT [FK_dbo.Html_dbo.UserNameLookups_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Html] ADD CONSTRAINT [FK_dbo.Html_dbo.UserNameLookups_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Html] ADD CONSTRAINT [FK_dbo.Html_dbo.Entities_EntityId] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([Id])
ALTER TABLE [dbo].[Html] ADD CONSTRAINT [FK_dbo.Html_dbo.UserNameLookups_LockedByUserId] FOREIGN KEY ([LockedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Html] ADD CONSTRAINT [FK_dbo.Html_dbo.UserNameLookups_PublishedByUserId] FOREIGN KEY ([PublishedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[Html] ADD CONSTRAINT [FK_dbo.Html_dbo.UserNameLookups_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContents] ADD CONSTRAINT [FK_dbo.MailContents_dbo.UserNameLookups_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContents] ADD CONSTRAINT [FK_dbo.MailContents_dbo.UserNameLookups_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContents] ADD CONSTRAINT [FK_dbo.MailContents_dbo.Entities_EntityId] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([Id])
ALTER TABLE [dbo].[MailContents] ADD CONSTRAINT [FK_dbo.MailContents_dbo.UserNameLookups_LockedByUserId] FOREIGN KEY ([LockedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContents] ADD CONSTRAINT [FK_dbo.MailContents_dbo.UserNameLookups_PublishedByUserId] FOREIGN KEY ([PublishedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContents] ADD CONSTRAINT [FK_dbo.MailContents_dbo.MailContentTemplates_TemplateId] FOREIGN KEY ([TemplateId]) REFERENCES [dbo].[MailContentTemplates] ([Id])
ALTER TABLE [dbo].[MailContents] ADD CONSTRAINT [FK_dbo.MailContents_dbo.UserNameLookups_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContentTemplates] ADD CONSTRAINT [FK_dbo.MailContentTemplates_dbo.UserNameLookups_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContentTemplates] ADD CONSTRAINT [FK_dbo.MailContentTemplates_dbo.UserNameLookups_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContentTemplates] ADD CONSTRAINT [FK_dbo.MailContentTemplates_dbo.Entities_EntityId] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([Id])
ALTER TABLE [dbo].[MailContentTemplates] ADD CONSTRAINT [FK_dbo.MailContentTemplates_dbo.UserNameLookups_LockedByUserId] FOREIGN KEY ([LockedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContentTemplates] ADD CONSTRAINT [FK_dbo.MailContentTemplates_dbo.UserNameLookups_PublishedByUserId] FOREIGN KEY ([PublishedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[MailContentTemplates] ADD CONSTRAINT [FK_dbo.MailContentTemplates_dbo.UserNameLookups_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[News] ADD CONSTRAINT [FK_dbo.News_dbo.UserNameLookups_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[News] ADD CONSTRAINT [FK_dbo.News_dbo.UserNameLookups_DeletedByUserId] FOREIGN KEY ([DeletedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[News] ADD CONSTRAINT [FK_dbo.News_dbo.Entities_EntityId] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([Id])
ALTER TABLE [dbo].[News] ADD CONSTRAINT [FK_dbo.News_dbo.UserNameLookups_LockedByUserId] FOREIGN KEY ([LockedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[News] ADD CONSTRAINT [FK_dbo.News_dbo.UserNameLookups_PublishedByUserId] FOREIGN KEY ([PublishedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[News] ADD CONSTRAINT [FK_dbo.News_dbo.UserNameLookups_UpdatedByUserId] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [dbo].[UserNameLookups] ([Id])
ALTER TABLE [dbo].[TaxonomyVocabularyEntityTypes] ADD CONSTRAINT [FK_dbo.TaxonomyVocabularyEntityTypes_dbo.TaxonomyVocabularies_VocabularyId] FOREIGN KEY ([VocabularyId]) REFERENCES [dbo].[TaxonomyVocabularies] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TaxonomyVocabularyEntityTypes] ADD CONSTRAINT [FK_dbo.TaxonomyVocabularyEntityTypes_dbo.EntityTypes_EntityTypeId] FOREIGN KEY ([EntityTypeId]) REFERENCES [dbo].[EntityTypes] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TaxonomySiblingTerms] ADD CONSTRAINT [FK_dbo.TaxonomySiblingTerms_dbo.TaxonomyTerms_TermId] FOREIGN KEY ([TermId]) REFERENCES [dbo].[TaxonomyTerms] ([Id])
ALTER TABLE [dbo].[TaxonomySiblingTerms] ADD CONSTRAINT [FK_dbo.TaxonomySiblingTerms_dbo.TaxonomyTerms_SiblingTermId] FOREIGN KEY ([SiblingTermId]) REFERENCES [dbo].[TaxonomyTerms] ([Id])
ALTER TABLE [dbo].[TaxonomyParentChildTerms] ADD CONSTRAINT [FK_dbo.TaxonomyParentChildTerms_dbo.TaxonomyTerms_TermId] FOREIGN KEY ([TermId]) REFERENCES [dbo].[TaxonomyTerms] ([Id])
ALTER TABLE [dbo].[TaxonomyParentChildTerms] ADD CONSTRAINT [FK_dbo.TaxonomyParentChildTerms_dbo.TaxonomyTerms_ParentTermId] FOREIGN KEY ([ParentTermId]) REFERENCES [dbo].[TaxonomyTerms] ([Id])
ALTER TABLE [dbo].[FileTerms] ADD CONSTRAINT [FK_dbo.FileTerms_dbo.Files_FileId] FOREIGN KEY ([FileId]) REFERENCES [dbo].[Files] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[FileTerms] ADD CONSTRAINT [FK_dbo.FileTerms_dbo.TaxonomyTerms_TermId] FOREIGN KEY ([TermId]) REFERENCES [dbo].[TaxonomyTerms] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TaxonomyTermEntities] ADD CONSTRAINT [FK_dbo.TaxonomyTermEntities_dbo.Entities_EntityId] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entities] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[TaxonomyTermEntities] ADD CONSTRAINT [FK_dbo.TaxonomyTermEntities_dbo.TaxonomyTerms_TermId] FOREIGN KEY ([TermId]) REFERENCES [dbo].[TaxonomyTerms] ([Id]) ON DELETE CASCADE