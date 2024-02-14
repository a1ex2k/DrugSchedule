﻿// <auto-generated />
using System;
using DrugSchedule.Storage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DrugSchedule.Storage.Migrations
{
    [DbContext(typeof(DrugScheduleContext))]
    [Migration("20240212160109_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.FileInfo", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FileCategory")
                        .HasColumnType("int");

                    b.Property<bool>("HasThumbnail")
                        .HasColumnType("bit");

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Guid");

                    b.ToTable("FileInfos");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.Medicament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Composition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReleaseFormId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("ReleaseFormId");

                    b.ToTable("Medicaments");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.MedicamentFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<Guid>("FileGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MedicamentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FileGuid");

                    b.HasIndex("MedicamentId");

                    b.ToTable("MedicamentFiles");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.MedicamentReleaseForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ReleaseForms");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.MedicamentTakingSchedule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<int?>("GlobalMedicamentId")
                        .HasColumnType("int");

                    b.Property<string>("Information")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserMedicamentId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserProfileId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GlobalMedicamentId");

                    b.HasIndex("UserMedicamentId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("MedicamentTakingSchedules");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.RefreshTokenEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("ClientInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityUserGuid")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.ScheduleRepeat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateOnly>("BeginDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<long>("MedicamentTakingScheduleId")
                        .HasColumnType("bigint");

                    b.Property<int>("RepeatDayOfWeek")
                        .HasColumnType("int");

                    b.Property<string>("TakingRule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time");

                    b.Property<int>("TimeOfDay")
                        .HasColumnType("int");

                    b.Property<long?>("UserProfileId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentTakingScheduleId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("ScheduleRepeat");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.ScheduleShare", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("MedicamentTakingScheduleId")
                        .HasColumnType("bigint");

                    b.Property<long>("ShareWithContactId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentTakingScheduleId");

                    b.HasIndex("ShareWithContactId");

                    b.ToTable("ScheduleShare");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.TakingСonfirmation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("ScheduleRepeatId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleRepeatId");

                    b.ToTable("TakingСonfirmations");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.TakingСonfirmationFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<Guid>("FileGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("TakingСonfirmationId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FileGuid");

                    b.HasIndex("TakingСonfirmationId");

                    b.ToTable("TakingСonfirmationFiles");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserMedicament", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int?>("BasedOnMedicamentId")
                        .HasColumnType("int");

                    b.Property<string>("Composition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManufacturerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReleaseForm")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserProfileId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BasedOnMedicamentId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserMedicaments");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserMedicamentFile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<Guid>("FileGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("UserMedicamentId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("FileGuid");

                    b.HasIndex("UserMedicamentId");

                    b.ToTable("UserMedicamentFiles");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserProfile", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<Guid?>("AvatarGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("IdentityGuid")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RealName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AvatarGuid");

                    b.HasIndex("IdentityGuid")
                        .IsUnique();

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserProfileContact", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ContactProfileId")
                        .HasColumnType("bigint");

                    b.Property<string>("CustomName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserProfileId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ContactProfileId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserProfileContacts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.Medicament", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId");

                    b.HasOne("DrugSchedule.Storage.Data.Entities.MedicamentReleaseForm", "ReleaseForm")
                        .WithMany()
                        .HasForeignKey("ReleaseFormId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Manufacturer");

                    b.Navigation("ReleaseForm");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.MedicamentFile", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.FileInfo", "FileInfo")
                        .WithMany()
                        .HasForeignKey("FileGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Storage.Data.Entities.Medicament", "Medicament")
                        .WithMany("Files")
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileInfo");

                    b.Navigation("Medicament");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.MedicamentTakingSchedule", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.Medicament", "GlobalMedicament")
                        .WithMany()
                        .HasForeignKey("GlobalMedicamentId");

                    b.HasOne("DrugSchedule.Storage.Data.Entities.UserMedicament", "UserMedicament")
                        .WithMany()
                        .HasForeignKey("UserMedicamentId");

                    b.HasOne("DrugSchedule.Storage.Data.Entities.UserProfile", "UserProfile")
                        .WithMany("MedicamentTakingSchedules")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("GlobalMedicament");

                    b.Navigation("UserMedicament");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.RefreshTokenEntry", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "IdentityUser")
                        .WithMany()
                        .HasForeignKey("IdentityUserId");

                    b.Navigation("IdentityUser");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.ScheduleRepeat", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.MedicamentTakingSchedule", "MedicamentTakingSchedule")
                        .WithMany("RepeatSchedules")
                        .HasForeignKey("MedicamentTakingScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Storage.Data.Entities.UserProfile", null)
                        .WithMany("ScheduleRepeats")
                        .HasForeignKey("UserProfileId");

                    b.Navigation("MedicamentTakingSchedule");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.ScheduleShare", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.MedicamentTakingSchedule", "MedicamentTakingSchedule")
                        .WithMany("ScheduleShares")
                        .HasForeignKey("MedicamentTakingScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Storage.Data.Entities.UserProfileContact", "ShareWithContact")
                        .WithMany("ScheduleShares")
                        .HasForeignKey("ShareWithContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicamentTakingSchedule");

                    b.Navigation("ShareWithContact");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.TakingСonfirmation", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.ScheduleRepeat", "ScheduleRepeat")
                        .WithMany("TakingСonfirmations")
                        .HasForeignKey("ScheduleRepeatId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ScheduleRepeat");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.TakingСonfirmationFile", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.FileInfo", "FileInfo")
                        .WithMany()
                        .HasForeignKey("FileGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Storage.Data.Entities.TakingСonfirmation", "TakingСonfirmation")
                        .WithMany("Files")
                        .HasForeignKey("TakingСonfirmationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileInfo");

                    b.Navigation("TakingСonfirmation");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserMedicament", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.Medicament", "BasedOnMedicament")
                        .WithMany()
                        .HasForeignKey("BasedOnMedicamentId");

                    b.HasOne("DrugSchedule.Storage.Data.Entities.UserProfile", "UserProfile")
                        .WithMany("UserMedicaments")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BasedOnMedicament");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserMedicamentFile", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.FileInfo", "FileInfo")
                        .WithMany()
                        .HasForeignKey("FileGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Storage.Data.Entities.UserMedicament", "UserMedicament")
                        .WithMany("Files")
                        .HasForeignKey("UserMedicamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FileInfo");

                    b.Navigation("UserMedicament");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserProfile", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.FileInfo", "AvatarInfo")
                        .WithMany()
                        .HasForeignKey("AvatarGuid");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithOne()
                        .HasForeignKey("DrugSchedule.Storage.Data.Entities.UserProfile", "IdentityGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AvatarInfo");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserProfileContact", b =>
                {
                    b.HasOne("DrugSchedule.Storage.Data.Entities.UserProfile", "ContactProfile")
                        .WithMany()
                        .HasForeignKey("ContactProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Storage.Data.Entities.UserProfile", "UserProfile")
                        .WithMany("Contacts")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ContactProfile");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.Medicament", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.MedicamentTakingSchedule", b =>
                {
                    b.Navigation("RepeatSchedules");

                    b.Navigation("ScheduleShares");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.ScheduleRepeat", b =>
                {
                    b.Navigation("TakingСonfirmations");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.TakingСonfirmation", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserMedicament", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserProfile", b =>
                {
                    b.Navigation("Contacts");

                    b.Navigation("MedicamentTakingSchedules");

                    b.Navigation("ScheduleRepeats");

                    b.Navigation("UserMedicaments");
                });

            modelBuilder.Entity("DrugSchedule.Storage.Data.Entities.UserProfileContact", b =>
                {
                    b.Navigation("ScheduleShares");
                });
#pragma warning restore 612, 618
        }
    }
}