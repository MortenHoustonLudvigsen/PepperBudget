namespace PepperBudget.Data;

public class SnakeCaseConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        Process(modelBuilder.Metadata.GetEntityTypes(), entityType => entityType.GetTableName(), action: entityType =>
        {
            Process(entityType.GetProperties(), property => property.Name);
            Process(entityType.GetKeys(), key => key.GetName(), ignore: 3);
            Process(entityType.GetForeignKeys(), fk => fk.GetConstraintName(), ignore: 3);
            Process(entityType.GetIndexes(), index => index.GetDatabaseName(), ignore: 3);
        });
    }

    private static void Process<TConvention>(IEnumerable<TConvention> conventions, Func<TConvention, string?> getName, int ignore = 1, Action<TConvention>? action = null)
        where TConvention : IConventionAnnotatable
    {
        foreach (var convention in conventions)
        {
            Process(convention, getName(convention), ignore, action);
        }
    }

    private static void Process<TConvention>(TConvention convention, string? name, int ignore = 1, Action<TConvention>? action = null)
        where TConvention : IConventionAnnotatable
    {
        if (string.IsNullOrWhiteSpace(name))
            return;

        SetName(convention, ToSnakeCase(name, ignore));
        action?.Invoke(convention);
    }

    private static void SetName<TConvention>(TConvention convention, string name)
        where TConvention : IConventionAnnotatable
    {
        switch (convention)
        {
            case IConventionEntityType entityType:
                entityType.Builder.HasAnnotation(RelationalAnnotationNames.TableName, name);
                break;
            case IConventionProperty property:
                property.Builder.HasAnnotation(RelationalAnnotationNames.ColumnName, name);
                break;
            case IConventionKey key:
                key.Builder.HasAnnotation(RelationalAnnotationNames.Name, name);
                break;
            case IConventionForeignKey foreignKey:
                foreignKey.Builder.HasAnnotation(RelationalAnnotationNames.Name, name);
                break;
            case IConventionIndex index:
                index.Builder.HasAnnotation(RelationalAnnotationNames.Name, name);
                break;
            default:
                throw new ArgumentException($"Unknown convention type: {convention.GetType().FullName}", nameof(convention));
        }
    }

    private static string ToSnakeCase(string str, int ignore = 1)
    {
        ArgumentNullException.ThrowIfNull(str);

        if (str.Length < 2)
            return str.ToLowerInvariant();

        var text = new StringBuilder();

        text.Append(str[..ignore].ToLowerInvariant());

        foreach (var c in str[ignore..])
        {
            if (char.IsUpper(c))
            {
                text.Append('_');
                text.Append(char.ToLowerInvariant(c));
            }
            else
            {
                text.Append(c);
            }
        }

        return text.ToString();
    }
}
