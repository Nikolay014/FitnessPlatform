using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Trainers");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "Trainers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Specialty of the trainer (e.g. Cardio, Strength, Yoga)");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the specialty")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the specialty"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Description of the specialty")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_SpecialtyId",
                table: "Trainers",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SpecialtyId",
                table: "AspNetUsers",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Specialties_SpecialtyId",
                table: "AspNetUsers",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainers_Specialties_SpecialtyId",
                table: "Trainers",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Specialties_SpecialtyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainers_Specialties_SpecialtyId",
                table: "Trainers");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropIndex(
                name: "IX_Trainers_SpecialtyId",
                table: "Trainers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SpecialtyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Trainers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Specialty of the trainer (e.g. Cardio, Strength, Yoga)");
        }
    }
}
