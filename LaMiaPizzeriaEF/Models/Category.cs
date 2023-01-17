namespace LaMiaPizzeriaEF.Models {
    public class Category {
        public Category() { }
        public Category(string name) {
            Name = name;
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [InverseProperty(nameof(Pizza.Category))]
        public List<Pizza> Pizzas { get; set; }

        public override string ToString() {
            return Name;
        }

        public static implicit operator string(Category category) => category.Name;
        public static explicit operator Category(string value) => new(value);
    }
}
