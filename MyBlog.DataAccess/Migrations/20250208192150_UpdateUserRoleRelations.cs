using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.DataAccess.Migrations
{
    public partial class UpdateUserRoleRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_ApplicationUserId",
                schema: "blog",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_ApplicationUserId",
                schema: "blog",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                schema: "blog",
                table: "AspNetUserRoles");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "blog",
                table: "AspNetUserRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "blog",
                table: "AspNetUserRoles");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                schema: "blog",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_ApplicationUserId",
                schema: "blog",
                table: "AspNetUserRoles",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_ApplicationUserId",
                schema: "blog",
                table: "AspNetUserRoles",
                column: "ApplicationUserId",
                principalSchema: "blog",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
