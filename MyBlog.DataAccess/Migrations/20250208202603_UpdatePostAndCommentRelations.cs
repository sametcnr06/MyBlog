using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBlog.DataAccess.Migrations
{
    public partial class UpdatePostAndCommentRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApprovedByUserId1",
                schema: "blog",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId1",
                schema: "blog",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AuthorId1",
                schema: "blog",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ApprovedByUserId1",
                schema: "blog",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                schema: "blog",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId1",
                schema: "blog",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                schema: "blog",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ApprovedByUserId",
                schema: "blog",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                schema: "blog",
                table: "Posts",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApprovedByUserId",
                schema: "blog",
                table: "Comments",
                column: "ApprovedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApprovedByUserId",
                schema: "blog",
                table: "Comments",
                column: "ApprovedByUserId",
                principalSchema: "blog",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                schema: "blog",
                table: "Posts",
                column: "AuthorId",
                principalSchema: "blog",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_ApprovedByUserId",
                schema: "blog",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId",
                schema: "blog",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_AuthorId",
                schema: "blog",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ApprovedByUserId",
                schema: "blog",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorId",
                schema: "blog",
                table: "Posts",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId1",
                schema: "blog",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApprovedByUserId",
                schema: "blog",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserId1",
                schema: "blog",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId1",
                schema: "blog",
                table: "Posts",
                column: "AuthorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApprovedByUserId1",
                schema: "blog",
                table: "Comments",
                column: "ApprovedByUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_ApprovedByUserId1",
                schema: "blog",
                table: "Comments",
                column: "ApprovedByUserId1",
                principalSchema: "blog",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_AuthorId1",
                schema: "blog",
                table: "Posts",
                column: "AuthorId1",
                principalSchema: "blog",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
