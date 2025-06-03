using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class CartItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemEntity_Books_BookId",
                table: "CartItemEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItemEntity_Cart_CartId",
                table: "CartItemEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItemEntity",
                table: "CartItemEntity");

            migrationBuilder.RenameTable(
                name: "CartItemEntity",
                newName: "CartItems");

            migrationBuilder.RenameIndex(
                name: "IX_CartItemEntity_CartId",
                table: "CartItems",
                newName: "IX_CartItems_CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItemEntity_BookId",
                table: "CartItems",
                newName: "IX_CartItems_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Books_BookId",
                table: "CartItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_CartItems_Books_BookId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Cart_CartId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "CartItemEntity");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartId",
                table: "CartItemEntity",
                newName: "IX_CartItemEntity_CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_BookId",
                table: "CartItemEntity",
                newName: "IX_CartItemEntity_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItemEntity",
                table: "CartItemEntity",
                column: "CartItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemEntity_Books_BookId",
                table: "CartItemEntity",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemEntity_Cart_CartId",
                table: "CartItemEntity",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "CartId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
