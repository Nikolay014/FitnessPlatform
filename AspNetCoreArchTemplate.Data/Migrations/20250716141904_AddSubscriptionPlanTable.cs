using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionPlanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SubscribedOn",
                table: "UserGymSubscription",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanId",
                table: "UserGymSubscription",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidUntil",
                table: "UserGymSubscription",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGymSubscription_SubscriptionPlanId",
                table: "UserGymSubscription",
                column: "SubscriptionPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGymSubscription_SubscriptionPlans_SubscriptionPlanId",
                table: "UserGymSubscription",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGymSubscription_SubscriptionPlans_SubscriptionPlanId",
                table: "UserGymSubscription");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropIndex(
                name: "IX_UserGymSubscription_SubscriptionPlanId",
                table: "UserGymSubscription");

            migrationBuilder.DropColumn(
                name: "SubscribedOn",
                table: "UserGymSubscription");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "UserGymSubscription");

            migrationBuilder.DropColumn(
                name: "ValidUntil",
                table: "UserGymSubscription");
        }
    }
}
