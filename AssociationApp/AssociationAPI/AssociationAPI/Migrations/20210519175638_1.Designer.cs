﻿// <auto-generated />
using System;
using AssociationAPI.Models.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AssociationAPI.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210519175638_1")]
    partial class _1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AssociationAPI.Models.DbModels.Archive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClientId");

                    b.Property<double>("ColdWaterBathroomDue");

                    b.Property<double>("ColdWaterBathroomQuantity");

                    b.Property<double>("ColdWaterKitchenDue");

                    b.Property<double>("ColdWaterKitchenQuantity");

                    b.Property<double>("ElectricityDue");

                    b.Property<double>("ElectricityQuantity");

                    b.Property<double>("GasDue");

                    b.Property<double>("GasQuantity");

                    b.Property<double>("HotWaterBathroomDue");

                    b.Property<double>("HotWaterBathroomQuantity");

                    b.Property<double>("HotWaterKitchenDue");

                    b.Property<double>("HotWaterKitchenQuantity");

                    b.Property<string>("Month");

                    b.Property<double>("TotalPayment");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Archives");
                });

            modelBuilder.Entity("AssociationAPI.Models.DbModels.Association", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("ColdWaterLiterPrice");

                    b.Property<string>("Description");

                    b.Property<double>("Electricity");

                    b.Property<double>("GasPrice");

                    b.Property<double>("HotWaterLiterPrice");

                    b.Property<string>("Location");

                    b.Property<double>("MonthPenalty");

                    b.Property<string>("Program");

                    b.Property<double>("Sanitation");

                    b.Property<double>("WorkingCapital");

                    b.HasKey("Id");

                    b.ToTable("Associations");
                });

            modelBuilder.Entity("AssociationAPI.Models.DbModels.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClientId");

                    b.Property<string>("Month");

                    b.Property<string>("MonthsDelay");

                    b.Property<bool>("PaymentStatus");

                    b.Property<double>("Penalties");

                    b.Property<bool>("SanitationStatus");

                    b.Property<double>("TotalDueWithPenalties");

                    b.Property<string>("UtilitiesPaper");

                    b.Property<bool>("WorkingCapitalStatus");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("AssociationAPI.Models.DbModels.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AmountPayed");

                    b.Property<string>("ClientId");

                    b.Property<DateTime>("PayDate");

                    b.Property<int?>("PaymentId");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("PaymentId");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AssociationAPI.Models.DbModels.User", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Address");

                    b.Property<int?>("AssociationId");

                    b.Property<string>("RepresentativeIdId");

                    b.Property<string>("Telephone");

                    b.HasIndex("AssociationId");

                    b.HasIndex("RepresentativeIdId");

                    b.ToTable("User");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("AssociationAPI.Models.DbModels.Archive", b =>
                {
                    b.HasOne("AssociationAPI.Models.DbModels.User", "Client")
                        .WithMany("ClientArchive")
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("AssociationAPI.Models.DbModels.Payment", b =>
                {
                    b.HasOne("AssociationAPI.Models.DbModels.User", "Client")
                        .WithMany("ClientPayment")
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("AssociationAPI.Models.DbModels.Receipt", b =>
                {
                    b.HasOne("AssociationAPI.Models.DbModels.User", "Client")
                        .WithMany("Receipts")
                        .HasForeignKey("ClientId");

                    b.HasOne("AssociationAPI.Models.DbModels.Payment", "Payment")
                        .WithMany("Receipts")
                        .HasForeignKey("PaymentId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AssociationAPI.Models.DbModels.User", b =>
                {
                    b.HasOne("AssociationAPI.Models.DbModels.Association", "Association")
                        .WithMany("Representative")
                        .HasForeignKey("AssociationId");

                    b.HasOne("AssociationAPI.Models.DbModels.User", "RepresentativeId")
                        .WithMany("Representative")
                        .HasForeignKey("RepresentativeIdId");
                });
#pragma warning restore 612, 618
        }
    }
}
