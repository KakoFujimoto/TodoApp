using System.ComponentModel.DataAnnotations;
using TodoApp.Models;

namespace TodoApp.Validation
{
    public class PriorityValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is Priority priority)
            {
                if (!Enum.IsDefined(priority))
                {
                    return false;
                }
                return priority != Priority.None;
            }
            return false;
        }
    }
}