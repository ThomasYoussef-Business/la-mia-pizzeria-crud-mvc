namespace LaMiaPizzeriaEF.Models {
    public class PizzaViewModel {
        public Pizza Pizza { get; set; }
        public List<Category>? Categories { get; set; } = new List<Category>();
        public List<SelectListItem>? Tags { get; set; } = new List<SelectListItem>();
        public List<string>? TagIds { get; set; } = new List<string>();
    }
}
