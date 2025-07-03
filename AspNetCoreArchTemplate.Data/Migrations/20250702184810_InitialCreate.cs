using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessPlatform.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "First name of the user"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Last name of the user"),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Biological sex of the user: Male, Female, Other"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false, comment: "Phone number of the current person"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of birth"),
                    HeightCm = table.Column<int>(type: "int", nullable: false, comment: "User's height in cm"),
                    WeightKg = table.Column<int>(type: "int", nullable: false, comment: "User's weight in kg"),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                },
                comment: "Extended Identity user with fitness and event tracking data");

            migrationBuilder.CreateTable(
                name: "Gym",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Key of the Gym")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the Gym"),
                    Location = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false, comment: "Location of the Gym"),
                    Description = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false, comment: "Description of the Gym"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gym", x => x.Id);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Id of the user that the daily log belongs to"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date of the daily log")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyLog_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GymImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Key of the Gym Image")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GymId = table.Column<int>(type: "int", nullable: false, comment: "Gym Id that the image belongs to"),
                    ImageUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "URL of the Gym Image"),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GymImage_Gym_GymId",
                        column: x => x.GymId,
                        principalTable: "Gym",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GymWorkingHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GymId = table.Column<int>(type: "int", nullable: false, comment: "The ID of the gym this working hour entry belongs to."),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false, comment: "Day of the week (0 = Sunday, 6 = Saturday)"),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Time the gym opens on the specified day"),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Time the gym closes on the specified day")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymWorkingHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GymWorkingHours_Gym_GymId",
                        column: x => x.GymId,
                        principalTable: "Gym",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents working hours for a gym on a specific day of the week.");

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GymId = table.Column<int>(type: "int", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Specialty of the trainer (e.g. Cardio, Strength, Yoga)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trainers_Gym_GymId",
                        column: x => x.GymId,
                        principalTable: "Gym",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGymSubscription",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "ID of the user who is subscribed to the gym"),
                    GymId = table.Column<int>(type: "int", nullable: false, comment: "ID of the gym the user is subscribed to")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGymSubscription", x => new { x.UserId, x.GymId });
                    table.ForeignKey(
                        name: "FK_UserGymSubscription_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGymSubscription_Gym_GymId",
                        column: x => x.GymId,
                        principalTable: "Gym",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Defines a user's subscription to a specific gym.");

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyLogId = table.Column<int>(type: "int", nullable: false, comment: "Id of the collection food for current day"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Image of the food item"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Calories = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Food_DailyLog_DailyLogId",
                        column: x => x.DailyLogId,
                        principalTable: "DailyLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyLogId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSession_DailyLog_DailyLogId",
                        column: x => x.DailyLogId,
                        principalTable: "DailyLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Title of the event"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GymId = table.Column<int>(type: "int", nullable: false, comment: "ID of the gym where the event will be held"),
                    TrainerId = table.Column<int>(type: "int", nullable: false, comment: "ID of the trainer"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Start date and time of the event"),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "End date and time of the event"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Description of the event"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Gym_GymId",
                        column: x => x.GymId,
                        principalTable: "Gym",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainerClients",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "int", nullable: false, comment: "ID of the trainer"),
                    ClientId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "ID of the client"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date when the trainer-client relationship was established")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerClients", x => new { x.ClientId, x.TrainerId });
                    table.ForeignKey(
                        name: "FK_TrainerClients_AspNetUsers_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerClients_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary key of the workout entry")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutSessionId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key to the workout session this entry belongs to"),
                    Exercise = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the exercise (e.g. Squat, Running, Pull-ups)"),
                    Repetitions = table.Column<int>(type: "int", nullable: true, comment: "Number of repetitions per set (if applicable)"),
                    Sets = table.Column<int>(type: "int", nullable: true, comment: "Number of sets (if applicable)"),
                    DistanceKm = table.Column<double>(type: "float", nullable: true, comment: "Distance in kilometers (for cardio exercises like running, biking)"),
                    WeightKg = table.Column<double>(type: "float", nullable: true, comment: "Weight used in kilograms (for strength training)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutEntries_WorkoutSession_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "WorkoutSession",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Contains a single exercise or activity performed as part of a workout session.");

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false, comment: "ID of the event being registered for"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "ID of the user registering for the event"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time when the registration was made")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => new { x.UserId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventRegistrations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_DailyLog_UserId",
                table: "DailyLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_EventId",
                table: "EventRegistrations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_GymId",
                table: "Events",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_TrainerId",
                table: "Events",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Food_DailyLogId",
                table: "Food",
                column: "DailyLogId");

            migrationBuilder.CreateIndex(
                name: "IX_GymImage_GymId",
                table: "GymImage",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_GymWorkingHours_GymId",
                table: "GymWorkingHours",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerClients_TrainerId",
                table: "TrainerClients",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_GymId",
                table: "Trainers",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_UserId",
                table: "Trainers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGymSubscription_GymId",
                table: "UserGymSubscription",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutEntries_WorkoutSessionId",
                table: "WorkoutEntries",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSession_DailyLogId",
                table: "WorkoutSession",
                column: "DailyLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "GymImage");

            migrationBuilder.DropTable(
                name: "GymWorkingHours");

            migrationBuilder.DropTable(
                name: "TrainerClients");

            migrationBuilder.DropTable(
                name: "UserGymSubscription");

            migrationBuilder.DropTable(
                name: "WorkoutEntries");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "WorkoutSession");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "DailyLog");

            migrationBuilder.DropTable(
                name: "Gym");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
