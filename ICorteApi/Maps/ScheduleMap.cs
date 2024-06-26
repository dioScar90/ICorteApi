using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

public class ScheduleMap : BaseMap<Schedule>
{
    public override void Configure(EntityTypeBuilder<Schedule> builder)
    {
        base.Configure(builder);

        // builder.HasOne(a => a.Barber)
        //     .WithMany()
        //     .HasForeignKey(a => a.BarberId);

        // builder.Property(x => x.Street).HasColumnType("varchar(100)").IsRequired();
        // builder.Property(x => x.Ativo).HasColumnName("ativo");

        // builder.HasMany(x => x.Especialidades)
        //     .WithMany(x => x.Profissionais)
        //     .UsingEntity<ProfissionalEspecialidade>(
        //         x => x.HasOne(p => p.Especialidade).WithMany().HasForeignKey(x => x.EspecialidadeId),
        //         x => x.HasOne(p => p.Profissionais).WithMany().HasForeignKey(x => x.ProfissionalId),
        //         x =>
        //         {
        //             x.ToTable("tb_profissional_especialidade");

        //             x.HasKey(p => new { p.EspecialidadeId, p.ProfissionalId });

        //             x.Property(p => p.EspecialidadeId).HasColumnName("id_especialidade").IsRequired();
        //             x.Property(p => p.ProfissionalId).HasColumnName("id_profissional").IsRequired();
        //         }
        //     ); ;
    }
}
