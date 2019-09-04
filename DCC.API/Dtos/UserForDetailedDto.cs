using System;
using System.Collections.Generic;

namespace DatingApp.API.Dtos
{
    public class UserForDetailedDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
         public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introducation { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string TypeOfUser { get; set; }
        public string PhotoUrl { get; set; }
        public int LikerCount { get; set; }
        public string Specialization { get; set; }

        public ICollection<PhotosForDetailedDto> Photos { get; set; }
    }
}