﻿// <auto-generated />
using System;
using BOMA.WRT.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BOMA.WRT.Infrastructure.Database.Migrations
{
    [DbContext(typeof(BomaDbContext))]
    [Migration("20220627064252_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BOMA.WTR.Domain.AggregateModels.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("RcpId")
                        .HasColumnType("int")
                        .HasColumnName("RcpId");

                    b.HasKey("Id");

                    b.ToTable("Employees", (string)null);
                });

            modelBuilder.Entity("BOMA.WTR.Domain.AggregateModels.Entities.WorkingTimeRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<byte>("EventType")
                        .HasColumnType("tinyint")
                        .HasColumnName("EventType");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("GroupId");

                    b.Property<DateTime>("OccuredAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("OccuredAt");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("WorkingTimeRecords", (string)null);
                });

            modelBuilder.Entity("BOMA.WTR.Domain.AggregateModels.Employee", b =>
                {
                    b.OwnsOne("BOMA.WTR.Domain.AggregateModels.ValueObjects.Name", "Name", b1 =>
                        {
                            b1.Property<int>("EmployeeId")
                                .HasColumnType("int");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)")
                                .HasColumnName("FirstName");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(64)
                                .HasColumnType("nvarchar(64)")
                                .HasColumnName("LastName");

                            b1.HasKey("EmployeeId");

                            b1.ToTable("Employees");

                            b1.WithOwner()
                                .HasForeignKey("EmployeeId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("BOMA.WTR.Domain.AggregateModels.Entities.WorkingTimeRecord", b =>
                {
                    b.HasOne("BOMA.WTR.Domain.AggregateModels.Employee", null)
                        .WithMany("WorkingTimeRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BOMA.WTR.Domain.AggregateModels.Employee", b =>
                {
                    b.Navigation("WorkingTimeRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
