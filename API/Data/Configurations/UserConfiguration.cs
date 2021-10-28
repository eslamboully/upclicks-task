using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasMany(u => u.DailyAuthentications).WithOne(c => c.User)
                .HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}