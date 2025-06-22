namespace Domain.Common.Utils;

public static class CollectionExtentios {
    public static bool NotNullOrEmpty<T>(this IEnumerable<T> collection) {
        return collection != null && collection.Any();
    }
}
