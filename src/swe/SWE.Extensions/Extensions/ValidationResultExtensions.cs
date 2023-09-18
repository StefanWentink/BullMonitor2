
using FluentValidation.Results;
using System.Text;

namespace SWE.Extensions.Extensions
{
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Extension displaying <see cref="ValidationResult"/> to string.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string ToMessage(this ValidationResult result)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(nameof(result.IsValid));
            stringBuilder.Append(":");
            stringBuilder.Append(result.IsValid);

            foreach (var error in result.Errors)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(error.PropertyName);
                stringBuilder.Append(":");
                stringBuilder.Append(error.ErrorMessage);
            }

            return stringBuilder.ToString();
        }

        public static ValidationResult Concat(
            this IEnumerable<ValidationResult> self)
        {
            return new ValidationResult(self.SelectMany(x => x.Errors));
        }

        public static void GuardValid(this ValidationResult self, string subject)
        {
            if (!self.IsValid)
            {
                if (string.IsNullOrWhiteSpace(subject))
                {
                    throw new Exception(self.ToMessage());
                }

                throw new Exception($"{ subject }: { self.ToMessage() }");
            }
        }
    }
}