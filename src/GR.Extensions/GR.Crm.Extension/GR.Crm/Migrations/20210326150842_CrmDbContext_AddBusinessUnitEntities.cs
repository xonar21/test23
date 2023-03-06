using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GR.Crm.Migrations
{
    public partial class CrmDbContext_AddBusinessUnitEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessJobPositions",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    HourlySalary = table.Column<int>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    RowOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessJobPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gradings",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BusinessJobPositionId = table.Column<Guid>(nullable: true),
                    GradingId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_BusinessJobPositions_BusinessJobPositionId",
                        column: x => x.BusinessJobPositionId,
                        principalSchema: "Crm",
                        principalTable: "BusinessJobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_Gradings_GradingId",
                        column: x => x.GradingId,
                        principalSchema: "Crm",
                        principalTable: "Gradings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobPositionGradings",
                schema: "Crm",
                columns: table => new
                {
                    JobPositionId = table.Column<Guid>(nullable: false),
                    GradingId = table.Column<Guid>(nullable: false),
                    ExternalHourlyGrade = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositionGradings", x => new { x.JobPositionId, x.GradingId });
                    table.ForeignKey(
                        name: "FK_JobPositionGradings_Gradings_GradingId",
                        column: x => x.GradingId,
                        principalSchema: "Crm",
                        principalTable: "Gradings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobPositionGradings_BusinessJobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalSchema: "Crm",
                        principalTable: "BusinessJobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUnits",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(maxLength: 900, nullable: true),
                    BusinessUnitLeadId = table.Column<Guid>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessUnits_ApplicationUsers_BusinessUnitLeadId",
                        column: x => x.BusinessUnitLeadId,
                        principalSchema: "Crm",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(maxLength: 900, nullable: true),
                    BusinessUnitId = table.Column<Guid>(nullable: true),
                    DepartmentLeadId = table.Column<Guid>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_BusinessUnits_BusinessUnitId",
                        column: x => x.BusinessUnitId,
                        principalSchema: "Crm",
                        principalTable: "BusinessUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Departments_ApplicationUsers_DepartmentLeadId",
                        column: x => x.DepartmentLeadId,
                        principalSchema: "Crm",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentTeams",
                schema: "Crm",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Changed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(maxLength: 900, nullable: true),
                    DepartmentId = table.Column<Guid>(nullable: true),
                    DepartmentTeamLeadId = table.Column<Guid>(nullable: true),
                    Abbreviation = table.Column<string>(nullable: true),
                    RowOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentTeams_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Crm",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepartmentTeams_ApplicationUsers_DepartmentTeamLeadId",
                        column: x => x.DepartmentTeamLeadId,
                        principalSchema: "Crm",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "JobDepartmentTeams",
                schema: "Crm",
                columns: table => new
                {
                    DepartmentTeamId = table.Column<Guid>(nullable: false),
                    JobPositionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDepartmentTeams", x => new { x.JobPositionId, x.DepartmentTeamId });
                    table.ForeignKey(
                        name: "FK_JobDepartmentTeams_DepartmentTeams_DepartmentTeamId",
                        column: x => x.DepartmentTeamId,
                        principalSchema: "Crm",
                        principalTable: "DepartmentTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobDepartmentTeams_BusinessJobPositions_JobPositionId",
                        column: x => x.JobPositionId,
                        principalSchema: "Crm",
                        principalTable: "BusinessJobPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDepartmentTeams",
                schema: "Crm",
                columns: table => new
                {
                    DeparmentTeamId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDepartmentTeams", x => new { x.DeparmentTeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserDepartmentTeams_DepartmentTeams_DeparmentTeamId",
                        column: x => x.DeparmentTeamId,
                        principalSchema: "Crm",
                        principalTable: "DepartmentTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDepartmentTeams_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Crm",
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_BusinessJobPositionId",
                schema: "Crm",
                table: "ApplicationUsers",
                column: "BusinessJobPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsers_GradingId",
                schema: "Crm",
                table: "ApplicationUsers",
                column: "GradingId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUnits_BusinessUnitLeadId",
                schema: "Crm",
                table: "BusinessUnits",
                column: "BusinessUnitLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_BusinessUnitId",
                schema: "Crm",
                table: "Departments",
                column: "BusinessUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentLeadId",
                schema: "Crm",
                table: "Departments",
                column: "DepartmentLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTeams_DepartmentId",
                schema: "Crm",
                table: "DepartmentTeams",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentTeams_DepartmentTeamLeadId",
                schema: "Crm",
                table: "DepartmentTeams",
                column: "DepartmentTeamLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_JobDepartmentTeams_DepartmentTeamId",
                schema: "Crm",
                table: "JobDepartmentTeams",
                column: "DepartmentTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPositionGradings_GradingId",
                schema: "Crm",
                table: "JobPositionGradings",
                column: "GradingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartmentTeams_UserId",
                schema: "Crm",
                table: "UserDepartmentTeams",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobDepartmentTeams",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "JobPositionGradings",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "UserDepartmentTeams",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "DepartmentTeams",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "BusinessUnits",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "ApplicationUsers",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "BusinessJobPositions",
                schema: "Crm");

            migrationBuilder.DropTable(
                name: "Gradings",
                schema: "Crm");
        }
    }
}
