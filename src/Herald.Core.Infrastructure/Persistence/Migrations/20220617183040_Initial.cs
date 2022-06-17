using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herald.Core.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "QueuedTracksSeq",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "QueuesSeq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    OwnerId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Joined = table.Column<bool>(type: "bit", nullable: false),
                    JoinedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeftOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "Queues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuildModules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuildId = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildModules_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueuedTracks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TrackId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Author = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TrackString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotifyChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    RequestUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Playing = table.Column<bool>(type: "bit", nullable: false),
                    Paused = table.Column<bool>(type: "bit", nullable: false),
                    Played = table.Column<bool>(type: "bit", nullable: false),
                    QueueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueuedTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueuedTracks_Queues_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuildModules_GuildId",
                table: "GuildModules",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_QueuedTracks_QueueId",
                table: "QueuedTracks",
                column: "QueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuildModules");

            migrationBuilder.DropTable(
                name: "QueuedTracks");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Queues");

            migrationBuilder.DropSequence(
                name: "QueuedTracksSeq");

            migrationBuilder.DropSequence(
                name: "QueuesSeq");
        }
    }
}
