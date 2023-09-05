using System.Text;
using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.BusinessLayer.Concrete;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>();

builder.Services.AddScoped<IRoomNumberDAL, EFRoomNumberDAL>();
builder.Services.AddScoped<IRoomNumberService, RoomNumberManager>();

builder.Services.AddScoped<IRoomDAL, EFRoomDAL>();
builder.Services.AddScoped<IRoomService, RoomManager>();

builder.Services.AddScoped<IHotelDAL, EFHotelDAL>();
builder.Services.AddScoped<IHotelService, HotelManager>();

builder.Services.AddScoped<IBookingDAL, EFBookingDAL>();
builder.Services.AddScoped<IBookingService, BookingManager>();

builder.Services.AddScoped<IRoomSpecsDAL, EFRoomsSpecsDAL>();
builder.Services.AddScoped<IRoomSpecsService, RoomSpecsManager>();

builder.Services.AddScoped<IHotelCategoryDAL, EFHotelCategoryDAL>();
builder.Services.AddScoped<IHotelCategoryService, HotelCategoryManager>();

builder.Services.AddScoped<IHotelCommentDAL, EFHotelCommentDAL>();
builder.Services.AddScoped<IHotelCommentService, HotelCommentManager>();


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = true;
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = "https://localhost",
        ValidAudience = "https://localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hotelandtoursapiproject")),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("OtelApiCors", opts =>
    {
        opts.WithOrigins("https://localhost:7236")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("OtelApiCors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();