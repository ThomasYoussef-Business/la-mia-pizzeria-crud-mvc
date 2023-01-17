namespace LaMiaPizzeriaEF.Models {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class EndsWithAttribute : ValidationAttribute {
        public string[] ValidEnds { get; init; }
        public EndsWithAttribute(params string[] validEnds) {
            ValidEnds = validEnds;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext) {
            if (value is not string castedValue) { return new ValidationResult("Il valore inserito non è una valida stringa."); }

            foreach (string validEnd in ValidEnds) {
                if (castedValue.EndsWith(validEnd)) { return ValidationResult.Success; }
            }

            return new ValidationResult($"Il valore inserito non finisce con una delle seguenti stringhe:" +
                $"{string.Join(", ", ValidEnds)}");
        }
    }
}
