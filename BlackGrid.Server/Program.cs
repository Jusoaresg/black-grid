using BlackGrid.Server.Infrastructure;
using BlackGrid.Server.Websockets;

var builder = WebApplication.CreateBuilder(args);
var contentRoot = builder.Environment.ContentRootPath;
ServerPaths.Init(contentRoot);

var app = builder.Build();


app.UseWebSockets();

app.Map("ws/", async context =>
{
	if (!context.WebSockets.IsWebSocketRequest)
	{
		context.Response.StatusCode = 400;
		return;
	}

	var socket = await context.WebSockets.AcceptWebSocketAsync();
	var connectionHandler = new ConnectionHandler();
	await connectionHandler.HandleConnection(socket);
});

app.Run();
