using System.ComponentModel.DataAnnotations;

namespace TodoApp.Validation
{
    public class DueDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dueDate)
            {
                return dueDate.Date >= DateTime.Today;
            }
            return true;
        }

    }
}