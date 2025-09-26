namespace aspnetcore_hexagonal_api.Features.FeatureExample.Application;

public static class FeatureExampleConstants
{
    public const string TableName = "FeatureExamples";

    public static class ValidationMessages
    {
        public const string NameRequired = "Name is required";
        public const string DescriptionRequired = "Description is required";
        public const string InvalidStatus = "Invalid status value";
        public const string EntityNotFound = "Feature example not found";
        public const string CannotDelete = "Cannot delete this feature example";
        public const string CannotChangeStatus = "Cannot change status to this value";
    }

    public static class CacheKeys
    {
        public const string AllFeatureExamples = "all_feature_examples";
        public const string FeatureExampleById = "feature_example_{0}";
        public const string FeatureExamplesByStatus = "feature_examples_status_{0}";
    }

    public static class StatusDescriptions
    {
        public const string Inactive = "Inativo";
        public const string Active = "Ativo";
        public const string Pending = "Pendente";
        public const string Completed = "Concluído";
        public const string Cancelled = "Cancelado";
    }

    public static class DefaultValues
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 100;
    }
}