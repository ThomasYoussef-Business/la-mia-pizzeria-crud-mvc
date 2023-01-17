using LaMiaPizzeriaEF.Models;

namespace LaMiaPizzeriaEF.Database {
    public class PizzasDbContext : DbContext {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=PizzaShop;" +
                                        "Integrated Security=True;TrustServerCertificate=True");
        }

        public int AddPizza(Pizza pizza) {
            Pizzas.Add(pizza);
            return SaveChanges();
        }

        public int DeletePizza(int id) {
            if (Pizzas.Find(id) is Pizza pizzaToRemove) {
                Pizzas.Remove(pizzaToRemove);
            }
            return SaveChanges();
        }

        public int AddCategory(Category category) {
            Categories.Add(category);
            return SaveChanges();
        }

        public int DeleteCategory(int id) {
            if (Categories.Find(id) is Category categoryToRemove) {
                Categories.Remove(categoryToRemove);
            }
            return SaveChanges();
        }
    }
}
