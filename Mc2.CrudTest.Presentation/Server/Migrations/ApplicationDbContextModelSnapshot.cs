﻿// <auto-generated />
using System;
using Mc2.CrudTest.Presentation.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mc2.CrudTest.Presentation.Server.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mc2.CrudTest.Presentation.Shared.Events.EventBase", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("OccurredOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("event_type")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("character varying(21)");

                    b.HasKey("EventId");

                    b.ToTable("Events");

                    b.HasDiscriminator<string>("event_type").HasValue("EventBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Mc2.CrudTest.Presentation.Shared.Events.CustomerCreatedEvent", b =>
                {
                    b.HasBaseType("Mc2.CrudTest.Presentation.Shared.Events.EventBase");

                    b.HasDiscriminator().HasValue("customer_create");
                });

            modelBuilder.Entity("Mc2.CrudTest.Presentation.Shared.Events.CustomerDeletedEvent", b =>
                {
                    b.HasBaseType("Mc2.CrudTest.Presentation.Shared.Events.EventBase");

                    b.HasDiscriminator().HasValue("customer_delete");
                });

            modelBuilder.Entity("Mc2.CrudTest.Presentation.Shared.Events.CustomerUpdatedEvent", b =>
                {
                    b.HasBaseType("Mc2.CrudTest.Presentation.Shared.Events.EventBase");

                    b.HasDiscriminator().HasValue("customer_update");
                });
#pragma warning restore 612, 618
        }
    }
}
