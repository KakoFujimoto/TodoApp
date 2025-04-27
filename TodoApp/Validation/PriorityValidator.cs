using System.ComponentModel.DataAnnotations;
using TodoApp.Models;

namespace TodoApp.Validation
{
    public class PriorityValidator : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is Priority priority)
            {
                return priority == Priority.Low || priority == Priority.Normal || priority == Priority.Urgent;
            }
            return false;
        }
    }
}