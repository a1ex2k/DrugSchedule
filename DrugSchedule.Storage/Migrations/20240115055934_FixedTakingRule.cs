using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrugSchedule.Storage.Migrations
{
    /// <inheritdoc />
    public partial class FixedTakingRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TakingRules");

            migrationBuilder.AddColumn<string>(
                name: "TakingRule",
                table: "Repeats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TakingRule",
                table: "Repeats");

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
        }
    }
}
