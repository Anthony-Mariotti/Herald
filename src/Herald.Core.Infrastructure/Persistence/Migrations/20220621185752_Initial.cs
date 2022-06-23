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
                    Identifier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Livestream = table.Column<bool>(type: "bit", nullable: false),
                    Seekable = table.Column<bool>(type: "bit", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    NotifyChannelId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    RequestUserId = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Encoded = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
