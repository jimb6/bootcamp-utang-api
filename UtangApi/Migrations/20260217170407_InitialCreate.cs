using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UtangApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Borrowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EmergencyContactName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrowers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BorrowerId = table.Column<int>(type: "integer", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    InterestMode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TermType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TermCount = table.Column<int>(type: "integer", nullable: false),
                    LiquidationRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false, defaultValue: 0m),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RemainingBalance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AmountPerTerm = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BorrowerId = table.Column<int>(type: "integer", nullable: false),
                    OfferedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    InterestRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    TermMonths = table.Column<int>(type: "integer", nullable: false),
                    OfferDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Borrowers_BorrowerId",
                        column: x => x.BorrowerId,
                        principalTable: "Borrowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContractId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Borrowers",
                columns: new[] { "Id", "Address", "BirthDate", "CreatedAt", "Email", "EmergencyContactName", "EmergencyContactPhone", "FirstName", "LastName", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Cebu City", new DateOnly(1990, 5, 15), new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc), "juan@email.com", "Maria", "09179876543", "Juan", "Dela Cruz", "09171234567", new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc) },
                    { 2, "Manila", new DateOnly(1988, 3, 22), new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc), "maria@email.com", "Jose", "09189876543", "Maria", "Santos", "09181234567", new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc) },
                    { 3, "Davao City", new DateOnly(1995, 11, 8), new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc), "pedro@email.com", null, null, "Pedro", "Reyes", "09191234567", new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AmountPerTerm", "BorrowerId", "CreatedAt", "DueDate", "InterestMode", "InterestRate", "Notes", "PrincipalAmount", "RemainingBalance", "StartDate", "Status", "TermCount", "TermType", "TotalAmount", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 875m, 1, new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 1, 15), "simple", 5m, "", 10000m, 8750m, new DateOnly(2026, 1, 15), "active", 12, "monthly", 10500m, new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc) },
                    { 2, 3433.33m, 2, new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 8, 1), "compound", 3m, "", 20000m, 20600m, new DateOnly(2026, 2, 1), "active", 6, "monthly", 20600m, new DateTime(2026, 1, 15, 8, 30, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "ContractId", "CreatedAt", "Notes", "PaymentDate", "ReceiptNumber" },
                values: new object[,]
                {
                    { 1, 875m, 1, new DateTime(2026, 2, 15, 10, 0, 0, 0, DateTimeKind.Utc), "", new DateOnly(2026, 2, 15), "RCP-001" },
                    { 2, 875m, 1, new DateTime(2026, 3, 15, 10, 0, 0, 0, DateTimeKind.Utc), "", new DateOnly(2026, 3, 15), "RCP-002" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_BorrowerId",
                table: "Contracts",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_BorrowerId",
                table: "Offers",
                column: "BorrowerId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ContractId",
                table: "Payments",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Borrowers");
        }
    }
}
