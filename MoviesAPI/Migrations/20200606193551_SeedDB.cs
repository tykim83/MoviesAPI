using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoviesAPI.Migrations
{
    public partial class SeedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "Adventure" },
                    { 5, "Animation" },
                    { 6, "Drama" },
                    { 7, "Romance" }
                });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "InTheathers", "Poster", "ReleaseDate", "Summary", "Title" },
                values: new object[,]
                {
                    { 10, true, null, new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Avengers: Endgame" },
                    { 11, false, null, new DateTime(2019, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Avengers: Infinity Wars" },
                    { 12, false, null, new DateTime(2020, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sonic the Hedgehog" },
                    { 13, false, null, new DateTime(2020, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Emma" },
                    { 14, false, null, new DateTime(2020, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Greed" }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "Biography", "DateOfBirth", "Name", "Picture" },
                values: new object[,]
                {
                    { 8, null, new DateTime(1962, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jim Carrey", null },
                    { 9, null, new DateTime(1965, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Robert Downey Jr.", null },
                    { 10, null, new DateTime(1981, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chris Evans", null }
                });

            migrationBuilder.InsertData(
                table: "MoviesActors",
                columns: new[] { "MovieId", "PersonId", "Character", "Order" },
                values: new object[,]
                {
                    { 12, 8, "Dr. Ivo Robotnik", 1 },
                    { 10, 9, "Tony Stark", 1 },
                    { 11, 9, "Tony Stark", 1 },
                    { 10, 10, "Steve Rogers", 2 },
                    { 11, 10, "Steve Rogers", 2 }
                });

            migrationBuilder.InsertData(
                table: "MoviesGeneres",
                columns: new[] { "MovieId", "GenreId" },
                values: new object[,]
                {
                    { 10, 6 },
                    { 10, 4 },
                    { 11, 6 },
                    { 11, 4 },
                    { 12, 4 },
                    { 13, 6 },
                    { 13, 7 },
                    { 14, 6 },
                    { 14, 7 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "MovieId", "PersonId" },
                keyValues: new object[] { 10, 9 });

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "MovieId", "PersonId" },
                keyValues: new object[] { 10, 10 });

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "MovieId", "PersonId" },
                keyValues: new object[] { 11, 9 });

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "MovieId", "PersonId" },
                keyValues: new object[] { 11, 10 });

            migrationBuilder.DeleteData(
                table: "MoviesActors",
                keyColumns: new[] { "MovieId", "PersonId" },
                keyValues: new object[] { 12, 8 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 10, 4 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 10, 6 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 11, 4 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 11, 6 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 12, 4 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 13, 6 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 13, 7 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 14, 6 });

            migrationBuilder.DeleteData(
                table: "MoviesGeneres",
                keyColumns: new[] { "MovieId", "GenreId" },
                keyValues: new object[] { 14, 7 });

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
