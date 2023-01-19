using LaMiaPizzeriaEF.Database;

namespace LaMiaPizzeriaEF.Models {
    public class Tag {
        #region Database Properties
        [Key]
        public int Id { get; set; }
        [MaxLength(64)]
        public string Text { get; set; }
        #endregion

        #region Database Relationships
        public List<Pizza> Pizzas { get; set; } = new List<Pizza>();
        #endregion

        #region Conversions
        public static implicit operator SelectListItem(Tag tag) {
            return new() {
                Value = tag.Id.ToString(),
                Text = tag.Text
            };
        }

        public static Tag? FromSelectListItem(SelectListItem item, PizzasDbContext db) {
            return db.Tags.Find(item.Value);
        }
        #endregion
    }
}
