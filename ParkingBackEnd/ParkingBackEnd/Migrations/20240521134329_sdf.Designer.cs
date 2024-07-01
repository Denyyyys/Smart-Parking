﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ParkingBackEnd.Entities;

#nullable disable

namespace ParkingBackEnd.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240521134329_sdf")]
    partial class sdf
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ParkingBackEnd.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicensePlateNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LicensePlateNumber")
                        .IsUnique();

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.Parking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("NumberFreeParkingSpaces")
                        .HasColumnType("int");

                    b.Property<int>("NumberParkingSpaces")
                        .HasColumnType("int");

                    b.Property<int>("ParkingAdminId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("ParkingAdminId")
                        .IsUnique();

                    b.ToTable("Parkings");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.ParkingAdmin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ParkingAdmins");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.ParkingAndDrivers", b =>
                {
                    b.Property<int>("DriverId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("ParkingId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("DriverId", "ParkingId");

                    b.HasIndex("ParkingId");

                    b.ToTable("ParkingAndDrivers");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.ParkingHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExitDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ParkingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("ParkingId");

                    b.ToTable("ParkingHistory");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.Parking", b =>
                {
                    b.HasOne("ParkingBackEnd.Entities.Address", "Address")
                        .WithOne()
                        .HasForeignKey("ParkingBackEnd.Entities.Parking", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParkingBackEnd.Entities.ParkingAdmin", "ParkingAdmin")
                        .WithOne()
                        .HasForeignKey("ParkingBackEnd.Entities.Parking", "ParkingAdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("ParkingAdmin");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.ParkingAndDrivers", b =>
                {
                    b.HasOne("ParkingBackEnd.Entities.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParkingBackEnd.Entities.Parking", "Parking")
                        .WithMany()
                        .HasForeignKey("ParkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Parking");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.ParkingHistory", b =>
                {
                    b.HasOne("ParkingBackEnd.Entities.Driver", "Driver")
                        .WithMany("ParkingHistory")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ParkingBackEnd.Entities.Parking", "Parking")
                        .WithMany("ParkingHistory")
                        .HasForeignKey("ParkingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Parking");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.Driver", b =>
                {
                    b.Navigation("ParkingHistory");
                });

            modelBuilder.Entity("ParkingBackEnd.Entities.Parking", b =>
                {
                    b.Navigation("ParkingHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
