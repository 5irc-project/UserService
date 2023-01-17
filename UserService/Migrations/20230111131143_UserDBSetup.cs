using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserService.Migrations
{
    public partial class UserDBSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "t_e_user_usr",
                schema: "public",
                columns: table => new
                {
                    usr_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    usr_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    usr_email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    usr_profile_picture_url = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_uti", x => x.usr_id);
                });

            migrationBuilder.CreateIndex(
                name: "uq_utl_mail",
                schema: "public",
                table: "t_e_user_usr",
                column: "usr_email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_user_usr",
                schema: "public");
        }
    }
}
