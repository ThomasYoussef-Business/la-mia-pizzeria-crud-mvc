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

        public SelectListItem ToSelectListItem() {
            return (SelectListItem)this;
        }

        public static SelectListItem ToSelectListItem(Tag tag) {
            return (SelectListItem)tag;
        }

        public static Tag? FromSelectListItem(SelectListItem item, PizzasDbContext db) {
            return db.Tags.Find(item.Value);
        }
        #endregion
    }

    public static class TagConverter {
        public static List<SelectListItem> ToSelectListItem(this IEnumerable<Tag> tags) {
            return tags.Select(t => t.ToSelectListItem()).ToList();
        }

        /// <summary>
        /// Given a Database Context containing the Tags DbSet, returns a list of tags with the given IDs, converted from strings.
        /// </summary>
        /// <param name="tagIds"></param>
        /// <param name="db"></param>
        /// <returns>A list of tags where the ID is in the given list of strings</returns>
        public static List<Tag> ToTagList(this IEnumerable<string> tagIds, PizzasDbContext db) {
            return tagIds.Select(id => db.Tags.Find(int.Parse(id))).ToList();
        }
    }
}
