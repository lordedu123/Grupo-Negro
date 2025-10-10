using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grupo_negro.Migrations
{
    /// <inheritdoc />
    public partial class AgregarSaldoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Saldo",
                table: "AspNetUsers",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "AspNetUsers");
        }
    }
}
