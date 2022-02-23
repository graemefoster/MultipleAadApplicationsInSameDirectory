using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "SantaWeb_OR_SantaWeb2";
        options.DefaultChallengeScheme = "SantaWeb_OR_SantaWeb2";
    })
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("SantaWeb"));

// builder.Services.AddAuthentication()
//     .AddMicrosoftIdentityWebApp(, openIdConnectScheme: "OpenIdConnect2", cookieScheme: "Cookies2");

builder.Services.AddAuthentication()
    .AddMicrosoftIdentityWebApp(options =>
    {
        var aadOptions = new MicrosoftIdentityOptions();
        builder.Configuration.GetSection("SantaWeb2").Bind(aadOptions);

        options.TenantId = aadOptions.TenantId;
        options.Instance = aadOptions.Instance;
        options.ClientSecret = aadOptions.ClientSecret;
        options.ClientId = aadOptions.ClientId;
        options.UsePkce = aadOptions.UsePkce;
        options.ResponseType = aadOptions.ResponseType;
        options.CallbackPath = aadOptions.CallbackPath;
    }, openIdConnectScheme: "OpenIdConnect2", cookieScheme: "Cookies2");

builder.Services.AddAuthentication()
    .AddPolicyScheme("SantaWeb_OR_SantaWeb2", "SantaWeb_OR_SantaWeb2", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            string host = context.Request.Headers[HeaderNames.Host];
            if (host.Contains("localhost", StringComparison.InvariantCultureIgnoreCase))
            {
                return OpenIdConnectDefaults.AuthenticationScheme;
            }

            return "OpenIdConnect2";
        };
    });

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();