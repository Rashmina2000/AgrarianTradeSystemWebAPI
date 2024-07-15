using AgrarianTradeSystemWebAPI.Data;
using AgrarianTradeSystemWebAPI.Services.EmailService;
using AgrarianTradeSystemWebAPI.Services.ProductServices;
using AgrarianTradeSystemWebAPI.Services.ReviewServices;
using AgrarianTradeSystemWebAPI.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AgrarianTradeSystemWebAPI.Services.AdminServices;
using AgrarianTradeSystemWebAPI.Services.NewOrderServices;
using AgrarianTradeSystemWebAPI.Services.OrderServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbu	ckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//auto mapper service setup
builder.Services.AddAutoMapper(typeof(Program).Assembly);



//add connection azure blob
builder.Services.AddScoped(_ =>
{
	return new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage"));
});
//register Services
builder.Services.AddScoped<IFileServices, FileServices>();
builder.Services.AddScoped<IReviewServices, ReviewServices>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IReturnServices, ReturnServices>();
builder.Services.AddScoped<IShoppingCartServices, ShoppingCartServices>();

//add cors for connect react and .net
builder.Services.AddCors(option =>
{
	option.AddPolicy(name: "ReactJSDomain",
		policy => policy.WithOrigins("*")
		.AllowAnyHeader()
		.AllowAnyMethod()
		.AllowAnyOrigin()
	);

});

builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IAdminServices, AdminServices>();
builder.Services.AddScoped<INewOrderServices, NewOrderServices>();
builder.Services.AddScoped<INotificationServices, NotificationServices>();

//builder.Services.AddScoped<IUserServices>(sp =>
// {
//     var dbContext = sp.GetRequiredService<DataContext>();
//     var configuration = sp.GetRequiredService<IConfiguration>();
//     return new UserServices(dbContext, configuration);
// });
builder.Services.AddDbContext<DataContext>();
builder.Services.AddDbContext<DataContext>();

builder.Services.AddDbContext<DataContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DataContext")));

builder.Services.AddCors(option =>
{
	option.AddPolicy(name: "ReactJSDomain",
        policy => policy.WithOrigins("https://agrarian-trade-system-frontend.vercel.app", "http://localhost:5173" , "https://ats-admin-dashboard.vercel.app").AllowAnyHeader().AllowAnyMethod());
});

builder.Services.AddAuthentication()
.AddJwtBearer("JwtBearer", jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:SecretKey"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
    };
});

//builder.Services.AddAuthentication(options =>
//{
//	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//            {
//                options.SaveToken = true;
//                options.RequireHttpsMetadata = false;
//                options.TokenValidationParameters = new TokenValidationParameters()
//                {
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
//                    ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
//                    ClockSkew = TimeSpan.Zero,
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:SecretKey"]!))
//                };
//            });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseCors("ReactJSDomain");

app.MapControllers();

app.Run();
