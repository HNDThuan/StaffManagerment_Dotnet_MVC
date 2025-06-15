using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_QRCodes_QRCodeId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "QRCodes");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_EmployeeId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_QRCodeId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AttendanceTime",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsCheckedIn",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "IsCheckedOut",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "LateCheckInTime",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "LatePenalty",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "ScheduledCheckInTime",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "QRCodeId",
                table: "Attendances",
                newName: "ShiftId");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckInTime",
                table: "Attendances",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "CheckOutTime",
                table: "Attendances",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Attendances",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "MinutesLate",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Penalty",
                table: "Attendances",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    ShiftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    GraceMinutes = table.Column<int>(type: "int", nullable: false),
                    CutoffTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.ShiftId);
                });

            migrationBuilder.CreateTable(
                name: "AttendanceTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceTokens_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShifts",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShifts", x => new { x.ShiftId, x.EmployeeId, x.StartDate });
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "ShiftId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId_ShiftId_Date",
                table: "Attendances",
                columns: new[] { "EmployeeId", "ShiftId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_ShiftId",
                table: "Attendances",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId",
                unique: true,
                filter: "[EmployeeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeId1",
                table: "AspNetUsers",
                column: "EmployeeId1",
                unique: true,
                filter: "[EmployeeId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceTokens_ExpiresAt",
                table: "AttendanceTokens",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceTokens_ShiftId",
                table: "AttendanceTokens",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_EmployeeId",
                table: "EmployeeShifts",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId1",
                table: "AspNetUsers",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Shifts_ShiftId",
                table: "Attendances",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId1",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Shifts_ShiftId",
                table: "Attendances");

            migrationBuilder.DropTable(
                name: "AttendanceTokens");

            migrationBuilder.DropTable(
                name: "EmployeeShifts");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_EmployeeId_ShiftId_Date",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_ShiftId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "CheckOutTime",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "MinutesLate",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Penalty",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ShiftId",
                table: "Attendances",
                newName: "QRCodeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AttendanceTime",
                table: "Attendances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedIn",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckedOut",
                table: "Attendances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LateCheckInTime",
                table: "Attendances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LatePenalty",
                table: "Attendances",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledCheckInTime",
                table: "Attendances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "QRCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    QRCodeValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QRCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QRCodes_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EmployeeId",
                table: "Attendances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_QRCodeId",
                table: "Attendances",
                column: "QRCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodes_EmployeeId",
                table: "QRCodes",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeId",
                table: "AspNetUsers",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_QRCodes_QRCodeId",
                table: "Attendances",
                column: "QRCodeId",
                principalTable: "QRCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
