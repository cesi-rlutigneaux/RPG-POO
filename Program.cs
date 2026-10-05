using RPG_POO.Api;
using RPG_POO.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<GameStateService>();

var app = builder.Build();
SaveService.EnsureDbCreated();

app.UseDefaultFiles();
app.UseStaticFiles();

var game = app.MapGroup("/api/game");
game.MapGet("/", (GameStateService state) => Results.Ok(state.GetSnapshot()));
game.MapPost("/new", (NewGameRequest request, GameStateService state) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
        return Results.BadRequest(new { message = "Le nom du héros est obligatoire." });
    return Results.Ok(state.NewGame(request));
});
game.MapPost("/load", (GameStateService state) =>
{
    var snapshot = state.Load();
    return snapshot is null
        ? Results.NotFound(new { message = "Aucune sauvegarde trouvée." })
        : Results.Ok(snapshot);
});
game.MapPost("/explore", (GameStateService state) => Execute(state.Explore));
game.MapPost("/action", (CombatActionRequest request, GameStateService state) =>
    Execute(() => state.PlayTurn(request.Action)));
game.MapPost("/save", (GameStateService state) => Execute(state.Save));
game.MapDelete("/save", (GameStateService state) =>
{
    state.DeleteSave();
    return Results.NoContent();
});

app.MapFallbackToFile("index.html");
app.Run();

static IResult Execute(Func<GameSnapshot> action)
{
    try { return Results.Ok(action()); }
    catch (InvalidOperationException error) { return Results.BadRequest(new { message = error.Message }); }
}
