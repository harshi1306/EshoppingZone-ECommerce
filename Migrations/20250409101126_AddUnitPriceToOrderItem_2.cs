﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EshoppingZoneAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUnitPriceToOrderItem_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItems",
                newName: "UnitPrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderItems",
                newName: "Price");
        }
    }
}
