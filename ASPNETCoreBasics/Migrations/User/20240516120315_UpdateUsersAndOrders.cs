using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNETCoreBasics.Migrations.User
{
    /// <inheritdoc />
    public partial class UpdateUsersAndOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Usuarios");

            migrationBuilder.AddColumn<int>(
                name: "UserModelId",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UserModelId",
                table: "Pedidos",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Usuarios_UserModelId",
                table: "Pedidos",
                column: "UserModelId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Usuarios_UserModelId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_UserModelId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Pedidos");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}