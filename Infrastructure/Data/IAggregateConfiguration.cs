using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
internal interface IAggregateConfiguration {
    ModelBuilder Apply(ModelBuilder modelBuilder);
}
