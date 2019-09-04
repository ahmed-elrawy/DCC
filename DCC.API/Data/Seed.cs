using System;
using System.Collections.Generic;
using System.Linq;
using DCC.API.Model;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DCC.API.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(DataContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;

        }

        public async void Initialize()
        {

            var BodyAreasData = System.IO.File.ReadAllText("Data/BodyAreasSeed.json");
            var DrugTypeData = System.IO.File.ReadAllText("Data/DrugTypeSeed.json");
            var DrugsData = System.IO.File.ReadAllText("Data/DrugSeed.json");

            var BodyAreas = JsonConvert.DeserializeObject<List<BodyAreas>>(BodyAreasData); ;

            var DrugTypes = JsonConvert.DeserializeObject<List<DrugType>>(DrugTypeData);
            var Drugs = JsonConvert.DeserializeObject<List<Drug>>(DrugsData);

            _context.Database.EnsureCreated();

            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeed.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role> {
                    new Role{ Name = "Doctors"},
                    new Role{ Name = "Patient"},
                    new Role{ Name = "Admin"},
                    new Role{ Name = "Moderator"},
                    new Role{ Name = "VIP"},


                };
                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                    
                }
 
                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "password").Wait();
                    _userManager.AddToRoleAsync(user, "Patient").Wait();
                }
               
            }



            if (!_context.BodyAreas.Any())
            {
                foreach (var body in BodyAreas)
                {
                    _context.BodyAreas.Add(body);

                }
                _context.SaveChanges();
            }
            if (!_context.DrugTyps.Any())
            {
                foreach (var type in DrugTypes)
                {
                    _context.DrugTyps.Add(type);

                }
                await _context.SaveChangesAsync();
            }

            if (!_context.Drugs.Any())
            {
                foreach (var drug in Drugs)
                {
                    _context.Drugs.Add(drug);

                }
                _context.SaveChanges();
            }

        }

    }
}