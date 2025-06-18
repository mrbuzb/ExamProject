using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Domain.Entities;

namespace ToDoList.Infrastructure.Persistence.Configurations;

public class RequestLogConfiguration : IEntityTypeConfiguration<RequestLog>
{
    public void Configure(EntityTypeBuilder<RequestLog> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Path).IsRequired().HasMaxLength(200);
        builder.Property(r => r.Method).IsRequired().HasMaxLength(10);
    }
}
