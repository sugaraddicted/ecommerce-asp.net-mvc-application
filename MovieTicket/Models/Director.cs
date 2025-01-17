﻿using MovieTicket.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace MovieTicket.Models
{
    public class Director : IEntityBase
    {
        [Key]

        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile Picture is required")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }


        /*-----Relationships-----*/

        public List<Movie>? Movies { get; set; }
    }
}
