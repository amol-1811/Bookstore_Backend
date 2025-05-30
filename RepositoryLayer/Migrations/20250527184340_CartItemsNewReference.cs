using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class CartItemsNewReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Cart_CartEntityCartId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartEntityCartId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartEntityCartId",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Cart_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Cart_CartId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "CartEntityCartId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartEntityCartId",
                table: "CartItems",
                column: "CartEntityCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Cart_CartEntityCartId",
                table: "CartItems",
                column: "CartEntityCartId",
                principalTable: "Cart",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
