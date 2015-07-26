#region Apache License

//-----------------------------------------------------------------------
// <copyright file="PlatformDataSource.cs" company="StrixIT">
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

#endregion Apache License

using StrixIT.Platform.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace StrixIT.Platform.Modules.Cms
{
    public class PlatformDataSource : EntityFrameworkDataSource, IPlatformDataSource
    {
        #region Private Fields

        private ICacheService _cache;
        private IFileSystemWrapper _fileSystemWrapper;

        #endregion Private Fields

        #region Constructors

        public PlatformDataSource(IFileSystemWrapper fileSystemWrapper, ICacheService cache)
            : base(CmsConstants.CMS)
        {
            this._fileSystemWrapper = fileSystemWrapper;
            this._cache = cache;
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        protected PlatformDataSource(string connectionStringName) : base(connectionStringName)
        {
        }

        private PlatformDataSource() : base(CmsConstants.CMS)
        {
        }

        #endregion Constructors

        #region Public Properties

        public IFileSystemWrapper FileSystemWrapper
        {
            get
            {
                return this._fileSystemWrapper;
            }
        }

        #endregion Public Properties

        #region DbSets

        /// <summary>
        /// Gets or sets the Comment entities.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the entity custom field values.
        /// </summary>
        public DbSet<ContentCustomFieldValue> ContentCustomFieldValues { get; set; }

        /// <summary>
        /// Gets or sets the Documents entities.
        /// </summary>
        public DbSet<Document> Documents { get; set; }

        /// <summary>
        /// Gets or sets the PlatformSettings entities.
        /// </summary>
        public DbSet<PlatformEntity> Entities { get; set; }

        /// <summary>
        /// Gets or sets the entity custom fields.
        /// </summary>
        public DbSet<EntityCustomField> EntityCustomFields { get; set; }

        /// <summary>
        /// Gets or sets the EntityType entities.
        /// </summary>
        public DbSet<EntityType> EntityTypes { get; set; }

        /// <summary>
        /// Gets or sets the EntityTypeServiceAction entities.
        /// </summary>
        public DbSet<EntityTypeServiceAction> EntityTypeServiceActions { get; set; }

        /// <summary>
        /// Gets or sets the File entities.
        /// </summary>
        public DbSet<File> Files { get; set; }

        /// <summary>
        /// Gets or sets the group name lookup entities.
        /// </summary>
        public DbSet<GroupData> GroupNameLookups { get; set; }

        /// <summary>
        /// Gets or sets the Html entities.
        /// </summary>
        public DbSet<Html> Html { get; set; }

        /// <summary>
        /// Gets or sets the Mail entities.
        /// </summary>
        public DbSet<MailContent> Mails { get; set; }

        /// <summary>
        /// Gets or sets the MailTemplate entities.
        /// </summary>
        public DbSet<MailContentTemplate> MailTemplates { get; set; }

        /// <summary>
        /// Gets or sets the News entities.
        /// </summary>
        public DbSet<News> News { get; set; }

        /// <summary>
        /// Gets or sets the Term entities.
        /// </summary>
        public DbSet<Term> Terms { get; set; }

        /// <summary>
        /// Gets or sets the user name lookup entities.
        /// </summary>
        public DbSet<UserData> UserNameLookups { get; set; }

        /// <summary>
        /// Gets or sets the Vocabulary entities.
        /// </summary>
        public DbSet<Vocabulary> Vocabularies { get; set; }

        #endregion DbSets

        #region Public Methods

        public static PlatformDataSource CreateForMigrations()
        {
            return new PlatformDataSource();
        }

        public IList GetExistingManyToManyRelations(object entity, string propertyName)
        {
            var relations = entity.GetPropertyValue(propertyName) as IEnumerable;
            var relationType = relations.AsQueryable().ElementType;
            var relationKeyProperties = this.GetKeyProperties(relationType);
            var keys = this.GetKeyValues(relationType, relations);

            if (keys.Count == 0)
            {
                return null;
            }

            // Construct the where clause.
            StringBuilder lineBuilder;
            StringBuilder whereBuilder = new StringBuilder();
            var index = 0;

            foreach (var key in keys)
            {
                if (whereBuilder.Length > 0)
                {
                    whereBuilder.Append(" Or ");
                }

                lineBuilder = new StringBuilder();

                foreach (var keyProperty in relationKeyProperties)
                {
                    if (lineBuilder.Length > 0)
                    {
                        lineBuilder.Append(" And ");
                    }

                    lineBuilder.Append(string.Format("{0}.Equals(@{1})", keyProperty, index));

                    index++;
                }

                whereBuilder.Append(string.Format("({0})", lineBuilder));
            }

            var allKeys = new List<object>();

            foreach (var key in keys)
            {
                allKeys.AddRange(key);
            }

            // Use the where clause and all keys to create the query.
            var query = this.Query(relationType).Where(whereBuilder.ToString(), allKeys.ToArray());
            var list = typeof(Enumerable).GetMethod("ToList").MakeGenericMethod(new Type[] { relationType }).Invoke(null, new object[] { query }) as IList;
            return list;
        }

        public TKey GetKeyValue<TKey>(object entity)
        {
            return (TKey)this.GetKeyValues(entity.GetType(), new object[] { entity }).First().First();
        }

        public string[] GetManyToManyRelations(Type entityType)
        {
            entityType = ObjectContext.GetObjectType(entityType);
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var item = objectContext.MetadataWorkspace.GetItem<System.Data.Entity.Core.Metadata.Edm.EntityType>(entityType.FullName, DataSpace.OSpace);
            var navProperties = item.NavigationProperties.ToList();
            var props = navProperties.Where(np => np.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many && np.FromEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many).Select(p => p.Name).ToArray();
            return props;
        }

        public IList<ModifiedPropertyValue> GetModifiedPropertyValues(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            List<ModifiedPropertyValue> list = new List<ModifiedPropertyValue>();
            var entry = this.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
                entry.GetDatabaseValues();
            }

            foreach (string property in entry.CurrentValues.PropertyNames)
            {
                object current = entry.CurrentValues[property];
                object original = entry.OriginalValues[property];

                list.AddRange(GetChangedProperties(property, current, original));
            }

            // Check many-on-many relations for changes.
            list.AddRange(this.GetChangedRelations(entity));

            return list;
        }

        public void Ignore(object entity)
        {
            var entry = this.Entry(entity);
            entry.State = System.Data.Entity.EntityState.Detached;
        }

        public IQueryable<T> Query<T>(string includes) where T : class
        {
            IQueryable query = base.Query<T>();
            query = this.AddIncludes(query, includes);
            query = this.SecureQuery(query);
            return query.Cast<T>();
        }

        public IQueryable Query(Type entityType)
        {
            var query = this.Set(entityType).AsQueryable();
            query = this.SecureQuery(query);
            return query;
        }

        public IQueryable Query(Type entityType, string includes)
        {
            var query = this.Set(entityType).AsQueryable();
            query = this.AddIncludes(query, includes);
            query = this.SecureQuery(query);
            return query;
        }

        #endregion Public Methods

        #region SaveChanges

        public new void SaveChanges()
        {
            try
            {
                var modifiedEntityTypes = this.ChangeTracker.Entries().Where(en => en.Entity != null && (en.State.Equals(EntityState.Added) || en.State.Equals(EntityState.Modified) || en.State.Equals(EntityState.Deleted))).Select(en => en.Entity.GetType()).Distinct().ToArray();

                base.SaveChanges();

                this._fileSystemWrapper.ClearSavedQueue();

                // Delete all files marked for deletion.
                this._fileSystemWrapper.ProcessDeleteQueue();

                foreach (var type in modifiedEntityTypes)
                {
                    var entityType = ObjectContext.GetObjectType(type);
                    this._cache.Delete(string.Format(CmsConstants.CONTENTPERCULTURE, entityType.Name));
                    this._cache.Delete(string.Format(CmsConstants.LOOKUPPERCULTURE, entityType.Name));
                }
            }
            catch (Exception ex)
            {
                this._fileSystemWrapper.RemoveFilesInSavedQueue();
                this._fileSystemWrapper.ClearDeleteQueue();
                Logger.Log("An error occurred while saving the context changes.", ex, LogLevel.Fatal);
                throw;
            }
        }

        #endregion SaveChanges

        #region Protected Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            modelBuilder.Entity<UserData>().ToTable("UserNameLookups");
            modelBuilder.Entity<GroupData>().ToTable("GroupNameLookups");
            modelBuilder.Entity<ContentSharedWithGroup>().HasKey(c => new { c.EntityId, c.GroupId });
            modelBuilder.Entity<EntityType>().HasMany(e => e.CustomFields).WithRequired(c => c.EntityType).WillCascadeOnDelete(false);
            modelBuilder.Entity<PlatformEntity>().HasRequired(e => e.Group).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<PlatformEntity>().HasRequired(e => e.OwnerUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<PlatformEntity>().HasMany(e => e.Comments).WithRequired(a => a.Entity).HasForeignKey(e => e.EntityId);
            modelBuilder.Entity<PlatformEntity>().ToTable("Entities");
            modelBuilder.Entity<Html>().ToTable("Html");
            modelBuilder.Entity<MailContent>().HasRequired(m => m.Template).WithMany(t => t.Mails).HasForeignKey(m => m.TemplateId).WillCascadeOnDelete(false);
            modelBuilder.Entity<File>().HasMany(e => e.Tags).WithMany(a => a.TaggedFiles).Map(ma => ma.MapLeftKey("FileId").MapRightKey("TermId").ToTable("FileTerms"));
            modelBuilder.Entity<File>().HasRequired(f => f.UploadedByUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>().HasRequired(f => f.CreatedByUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Comment>().HasRequired(f => f.UpdatedByUser).WithMany().WillCascadeOnDelete(false);

            // Taxonomy
            modelBuilder.Entity<Term>().ToTable("TaxonomyTerms");
            modelBuilder.Entity<Term>().HasMany(tt => tt.Parents).WithMany(tt => tt.Children).Map(ma => ma.MapLeftKey("TermId").MapRightKey("ParentTermId").ToTable("TaxonomyParentChildTerms"));
            modelBuilder.Entity<Term>().HasMany(tt => tt.OtherSiblingTerms).WithMany(tt => tt.SiblingTerms).Map(ma => ma.MapLeftKey("TermId").MapRightKey("SiblingTermId").ToTable("TaxonomySiblingTerms"));
            modelBuilder.Entity<Vocabulary>().ToTable("TaxonomyVocabularies");
            modelBuilder.Entity<Vocabulary>().HasRequired(v => v.Group).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Vocabulary>().HasMany(t => t.EntityTypes).WithMany(e => e.Vocabularies).Map(ma => ma.MapLeftKey("VocabularyId").MapRightKey("EntityTypeId").ToTable("TaxonomyVocabularyEntityTypes"));
            modelBuilder.Entity<Synonym>().ToTable("TaxonomySynonyms");
            modelBuilder.Entity<Synonym>().HasKey(s => new { s.TermId, s.Name });
            modelBuilder.Entity<PlatformEntity>().HasMany(e => e.Tags).WithMany(a => a.TaggedEntities).Map(ma => ma.MapLeftKey("EntityId").MapRightKey("TermId").ToTable("TaxonomyTermEntities"));

            ReadEntityTypeConfigurations(modelBuilder);

            // Disable cascade for entity and create and update user relations on content base
            // entities. This is needed to prevent multiple cascade errors when generating the
            // database and (for the entity relation) being unable to keep the
            // ManyToManyCascadeDeleteConvention for relations between entities (as used in e.g. the
            // Path of Heroes module).
            foreach (var contentType in ModuleManager.GetTypeList(typeof(ContentBase)).Where(t => !t.Equals(typeof(ContentBase))))
            {
                var entityMethodResult = typeof(DbModelBuilder).GetMethod("Entity").MakeGenericMethod(contentType).Invoke(modelBuilder, null);
                var hasRequiredEntityCall = entityMethodResult.GetType().GetMethod("HasRequired").MakeGenericMethod(typeof(PlatformEntity));
                var hasRequiredUserCall = entityMethodResult.GetType().GetMethod("HasRequired").MakeGenericMethod(typeof(UserData));
                DisableCascade(contentType, entityMethodResult, hasRequiredEntityCall, "Entity", typeof(PlatformEntity));
                DisableCascade(contentType, entityMethodResult, hasRequiredUserCall, "CreatedByUser", typeof(UserData));
                DisableCascade(contentType, entityMethodResult, hasRequiredUserCall, "UpdatedByUser", typeof(UserData));
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override IQueryable SecureQuery(IQueryable query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var entityType = query.ElementType;
            var isContent = typeof(IContent).IsAssignableFrom(entityType);
            var currentGroupId = StrixPlatform.User.GroupId;
            string prefix = string.Empty;

            if (isContent)
            {
                prefix = "Entity.";
            }

            if (isContent || typeof(PlatformEntity).IsAssignableFrom(entityType))
            {
                var currentUserId = StrixPlatform.User.Id;

                if (currentUserId != null)
                {
                    query = query.Where(string.Format("!{0}IsPrivate Or {0}OwnerUserId.Equals(@0)", prefix), currentUserId);
                }

                query = query.Where(string.Format("{0}GroupId.Equals(@0)", prefix), currentGroupId);
            }

            if (typeof(File).IsAssignableFrom(entityType))
            {
                query = query.Where("GroupId.Equals(@0)", currentGroupId);
            }

            return query;
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Disables cascades for usernamelookups.
        /// </summary>
        /// <param name="contentType">The content type to disable the cascade for</param>
        /// <param name="entityMethodResult">The entity method result</param>
        /// <param name="hasRequiredCall">The hasrequired method info</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="propertyType">The property type</param>
        private static void DisableCascade(Type contentType, object entityMethodResult, MethodInfo hasRequiredCall, string propertyName, Type propertyType)
        {
            var hasRequiredResult = hasRequiredCall.Invoke(entityMethodResult, new object[] { GetRequiredPropertyTypeExpression(contentType, propertyName, propertyType) });
            var withManyResult = hasRequiredResult.GetType().GetMethods().First(m => m.Name == "WithMany" && m.GetParameters().Count() == 0).Invoke(hasRequiredResult, null);
            withManyResult.GetType().GetMethods().First(m => m.Name == "WillCascadeOnDelete" && m.GetParameters().Count() == 1).Invoke(withManyResult, new object[] { false });
        }

        /// <summary>
        /// Gets the changed properties recursively.
        /// </summary>
        /// <param name="propertyName">The property name</param>
        /// <param name="current">The current property value</param>
        /// <param name="original">The original property value</param>
        /// <returns>True if the property has changed, false otherwise</returns>
        private static List<ModifiedPropertyValue> GetChangedProperties(string propertyName, object current, object original)
        {
            var list = new List<ModifiedPropertyValue>();

            var currentPropertyValues = current as DbPropertyValues;

            // If the property is a complex type, check all its sub properties.
            if (currentPropertyValues != null)
            {
                foreach (var innerProperty in currentPropertyValues.PropertyNames)
                {
                    object innerCurrent = currentPropertyValues[innerProperty];
                    object innerOriginal = ((DbPropertyValues)original)[innerProperty];

                    list.AddRange(GetChangedProperties(propertyName + "." + innerProperty, innerCurrent, innerOriginal));
                }
            }
            else if (!(current == null && original == null) && (current != null && original != null && !current.Equals(original)))
            {
                list.Add(new ModifiedPropertyValue { PropertyName = propertyName, NewValue = current, OldValue = original });
            }

            return list;
        }

        /// <summary>
        /// Gets a lambda for getting the relation property for a select many query.
        /// </summary>
        /// <param name="entityType">The type of the entity the query is for</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="propertyType">The property type</param>
        /// <returns>The relation property lambda</returns>
        private static LambdaExpression GetPropertyExpression(Type entityType, string propertyName, Type propertyType)
        {
            ParameterExpression parameter = Expression.Parameter(entityType, "i");
            MemberExpression property = Expression.Property(parameter, propertyName);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(propertyType);
            var delegateType = typeof(Func<,>).MakeGenericType(entityType, enumerableType);
            return Expression.Lambda(delegateType, property, parameter);
        }

        /// <summary>
        /// Gets the key values for many-on-many related entities.
        /// </summary>
        /// <param name="query">The related entity query</param>
        /// <param name="select">The key value select clause</param>
        /// <returns>A list of key values dictionaries</returns>
        private static List<IDictionary<string, object>> GetRelationValues(IQueryable query, string select)
        {
            var results = query.Select(string.Format("new ( {0} )", select));
            var resultProperties = results.ElementType.GetProperties();
            var dictionaryList = new List<IDictionary<string, object>>();

            foreach (var result in results)
            {
                var dictionary = new Dictionary<string, object>();

                foreach (var resultProperty in resultProperties)
                {
                    dictionary.Add(resultProperty.Name, result.GetPropertyValue(resultProperty.Name));
                }

                dictionaryList.Add(dictionary);
            }

            return dictionaryList;
        }

        /// <summary>
        /// Gets the expression to get for a required relation.
        /// </summary>
        /// <param name="contentType">The content type to disable the cascade for</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="propertyType">The property type</param>
        /// <returns>The expression</returns>
        private static LambdaExpression GetRequiredPropertyTypeExpression(Type contentType, string propertyName, Type propertyType)
        {
            ParameterExpression parameter = Expression.Parameter(contentType, "i");
            MemberExpression property = Expression.Property(parameter, propertyName);
            var delegateType = typeof(Func<,>).MakeGenericType(contentType, propertyType);
            return Expression.Lambda(delegateType, property, parameter);
        }

        /// <summary>
        /// Reads all entity type configurations to add externally defined entities to the model.
        /// Used for modules.
        /// </summary>
        /// <param name="modelBuilder">The model builder for the model</param>
        private static void ReadEntityTypeConfigurations(DbModelBuilder modelBuilder)
        {
            // Add all objects configured using EntityTypeConfigurations from all loaded assemblies
            // to the model.
            var addMethod = typeof(ConfigurationRegistrar).GetMethods().Single(m => m.Name == "Add" && m.GetGenericArguments().Any(a => a.Name == "TEntityType"));

            foreach (var assembly in ModuleManager.LoadedAssemblies.Where(a => a.GetName().Name != "EntityFramework"))
            {
                var configTypes = assembly.GetTypes().Where(t => t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>)).ToList();
                object config = null;

                foreach (var type in configTypes)
                {
                    // Get the object type.
                    var objectType = type.BaseType.GetGenericArguments().Single();

                    // Create a configuration object.
                    config = Activator.CreateInstance(type);

                    // Add the configuration object to the model.
                    addMethod.MakeGenericMethod(objectType).Invoke(modelBuilder.Configurations, new object[] { config });
                }
            }
        }

        private IQueryable AddIncludes(IQueryable query, string includes)
        {
            if (!string.IsNullOrWhiteSpace(includes))
            {
                foreach (var include in includes.Replace(" ", string.Empty).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Trim())
                {
                    query = query.Include(include);
                }
            }

            return query;
        }

        /// <summary>
        /// Build a select many query for many-on-many relations.
        /// </summary>
        /// <param name="entityType">The entity type to build the query for</param>
        /// <param name="propertyType">The property type for the select many</param>
        /// <param name="propertyName">The property name for the select many</param>
        /// <param name="where">The where clause to select the entity to use the select many on</param>
        /// <param name="keyValues">The entity key values</param>
        /// <returns>The select many query</returns>
        private IQueryable BuildRelationQuery(Type entityType, Type propertyType, string propertyName, string where, object[] keyValues)
        {
            var query = this.Set(entityType).Where(where, keyValues);
            var selectManyMethod = typeof(Queryable).GetMethods().Where(m => m.Name == "SelectMany" && m.GetGenericArguments().Count() == 2).First();
            var nonGeneric = selectManyMethod.MakeGenericMethod(entityType, propertyType);
            var lambda = GetPropertyExpression(entityType, propertyName, propertyType);
            var selectManyInvoke = nonGeneric.Invoke(null, new object[] { query, lambda }) as IQueryable;
            return selectManyInvoke;
        }

        /// <summary>
        /// Gets a list of modified property values for many-on-many relations.
        /// </summary>
        /// <param name="entity">The entity to get the modified many-on-many relations for</param>
        /// <returns>The modified many-on-many relations</returns>
        private List<ModifiedPropertyValue> GetChangedRelations(object entity)
        {
            var list = new List<ModifiedPropertyValue>();
            var entityType = ObjectContext.GetObjectType(entity.GetType());
            var props = this.GetManyToManyRelations(entityType);
            var where = this.GetWhereClause(entityType);
            var keyValues = this.GetKeyValues(entityType, new object[] { entity }).First();

            foreach (var prop in props)
            {
                var propertyType = entity.GetType().GetProperty(prop).PropertyType.GenericTypeArguments[0];
                var oldValuesQuery = this.BuildRelationQuery(entityType, propertyType, prop, where, keyValues);
                var select = this.GetSelectClause(propertyType);
                var oldValues = GetRelationValues(oldValuesQuery, select);
                var newValues = new List<IDictionary<string, object>>();
                var relationProperty = entity.GetPropertyValue(prop) as IEnumerable;

                if (relationProperty != null)
                {
                    newValues = GetRelationValues(relationProperty.AsQueryable(), select);
                }

                var hasChanges = (newValues == null && oldValues.Count > 0) || (newValues.Count != oldValues.Count);

                if (!hasChanges)
                {
                    for (int i = 0; i < newValues.Count; i++)
                    {
                        hasChanges = !oldValues.Any(o =>
                        {
                            var result = true;

                            foreach (var newPropertyValue in newValues[i])
                            {
                                result = o[newPropertyValue.Key].Equals(newPropertyValue.Value);

                                if (!result)
                                {
                                    break;
                                }
                            }

                            return result;
                        });

                        if (hasChanges)
                        {
                            break;
                        }
                    }
                }

                if (hasChanges)
                {
                    list.Add(new ModifiedPropertyValue { PropertyName = prop, OldValue = oldValues, NewValue = newValues });
                }
            }

            return list;
        }

        /// <summary>
        /// Creates a select clause for entity key properties.
        /// </summary>
        /// <param name="entityType">The type of the entity</param>
        /// <returns>The key property select clause</returns>
        private string GetSelectClause(Type entityType)
        {
            StringBuilder select = new StringBuilder();

            foreach (var key in this.GetKeyProperties(entityType))
            {
                if (select.Length != 0)
                {
                    select.Append(", ");
                }

                select.Append(string.Format("{0} as {0}", key));
            }

            return select.ToString();
        }

        /// <summary>
        /// Creates a where clause for an entity type.
        /// </summary>
        /// <param name="entityType">The entity type to create the where clause for</param>
        /// <returns>The where clause</returns>
        private string GetWhereClause(Type entityType)
        {
            StringBuilder where = new StringBuilder();
            var keyProperties = this.GetKeyProperties(entityType);

            for (int i = 0; i < keyProperties.Length; i++)
            {
                if (where.Length != 0)
                {
                    where.Append(" AND ");
                }

                where.Append(string.Format("{0}.Equals(@{1})", keyProperties[i], i));
            }

            return where.ToString();
        }

        #endregion Private Methods
    }
}