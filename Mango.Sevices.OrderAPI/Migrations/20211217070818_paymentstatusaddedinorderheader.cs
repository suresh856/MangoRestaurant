using Microsoft.EntityFrameworkCore.Migrations;

namespace Mango.Sevices.OrderAPI.Migrations
{
    public partial class paymentstatusaddedinorderheader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PaymentStatus",
                table: "OrderHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderHeaders");
        }
    }
}
