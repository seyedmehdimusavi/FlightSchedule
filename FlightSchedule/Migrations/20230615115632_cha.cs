using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlightSchedule.Migrations
{
    /// <inheritdoc />
    public partial class cha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_routes_flights_flightid",
                table: "routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_routes",
                table: "routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_flights",
                table: "flights");

            migrationBuilder.DropColumn(
                name: "id",
                table: "routes");

            migrationBuilder.DropColumn(
                name: "id",
                table: "flights");

            migrationBuilder.RenameColumn(
                name: "flightid",
                table: "routes",
                newName: "flight_id");

            migrationBuilder.RenameIndex(
                name: "IX_routes_flightid",
                table: "routes",
                newName: "IX_routes_flight_id");

            migrationBuilder.AlterColumn<int>(
                name: "route_id",
                table: "routes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "flight_id",
                table: "flights",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_routes",
                table: "routes",
                column: "route_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_flights",
                table: "flights",
                column: "flight_id");

            migrationBuilder.AddForeignKey(
                name: "FK_routes_flights_flight_id",
                table: "routes",
                column: "flight_id",
                principalTable: "flights",
                principalColumn: "flight_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_routes_flights_flight_id",
                table: "routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_routes",
                table: "routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_flights",
                table: "flights");

            migrationBuilder.RenameColumn(
                name: "flight_id",
                table: "routes",
                newName: "flightid");

            migrationBuilder.RenameIndex(
                name: "IX_routes_flight_id",
                table: "routes",
                newName: "IX_routes_flightid");

            migrationBuilder.AlterColumn<int>(
                name: "route_id",
                table: "routes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "routes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "flight_id",
                table: "flights",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "flights",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_routes",
                table: "routes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_flights",
                table: "flights",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_routes_flights_flightid",
                table: "routes",
                column: "flightid",
                principalTable: "flights",
                principalColumn: "id");
        }
    }
}
