using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data;

internal static class TimeConverter {
    class UtcDateTimeConverter : ValueConverter<DateTime, DateTime> {
        // Convert to UTC when saving
        public UtcDateTimeConverter()
            : base(v => ConvertToUtc(v), v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) { }

        private static DateTime ConvertToUtc(DateTime value) {
            if (value.Kind != DateTimeKind.Utc) {
                // Simple logging example
                Console.WriteLine(
                    $"[UtcDateTimeConverter] Converting local DateTime to UTC: {value:o} (Kind={value.Kind})"
                );

                // You could log a stack trace to trace back the source
                var trace = Environment.StackTrace;
                Console.WriteLine(trace);

                return value.ToUniversalTime();
            }

            return value;
        }
    }

    public static void AllEntitiesToUtcTimes(this ModelBuilder modelBuilder) {
        var dateTimeConverter = new UtcDateTimeConverter();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
            if (entityType.IsOwned()) {
                continue;
            }

            var properties = entityType
                .ClrType.GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime));

            foreach (var property in properties) {
                modelBuilder
                    .Entity(entityType.Name)
                    .Property(property.Name)
                    .HasConversion(dateTimeConverter);
            }
        }
    }
}
