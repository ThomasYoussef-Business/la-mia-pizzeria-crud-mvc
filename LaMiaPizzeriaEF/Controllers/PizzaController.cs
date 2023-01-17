using LaMiaPizzeriaEF.Database;
using LaMiaPizzeriaEF.Models;
using Microsoft.AspNetCore.Mvc;

namespace LaMiaPizzeriaEF.Controllers {
    public class PizzaController : Controller {
        // MAIN PAGE
        [Route("/pizze")]
        public IActionResult Index() {
            using var db = new PizzasDbContext();
            List<Pizza> pizzas = db.Pizzas.ToList();

            foreach (Pizza pizza in pizzas) {

                db.Entry(pizza)
                  .Reference(p => p.Category)
                  .Load();
            }

            return View(pizzas);
        }

        [Route("pizze/{id}")]
        public IActionResult Pizza(int id) {
            using var db = new PizzasDbContext();
            Pizza? pizza = db.Pizzas.Find(id);
            db.Entry(pizza)
              .Reference(p => p.Category)
              .Load();
            return View(pizza);
        }

        // ADD A NEW PIZZA
        public IActionResult New() {
            using var db = new PizzasDbContext();
            PizzaCategoriesView DataTransferObject = new() {
                Pizza = new Pizza(),
                Categories = db.Categories.ToList()
            };
            return View(DataTransferObject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(PizzaCategoriesView DataTransferObject) {
            if (!ModelState.IsValid) {
                using (var db = new PizzasDbContext()) {
                    DataTransferObject.Categories = db.Categories.ToList();
                    return View(DataTransferObject);
                }
            }

            using (var db = new PizzasDbContext()) {
                int modifications = db.AddPizza(DataTransferObject.Pizza);
                Console.WriteLine(modifications);
            }

            return RedirectToAction(nameof(Index));
        }

        // EDIT A PIZZA
        public IActionResult Edit(int id) {
            using PizzasDbContext db = new();
            if (db.Pizzas.Find(id) is Pizza pizzaToEdit) {
                PizzaCategoriesView DataTransferObject = new() {
                    Pizza = pizzaToEdit,
                    Categories = db.Categories.ToList()
                };
                return View(DataTransferObject);
            }
            else {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Pizza pizza) {
            if (!ModelState.IsValid) {
                using var db = new PizzasDbContext();
                PizzaCategoriesView DataTransferObject = new() {
                    Pizza = pizza,
                    Categories = db.Categories.ToList()
                };
                return View(DataTransferObject);
            }

            using (var db = new PizzasDbContext()) {
                if (db.Pizzas.FirstOrDefault(p => p.Id == id) is Pizza pizzaToModify) {
                    pizzaToModify.Name = pizza.Name;
                    pizzaToModify.Description = pizza.Description;
                    pizzaToModify.Price = pizza.Price;
                    pizzaToModify.PictureUrl = pizza.PictureUrl;
                    pizzaToModify.CategoryId = pizza.CategoryId;
                    db.SaveChanges();
                }
                else {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE A PIZZA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id) {
            using PizzasDbContext db = new();
            int deletedItems = db.DeletePizza(id);

            if (deletedItems > 0) {
                return RedirectToAction(nameof(Index));
            }
            else {
                return NotFound();
            }
        }
    }
}
