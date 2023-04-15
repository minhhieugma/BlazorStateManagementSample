using Blazored.LocalStorage;
using BlazorSSR.Data;
using BlazorSSR.Store;
using Fluxor;
using Fluxor.Persist.Middleware;
using Fluxor.Persist.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddFluxor(o =>
{
    o.UsePersist();
    o.ScanAssemblies(typeof(Program).Assembly);

#if DEBUG
    o.UseReduxDevTools(rdt =>
    {
        rdt.Name = "My application";
        // rdt.EnableStackTrace();
    });
#endif
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IStringStateStorage, LocalStateStorage>();
builder.Services.AddScoped<IStoreHandler, JsonStoreHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();