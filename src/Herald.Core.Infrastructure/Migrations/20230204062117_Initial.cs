using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Herald.Core.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Economy",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChannelRestriction = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Economy", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    OwnerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Joined = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    JoinedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LeftOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    ChanncelRestriction = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RoleRestriction = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RoleMode = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    XpRate = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Released = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Queues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queues", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EconomyChannels",
                columns: table => new
                {
                    EconomyId = table.Column<long>(type: "bigint", nullable: false),
                    ChannelId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EconomyChannels", x => new { x.EconomyId, x.ChannelId });
                    table.ForeignKey(
                        name: "FK_EconomyChannels_Economy_EconomyId",
                        column: x => x.EconomyId,
                        principalTable: "Economy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GuildCatalog",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<double>(type: "double", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildCatalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuildCatalog_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    MemberId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Balance = table.Column<double>(type: "double", nullable: false, defaultValue: 0.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LevelMembers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LevelId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Xp = table.Column<double>(type: "double", nullable: false, defaultValue: 0.0),
                    Level = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelMembers_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LevelRestrictedChannels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LevelId = table.Column<long>(type: "bigint", nullable: false),
                    ChannelId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelRestrictedChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelRestrictedChannels_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LevelRestrictedRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LevelId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelRestrictedRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelRestrictedRoles_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LevelRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LevelId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    AtLevel = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelRole_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ModuleAccess",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    HasAccess = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleAccess", x => new { x.ModuleId, x.GuildId });
                    table.ForeignKey(
                        name: "FK_ModuleAccess_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleAccess_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "QueuedTracks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Identifier = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StatusReason = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Livestream = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Seekable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Provider = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NotifyChannelId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    RequestUserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Source = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Encoded = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QueueId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueuedTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueuedTracks_Queues_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MemberInventories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberInventories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberInventories_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GuildCatalog_GuildId",
                table: "GuildCatalog",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelMembers_LevelId_MemberId",
                table: "LevelMembers",
                columns: new[] { "LevelId", "MemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_LevelRestrictedChannels_LevelId_ChannelId",
                table: "LevelRestrictedChannels",
                columns: new[] { "LevelId", "ChannelId" });

            migrationBuilder.CreateIndex(
                name: "IX_LevelRestrictedRoles_LevelId_RoleId",
                table: "LevelRestrictedRoles",
                columns: new[] { "LevelId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_LevelRole_LevelId",
                table: "LevelRole",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberInventories_MemberId",
                table: "MemberInventories",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_GuildId",
                table: "Members",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleAccess_GuildId",
                table: "ModuleAccess",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_QueuedTracks_QueueId",
                table: "QueuedTracks",
                column: "QueueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EconomyChannels");

            migrationBuilder.DropTable(
                name: "GuildCatalog");

            migrationBuilder.DropTable(
                name: "LevelMembers");

            migrationBuilder.DropTable(
                name: "LevelRestrictedChannels");

            migrationBuilder.DropTable(
                name: "LevelRestrictedRoles");

            migrationBuilder.DropTable(
                name: "LevelRole");

            migrationBuilder.DropTable(
                name: "MemberInventories");

            migrationBuilder.DropTable(
                name: "ModuleAccess");

            migrationBuilder.DropTable(
                name: "QueuedTracks");

            migrationBuilder.DropTable(
                name: "Economy");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Queues");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
