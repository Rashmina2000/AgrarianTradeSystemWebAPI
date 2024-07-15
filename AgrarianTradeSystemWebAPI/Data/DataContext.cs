using AgrarianTradeSystemWebAPI.Models;
using AgrarianTradeSystemWebAPI.Models.AdminModels;
using AgrarianTradeSystemWebAPI.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace AgrarianTradeSystemWebAPI.Data
{
	public class DataContext : DbContext

	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer("Data Source=SQL8010.site4now.net;Initial Catalog=db_aa969d_atsdb;User Id=db_aa969d_atsdb_admin;Password=syntec@123");
		}

        public DbSet<User> Users { get; set; }
		public DbSet<Farmer> Farmers { get; set; }
		public DbSet<Courier> Couriers { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Cart> Cart { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Orders> Orders { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<Returns> Returns { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<Notification> Notifications { get; set; }

	}

}
