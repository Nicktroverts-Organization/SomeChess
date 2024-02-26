using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SomeChess.Code;
using SomeChess.Code.MatchMaking;
using SomeChess.Code.Social;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<PlayerStorage>();
builder.Services.AddSingleton<MatchSearching>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
