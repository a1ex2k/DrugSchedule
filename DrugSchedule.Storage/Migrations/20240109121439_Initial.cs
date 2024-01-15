using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugSchedule.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TakingRules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakingRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdentityUserGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Medicaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Composition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseFormId = table.Column<int>(type: "int", nullable: false),
                    ManufacturerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicaments_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Medicaments_ReleaseForms_ReleaseFormId",
                        column: x => x.ReleaseFormId,
                        principalTable: "ReleaseForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileInfos",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileCategory = table.Column<int>(type: "int", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TakingСonfirmationId = table.Column<long>(type: "bigint", nullable: true),
                    UserMedicamentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInfos", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicamentId = table.Column<int>(type: "int", nullable: false),
                    FileGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicamentFiles_FileInfos_FileGuid",
                        column: x => x.FileGuid,
                        principalTable: "FileInfos",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicamentFiles_Medicaments_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityGuid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RealName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    AvatarGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_IdentityGuid",
                        column: x => x.IdentityGuid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_FileInfos_AvatarGuid",
                        column: x => x.AvatarGuid,
                        principalTable: "FileInfos",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "UserMedicaments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasedOnMedicamentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Composition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseForm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManufacturerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMedicaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMedicaments_Medicaments_BasedOnMedicamentId",
                        column: x => x.BasedOnMedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserMedicaments_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfileContacts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false),
                    ContactProfileId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileContacts_UserProfiles_ContactProfileId",
                        column: x => x.ContactProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfileContacts_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicamentTakingSchedule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false),
                    UserMedicamentId = table.Column<long>(type: "bigint", nullable: true),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentTakingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicamentTakingSchedule_UserMedicaments_UserMedicamentId",
                        column: x => x.UserMedicamentId,
                        principalTable: "UserMedicaments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicamentTakingSchedule_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserMedicamentFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserMedicamentId = table.Column<long>(type: "bigint", nullable: false),
                    FileGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMedicamentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMedicamentFiles_FileInfos_FileGuid",
                        column: x => x.FileGuid,
                        principalTable: "FileInfos",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMedicamentFiles_UserMedicaments_UserMedicamentId",
                        column: x => x.UserMedicamentId,
                        principalTable: "UserMedicaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Repeats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<long>(type: "bigint", nullable: false),
                    BeginDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    TimeOfDay = table.Column<int>(type: "int", nullable: false),
                    RepeatDayOfWeek = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MedicamentTakingScheduleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repeats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repeats_MedicamentTakingSchedule_MedicamentTakingScheduleId",
                        column: x => x.MedicamentTakingScheduleId,
                        principalTable: "MedicamentTakingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Repeats_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleShare",
                columns: table => new
                {
                    MedicamentTakingScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    ShareWithContactId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleShare", x => new { x.MedicamentTakingScheduleId, x.ShareWithContactId });
                    table.ForeignKey(
                        name: "FK_ScheduleShare_MedicamentTakingSchedule_MedicamentTakingScheduleId",
                        column: x => x.MedicamentTakingScheduleId,
                        principalTable: "MedicamentTakingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduleShare_UserProfileContacts_ShareWithContactId",
                        column: x => x.ShareWithContactId,
                        principalTable: "UserProfileContacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TakingСonfirmations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleRepeatId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakingСonfirmations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TakingСonfirmations_Repeats_ScheduleRepeatId",
                        column: x => x.ScheduleRepeatId,
                        principalTable: "Repeats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TakingСonfirmationFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TakingСonfirmationId = table.Column<long>(type: "bigint", nullable: false),
                    FileGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakingСonfirmationFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TakingСonfirmationFiles_FileInfos_FileGuid",
                        column: x => x.FileGuid,
                        principalTable: "FileInfos",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TakingСonfirmationFiles_TakingСonfirmations_TakingСonfirmationId",
                        column: x => x.TakingСonfirmationId,
                        principalTable: "TakingСonfirmations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FileInfos_TakingСonfirmationId",
                table: "FileInfos",
                column: "TakingСonfirmationId");

            migrationBuilder.CreateIndex(
                name: "IX_FileInfos_UserMedicamentId",
                table: "FileInfos",
                column: "UserMedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentFiles_FileGuid",
                table: "MedicamentFiles",
                column: "FileGuid");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentFiles_MedicamentId",
                table: "MedicamentFiles",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_ManufacturerId",
                table: "Medicaments",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicaments_ReleaseFormId",
                table: "Medicaments",
                column: "ReleaseFormId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentTakingSchedule_UserMedicamentId",
                table: "MedicamentTakingSchedule",
                column: "UserMedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentTakingSchedule_UserProfileId",
                table: "MedicamentTakingSchedule",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_IdentityUserId",
                table: "RefreshTokens",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Repeats_MedicamentTakingScheduleId",
                table: "Repeats",
                column: "MedicamentTakingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Repeats_UserProfileId",
                table: "Repeats",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleShare_ShareWithContactId",
                table: "ScheduleShare",
                column: "ShareWithContactId");

            migrationBuilder.CreateIndex(
                name: "IX_TakingСonfirmationFiles_FileGuid",
                table: "TakingСonfirmationFiles",
                column: "FileGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TakingСonfirmationFiles_TakingСonfirmationId",
                table: "TakingСonfirmationFiles",
                column: "TakingСonfirmationId");

            migrationBuilder.CreateIndex(
                name: "IX_TakingСonfirmations_ScheduleRepeatId",
                table: "TakingСonfirmations",
                column: "ScheduleRepeatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicamentFiles_FileGuid",
                table: "UserMedicamentFiles",
                column: "FileGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicamentFiles_UserMedicamentId",
                table: "UserMedicamentFiles",
                column: "UserMedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicaments_BasedOnMedicamentId",
                table: "UserMedicaments",
                column: "BasedOnMedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedicaments_UserProfileId",
                table: "UserMedicaments",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileContacts_ContactProfileId",
                table: "UserProfileContacts",
                column: "ContactProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileContacts_UserProfileId",
                table: "UserProfileContacts",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AvatarGuid",
                table: "UserProfiles",
                column: "AvatarGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_IdentityGuid",
                table: "UserProfiles",
                column: "IdentityGuid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FileInfos_TakingСonfirmations_TakingСonfirmationId",
                table: "FileInfos",
                column: "TakingСonfirmationId",
                principalTable: "TakingСonfirmations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileInfos_UserMedicaments_UserMedicamentId",
                table: "FileInfos",
                column: "UserMedicamentId",
                principalTable: "UserMedicaments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_IdentityGuid",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_FileInfos_TakingСonfirmations_TakingСonfirmationId",
                table: "FileInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_FileInfos_UserMedicaments_UserMedicamentId",
                table: "FileInfos");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "MedicamentFiles");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "ScheduleShare");

            migrationBuilder.DropTable(
                name: "TakingRules");

            migrationBuilder.DropTable(
                name: "TakingСonfirmationFiles");

            migrationBuilder.DropTable(
                name: "UserMedicamentFiles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "UserProfileContacts");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TakingСonfirmations");

            migrationBuilder.DropTable(
                name: "Repeats");

            migrationBuilder.DropTable(
                name: "MedicamentTakingSchedule");

            migrationBuilder.DropTable(
                name: "UserMedicaments");

            migrationBuilder.DropTable(
                name: "Medicaments");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "ReleaseForms");

            migrationBuilder.DropTable(
                name: "FileInfos");
        }
    }
}
