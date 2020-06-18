using Microsoft.EntityFrameworkCore.Migrations;

namespace MoviesAPI.Migrations
{
    public partial class AddAdminRoleValueToAspNetRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                Insert into AspNetRoles( Id, [name], [NormalizedName])
                values('4c5ac988-3b2f-42ab-91b4-e7450cdc975b', 'Admin', 'Admin')
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                delete AspNetRoles
                where Id = '4c5ac988-3b2f-42ab-91b4-e7450cdc975b'
            ");
        }
    }
}