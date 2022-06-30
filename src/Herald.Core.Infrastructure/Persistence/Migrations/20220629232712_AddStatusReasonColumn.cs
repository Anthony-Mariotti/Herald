using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herald.Core.Infrastructure.Persistence.Migrations
{
    public partial class AddStatusReasonColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusReason",
                table: "QueuedTracks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusReason",
                table: "QueuedTracks");
        }
    }
}
