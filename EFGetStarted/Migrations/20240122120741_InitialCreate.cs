using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFGetStarted.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TasksId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentTaskTasksId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Teams_Tasks_CurrentTaskTasksId",
                        column: x => x.CurrentTaskTasksId,
                        principalTable: "Tasks",
                        principalColumn: "TasksId");
                });

            migrationBuilder.CreateTable(
                name: "TeamWorkers",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamWorkers", x => new { x.WorkerId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_TeamWorkers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    TodoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsComplete = table.Column<bool>(type: "INTEGER", nullable: false),
                    TasksId = table.Column<int>(type: "INTEGER", nullable: true),
                    WorkerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.TodoId);
                    table.ForeignKey(
                        name: "FK_Todos_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "TasksId");
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentTodoTodoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerId);
                    table.ForeignKey(
                        name: "FK_Workers_Todos_CurrentTodoTodoId",
                        column: x => x.CurrentTodoTodoId,
                        principalTable: "Todos",
                        principalColumn: "TodoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TeamId",
                table: "Tasks",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CurrentTaskTasksId",
                table: "Teams",
                column: "CurrentTaskTasksId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamWorkers_TeamId",
                table: "TeamWorkers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_TasksId",
                table: "Todos",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_WorkerId",
                table: "Todos",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Workers_CurrentTodoTodoId",
                table: "Workers",
                column: "CurrentTodoTodoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Teams_TeamId",
                table: "Tasks",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamWorkers_Workers_WorkerId",
                table: "TeamWorkers",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "WorkerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Workers_WorkerId",
                table: "Todos",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Teams_TeamId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Tasks_TasksId",
                table: "Todos");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Workers_WorkerId",
                table: "Todos");

            migrationBuilder.DropTable(
                name: "TeamWorkers");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
