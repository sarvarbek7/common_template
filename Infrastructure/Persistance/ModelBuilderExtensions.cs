using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public static class ModelBuilderExtensions
{
    public static void AddTranslationConfig(this ModelBuilder modelBuilder, params Type[] ignoredTypes)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITranslatable<,>))))
        {
            var clrType = entityType.ClrType;

            if (ignoredTypes.Contains(clrType))
            {
                continue;
            }

            // Get the generic ITranslatable<T, TTranslation> interface
            var translatableInterface = clrType.GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITranslatable<,>));

            // Get the TTranslation type from the interface
            var translationType = translatableInterface.GetGenericArguments()[1];

            // Get the Translations property
            var translationsProperty = clrType.GetProperty("Translations");

            if (translationsProperty == null)
            {
                continue; // Skip if Translations property is not found
            }

            // Define the foreign key name (e.g., HandbookId for Handbook)
            var entityName = clrType.Name;
            var foreignKeyName = $"{entityName}Id";

            // Configure OwnsMany for the Translations property
            modelBuilder.Entity(clrType).OwnsMany(
                translationType,
                translationsProperty.Name,
                builder =>
                {
                    // Configure the foreign key
                    builder.WithOwner().HasForeignKey(foreignKeyName);

                    // Configure the composite key: (EntityId, Language)
                    builder.HasKey(foreignKeyName, nameof(ITranslation.Language));
                });
        }
    }

    public static void AddIsDeletedQueryFilters(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.IsAssignableTo(typeof(ISoftDeletedEntity))))
        {
            // Create lambda expression: x => !x.IsDeleted
            var parameter = Expression.Parameter(entityType.ClrType, "x");
            var property = Expression.Property(parameter, nameof(ISoftDeletedEntity.IsDeleted));
            var notDeleted = Expression.Not(property);
            var lambda = Expression.Lambda(notDeleted, parameter);

            // Apply query filter
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }

    public static void AddCreatedAuditableForeignKey<TAudit, TAuditId>(this ModelBuilder modelBuilder)
        where TAudit : class, IEntity<TAuditId>
        where TAuditId : struct
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.IsAssignableTo(typeof(ICreationAuditableEntity<TAudit, TAuditId>))))
        {
            modelBuilder.Entity(entityType.ClrType)
                .HasOne(nameof(ICreationAuditableEntity<TAudit, TAuditId>.CreatedBy))
                .WithMany()
                .HasForeignKey(nameof(ICreationAuditableEntity<TAudit, TAuditId>.CreatedById))
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public static void AddUpdatedAuditableForeignKey<TAudit, TAuditId>(this ModelBuilder modelBuilder)
        where TAudit : class, IEntity<TAuditId>
        where TAuditId : struct
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.IsAssignableTo(typeof(IUpdateAuditableEntity<TAudit, TAuditId>))))
        {
            modelBuilder.Entity(entityType.ClrType)
                .HasOne(nameof(IUpdateAuditableEntity<TAudit, TAuditId>.UpdatedBy))
                .WithMany()
                .HasForeignKey(nameof(IUpdateAuditableEntity<TAudit, TAuditId>.UpdatedById))
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public static void AddDeletedAuditableForeignKey<TAudit, TAuditId>(this ModelBuilder modelBuilder)
        where TAudit : class, IEntity<TAuditId>
        where TAuditId : struct
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.IsAssignableTo(typeof(IDeletionAuditableEntity<TAudit, TAuditId>))))
        {
            modelBuilder.Entity(entityType.ClrType)
                .HasOne(nameof(IDeletionAuditableEntity<TAudit, TAuditId>.DeletedBy))
                .WithMany()
                .HasForeignKey(nameof(IDeletionAuditableEntity<TAudit, TAuditId>.DeletedById))
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}