using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthDiary.Data.Migrations
{
    /// <inheritdoc />
    public partial class StepLabDomainModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShoreHardness = table.Column<double>(type: "float", nullable: false),
                    PricePerGram = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExtrusionTemp = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeightKg = table.Column<double>(type: "float", nullable: false),
                    ShoeSize = table.Column<double>(type: "float", nullable: false),
                    DiagnosisNotes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GaitAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalysisDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VideoFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PronationAngle = table.Column<double>(type: "float", nullable: false),
                    NeedsCorrection = table.Column<bool>(type: "bit", nullable: false),
                    CVProcessingLog = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GaitAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GaitAnalyses_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GaitAnalyses_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InsoleOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrinterModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    GCodeFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GaitAnalysisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsoleOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsoleOrders_GaitAnalyses_GaitAnalysisId",
                        column: x => x.GaitAnalysisId,
                        principalTable: "GaitAnalyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InsoleOrderMaterials",
                columns: table => new
                {
                    InsoleOrdersId = table.Column<int>(type: "int", nullable: false),
                    MaterialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsoleOrderMaterials", x => new { x.InsoleOrdersId, x.MaterialsId });
                    table.ForeignKey(
                        name: "FK_InsoleOrderMaterials_InsoleOrders_InsoleOrdersId",
                        column: x => x.InsoleOrdersId,
                        principalTable: "InsoleOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsoleOrderMaterials_Materials_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GaitAnalyses_DoctorId",
                table: "GaitAnalyses",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_GaitAnalyses_PatientId",
                table: "GaitAnalyses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_InsoleOrderMaterials_MaterialsId",
                table: "InsoleOrderMaterials",
                column: "MaterialsId");

            migrationBuilder.CreateIndex(
                name: "IX_InsoleOrders_GaitAnalysisId",
                table: "InsoleOrders",
                column: "GaitAnalysisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsoleOrderMaterials");

            migrationBuilder.DropTable(
                name: "InsoleOrders");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "GaitAnalyses");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
