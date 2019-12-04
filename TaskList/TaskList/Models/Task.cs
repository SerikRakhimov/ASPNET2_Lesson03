using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskList.Models
{
    public class Task
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [MaxLength(50, ErrorMessage = "Максимум 50 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public string Description { get; set; }
        public bool Perform { get; set; }
    }
}