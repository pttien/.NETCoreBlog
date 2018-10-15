using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogDemo.Web.Migrations
{
    public partial class jhagff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_AspNetUsers_UserId",
                table: "PostCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostCategories_PostCategoryId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_PostCategories_UserId",
                table: "PostCategories");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PostCategories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PostCategories");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "PostCategories");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PostCategories",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "PostCategoryId",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PostCategories",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostCategories_PostCategoryId",
                table: "Posts",
                column: "PostCategoryId",
                principalTable: "PostCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostCategories_PostCategoryId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PostCategories",
                newName: "UserId");

            migrationBuilder.AlterColumn<int>(
                name: "PostCategoryId",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PostCategories",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PostCategories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PostCategories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "PostCategories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategories_UserId",
                table: "PostCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_AspNetUsers_UserId",
                table: "PostCategories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostCategories_PostCategoryId",
                table: "Posts",
                column: "PostCategoryId",
                principalTable: "PostCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
