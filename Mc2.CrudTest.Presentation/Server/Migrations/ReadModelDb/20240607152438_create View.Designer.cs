﻿// <auto-generated />
using System;
using Mc2.CrudTest.Presentation.Server.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mc2.CrudTest.Presentation.Server.Migrations.ReadModelDb
{
    [DbContext(typeof(ReadModelDbContext))]
    [Migration("20240607152438_create View")]
    partial class createView
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mc2.CrudTest.Presentation.Shared.ReadModels.CustomerReadModel", b =>
                {
                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uuid")
                        .HasColumnName("AggregateId");

                    b.Property<string>("BankAccount")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("bankaccount");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("event_type");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<DateTimeOffset>("OccurredOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("OccurredOn");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phonenumber");

                    b.Property<string>("_dateOfBirth")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("dateofbirth");

                    b.Property<string>("_isDeleted")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("isdeleted");

                    b.ToTable("events_view", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
