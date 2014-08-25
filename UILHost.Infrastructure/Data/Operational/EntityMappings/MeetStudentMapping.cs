using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using UILHost.Infrastructure.Domain;

namespace UILHost.Infrastructure.Data.Operational.EntityMappings
{
    class MeetStudentMapping :EntityTypeConfiguration<MeetStudent>
    {
        public MeetStudentMapping()
        {
            // PRIMARY KEY

            HasKey(p => p.Id);

            // PROPERTIES

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(m => m.MeetSchool)
                .WithMany(m => m.MeetStudents)
                .WillCascadeOnDelete(true);
            HasRequired(m => m.Student)
                .WithMany()
                .WillCascadeOnDelete(false);
        }
    }
}
