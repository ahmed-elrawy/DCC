using System;

namespace DCC.API.Dtos
{
    public class UserForUpdateDto
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public string Introducation { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string TypeOfUser { get; set; }
        public string Country { get; set; }
        public string Specialization { get; set; }

    }
}