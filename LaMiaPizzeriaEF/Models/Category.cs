namespace LaMiaPizzeriaEF.Models {
    public class Category {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [InverseProperty(nameof(Pizza.Category))]
        public List<Pizza> Pizzas { get; set; }
    }
}
