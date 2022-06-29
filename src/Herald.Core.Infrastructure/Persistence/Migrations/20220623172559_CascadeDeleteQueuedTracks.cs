using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herald.Core.Infrastructure.Persistence.Migrations
{
    public partial class CascadeDeleteQueuedTracks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueuedTracks_Queues_QueueId",
                table: "QueuedTracks");

            migrationBuilder.AddForeignKey(
                name: "FK_QueuedTracks_Queues_QueueId",
                table: "QueuedTracks",
                column: "QueueId",
                principalTable: "Queues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QueuedTracks_Queues_QueueId",
                table: "QueuedTracks");

            migrationBuilder.AddForeignKey(
                name: "FK_QueuedTracks_Queues_QueueId",
                table: "QueuedTracks",
                column: "QueueId",
                principalTable: "Queues",
                principalColumn: "Id");
        }
    }
}
