﻿// <auto-generated />
using System;
using DrugSchedule.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DrugSchedule.Api.Migrations
{
    [DbContext(typeof(DrugScheduleContext))]
    [Migration("20231130103659_InitialCreation")]
    partial class InitialCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DrugSchedule.Api.Data.Entities.Schedule.UserMedicament", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int?>("BasedOnMedicamentId")
                        .HasColumnType("int");

                    b.Property<string>("Dosage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ManufacturerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PackQuantity")
                        .HasColumnType("int");

                    b.Property<string>("ReleaseForm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BasedOnMedicamentId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserMedicaments");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.Manufacturer", b =>
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

            modelBuilder.Entity("DrugSchedule.Api.Data.Medicament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Dosage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PackQuantity")
                        .HasColumnType("int");

                    b.Property<int>("ReleaseFormId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("ReleaseFormId");

                    b.ToTable("Medicaments");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.MedicamentReleaseForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.MedicamentTakingSchedule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Information")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserMedicamentId")
                        .HasColumnType("bigint");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserMedicamentId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("MedicamentTakingSchedule");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.Repeat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateOnly>("BeginDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<long>("MedicamentTakingScheduleId")
                        .HasColumnType("bigint");

                    b.Property<int>("RepeatDayOfWeek")
                        .HasColumnType("int");

                    b.Property<int>("TakingScheduleId")
                        .HasColumnType("int");

                    b.Property<TimeOnly>("Time")
                        .HasColumnType("time");

                    b.Property<int>("TimeOfDay")
                        .HasColumnType("int");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentTakingScheduleId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("Repeats");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.ScheduleRepeat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("MedicamentTakingScheduleId")
                        .HasColumnType("bigint");

                    b.Property<long>("RepeatId")
                        .HasColumnType("bigint");

                    b.Property<long>("TakingRuleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentTakingScheduleId");

                    b.HasIndex("RepeatId");

                    b.HasIndex("TakingRuleId");

                    b.ToTable("ScheduleRepeat");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.ScheduleShare", b =>
                {
                    b.Property<long>("MedicamentTakingScheduleId")
                        .HasColumnType("bigint");

                    b.Property<long>("SharedWithId")
                        .HasColumnType("bigint");

                    b.Property<int>("ShareWithProfileId")
                        .HasColumnType("int");

                    b.HasKey("MedicamentTakingScheduleId", "SharedWithId");

                    b.HasIndex("ShareWithProfileId");

                    b.HasIndex("SharedWithId");

                    b.ToTable("ScheduleShare");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.TakingRule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TakingRules");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.TakingСonfirmation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ImageGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long?>("ScheduleRepeatId")
                        .HasColumnType("bigint");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleRepeatId");

                    b.ToTable("TakingСonfirmations");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.UserProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly?>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("RealName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RoadMapStatuses");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.UserProfileContact", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("ContactProfileId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContactProfileId");

                    b.HasIndex("UserProfileId");

                    b.ToTable("UserProfileContacts");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.Entities.Schedule.UserMedicament", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.Medicament", "BasedOnMedicament")
                        .WithMany()
                        .HasForeignKey("BasedOnMedicamentId");

                    b.HasOne("DrugSchedule.Api.Data.UserProfile", null)
                        .WithMany("UserMedicaments")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BasedOnMedicament");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.Medicament", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Api.Data.MedicamentReleaseForm", "ReleaseForm")
                        .WithMany()
                        .HasForeignKey("ReleaseFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacturer");

                    b.Navigation("ReleaseForm");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.MedicamentTakingSchedule", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.Entities.Schedule.UserMedicament", "UserMedicament")
                        .WithMany()
                        .HasForeignKey("UserMedicamentId");

                    b.HasOne("DrugSchedule.Api.Data.UserProfile", "UserProfile")
                        .WithMany("MedicamentTakingSchedules")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserMedicament");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.Repeat", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.MedicamentTakingSchedule", "MedicamentTakingSchedule")
                        .WithMany()
                        .HasForeignKey("MedicamentTakingScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Api.Data.UserProfile", "UserProfile")
                        .WithMany("TakingRepeats")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicamentTakingSchedule");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.ScheduleRepeat", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.MedicamentTakingSchedule", "MedicamentTakingSchedule")
                        .WithMany("RepeatSchedules")
                        .HasForeignKey("MedicamentTakingScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Api.Data.Repeat", "Repeat")
                        .WithMany()
                        .HasForeignKey("RepeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Api.Data.TakingRule", "TakingRule")
                        .WithMany()
                        .HasForeignKey("TakingRuleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicamentTakingSchedule");

                    b.Navigation("Repeat");

                    b.Navigation("TakingRule");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.ScheduleShare", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.MedicamentTakingSchedule", "MedicamentTakingSchedule")
                        .WithMany()
                        .HasForeignKey("MedicamentTakingScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Api.Data.UserProfile", "ShareWithProfile")
                        .WithMany()
                        .HasForeignKey("ShareWithProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Api.Data.UserProfileContact", null)
                        .WithMany()
                        .HasForeignKey("SharedWithId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MedicamentTakingSchedule");

                    b.Navigation("ShareWithProfile");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.TakingСonfirmation", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.ScheduleRepeat", "ScheduleRepeat")
                        .WithMany()
                        .HasForeignKey("ScheduleRepeatId");

                    b.Navigation("ScheduleRepeat");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.UserProfile", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.UserProfileContact", b =>
                {
                    b.HasOne("DrugSchedule.Api.Data.UserProfile", "ContactProfile")
                        .WithMany()
                        .HasForeignKey("ContactProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DrugSchedule.Api.Data.UserProfile", "UserProfile")
                        .WithMany("Contacts")
                        .HasForeignKey("UserProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactProfile");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.MedicamentTakingSchedule", b =>
                {
                    b.Navigation("RepeatSchedules");
                });

            modelBuilder.Entity("DrugSchedule.Api.Data.UserProfile", b =>
                {
                    b.Navigation("Contacts");

                    b.Navigation("MedicamentTakingSchedules");

                    b.Navigation("TakingRepeats");

                    b.Navigation("UserMedicaments");
                });
#pragma warning restore 612, 618
        }
    }
}
