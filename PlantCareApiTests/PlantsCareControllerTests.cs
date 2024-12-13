using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantCareBackEnd.Controllers;
using PlantCareBackEnd.Data;
using PlantCareBackEnd.Models;

namespace PlantCareApiTests
{
    public class PlantsCareControllerTests
    {
        //creamos el contexto de la memoria temporal para pruebas.
        private PlantDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<PlantDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new PlantDbContext(options);
        }

        //TestCase para el api get all plant cares
        //Escenario caso 1, se registran 2 se esperan listar 2 registros
        [Fact]
        public void TestApi_GetAll_Case_1()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Plants.AddRange(
                new Plant { Id = Guid.NewGuid().ToString(), Name = "Aloe Vera", Type = "succulent", WateringFrequencyDays = 7, LastWateredDate = DateTime.Now, Location = "Kitchen" },
                new Plant { Id = Guid.NewGuid().ToString(), Name = "Basil", Type = "herb", WateringFrequencyDays = 3, LastWateredDate = DateTime.Now, Location = "Garden" }
            );
            context.SaveChanges();

            var controller = new PlantsCareController(context);

            // Act
            var result = controller.GetAll();

            // Assert
            var plants = Assert.IsType<List<Plant>>(result.Value);
            Assert.Equal(2, plants.Count);
        }

        //TestCase para el api get all plant cares
        //Escenario caso 2, se registran 2 se esperan listar 4 registros
        [Fact]
        public void TestApi_GetAll_Case_2_No_Ok()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Plants.AddRange(
                new Plant { Id = Guid.NewGuid().ToString(), Name = "Aloe Vera", Type = "succulent", WateringFrequencyDays = 7, LastWateredDate = DateTime.Now, Location = "Kitchen" },
                new Plant { Id = Guid.NewGuid().ToString(), Name = "Basil", Type = "herb", WateringFrequencyDays = 3, LastWateredDate = DateTime.Now, Location = "Garden" }
            );
            context.SaveChanges();

            var controller = new PlantsCareController(context);

            // Act
            var result = controller.GetAll();

            // Assert
            var plants = Assert.IsType<List<Plant>>(result.Value);
            Assert.NotEqual(4, plants.Count);
        }


        //TestCase para el api create new plant care
        //Escenario caso 1, se registran cactus y se espera cactus
        [Fact]
        public void TestApi_Create_Case_1()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new PlantsCareController(context);

            var newPlant = new Plant
            {
                Name = "Cactus",
                Type = "cacti",
                WateringFrequencyDays = 14,
                LastWateredDate = DateTime.Now,
                Location = "Office"
            };

            // Act
            var result = controller.Create(newPlant);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Single(context.Plants);
            Assert.Equal("Cactus", context.Plants.First().Name);
        }


        //TestCase para el api create new plant care
        //Escenario caso 2, se registran cactus y se espera aloe vera
        [Fact]
        public void TestApi_Create_Case_2_No_Ok()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new PlantsCareController(context);

            var newPlant = new Plant
            {
                Name = "Cactus",
                Type = "cacti",
                WateringFrequencyDays = 14,
                LastWateredDate = DateTime.Now,
                Location = "Office"
            };

            // Act
            var result = controller.Create(newPlant);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Single(context.Plants);
            Assert.NotEqual("Aloe Vera", context.Plants.First().Name);
        }

        //TestCase para el api update plant care
        //Escenario caso 1, se registran y actualizan datos para un 41ad872b-0e57-4859-8ad4-d252f6f551c5
        [Fact]
        public void TestApi_Update_Case_1()
        {
            var id = "41ad872b-0e57-4859-8ad4-d252f6f551c5";
            // Arrange
            var context = GetInMemoryDbContext();
            
            var newplant = new Plant { Id = id, Name = "Fern", Type = "tropical", WateringFrequencyDays = 5, LastWateredDate = DateTime.Now, Location = "Living Room" };
            
            context.Plants.Add(newplant);
            context.SaveChanges();
            context.Entry(newplant).State = EntityState.Detached;

            var controller = new PlantsCareController(context);

            var updatedPlant = new Plant
            {
                Id = id,
                Name = "Updated Fern",
                Type = "tropical",
                WateringFrequencyDays = 6,
                LastWateredDate = DateTime.Now.AddDays(-1),
                Location = "Bedroom"
            };

            // Act
            var result = controller.Update(id, updatedPlant);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var plant = context.Plants.Find(id);
            Assert.Equal("Updated Fern", plant?.Name);
        }


        //TestCase para el api update plant care
        //Escenario caso 2, se registran datos para plant care con id 41ad872b-0e57-4859-8ad4-d252f6f551c5
        //y se intenta actualizan datos con un diferente id 41ad872b-0e57-4859-8ad4-d252f6f551c8
        [Fact]
        public void TestApi_Update_Case_2_No_Ok()
        {
            var id = "41ad872b-0e57-4859-8ad4-d252f6f551c5";
            // Arrange
            var context = GetInMemoryDbContext();

            var newplant = new Plant { Id = id, Name = "Fern", Type = "tropical", WateringFrequencyDays = 5, LastWateredDate = DateTime.Now, Location = "Living Room" };

            context.Plants.Add(newplant);
            context.SaveChanges();
            context.Entry(newplant).State = EntityState.Detached;

            var controller = new PlantsCareController(context);

            var updatedPlant = new Plant
            {
                Id = "41ad872b-0e57-4859-8ad4-d252f6f551c8",
                Name = "Updated Fern",
                Type = "tropical",
                WateringFrequencyDays = 6,
                LastWateredDate = DateTime.Now.AddDays(-1),
                Location = "Bedroom"
            };

            // Act
            var result = controller.Update("41ad872b-0e57-4859-8ad4-d252f6f551c8", updatedPlant);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            var plant = context.Plants.Find("41ad872b-0e57-4859-8ad4-d252f6f551c8");
            Assert.NotEqual("Updated Fern", plant?.Name);
        }

        //TestCase para validar la frecuencia de riego
        //Escenario caso 1, se registran rose con frecuencia de riego 1 dia, se espera frecuencia de riego 1 dia
        [Fact]
        public void WateringFrequencyValidation_Case_1()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new PlantsCareController(context);

            var invalidPlant = new Plant
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Rose",
                Type = "tropical",
                WateringFrequencyDays = 1,
                LastWateredDate = DateTime.Now,
                Location = "Garden"
            };

            // Act
            var result = controller.Create(invalidPlant);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Single(context.Plants);
            Assert.Equal(1, context.Plants.First().WateringFrequencyDays);
        }

        //TestCase para validar la frecuencia de riego
        //Escenario caso 2, se registran rose con frecuencia de riego 1 dia, se espera frecuencia de riego 3 dias
        [Fact]
        public void WateringFrequencyValidation_Case_2_No_Ok()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new PlantsCareController(context);

            var invalidPlant = new Plant
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Rose",
                Type = "tropical",
                WateringFrequencyDays = 1,
                LastWateredDate = DateTime.Now,
                Location = "Garden"
            };

            // Act
            var result = controller.Create(invalidPlant);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Single(context.Plants);
            Assert.NotEqual(3, context.Plants.First().WateringFrequencyDays);
        }

        //TestCase para calcular la fecha de vencimiento del riego
        //Escenario caso 1, se tiene una planta con una frecuencia de riego de 5 dias y su ultimo riego fue hace 3 dias de hoy, se espera que este
        //que la fecha de vencimiento aun no sea mayor que hoy
        [Fact]
        public void DueDateCalculation_Case_1()
        {
            var plant = new Plant
            {
                Id = "1",
                Name = "Lavender",
                Type = "herb",
                WateringFrequencyDays = 5,
                LastWateredDate = DateTime.Now.AddDays(-3),
                Location = "Window"
            };

            var nextWateringDate = plant.LastWateredDate.Value.AddDays(plant.WateringFrequencyDays.Value);

            // Assert
            Assert.Equal(DateTime.Now.AddDays(2).Date, nextWateringDate.Date);
        }

        //TestCase para calcular la fecha de vencimiento del riego
        //Escenario caso 2, se tiene una planta con una frecuencia de riego de 5 dias y su ultimo riego fue hace 4 dias de hoy, se espera que este
        //que la fecha de vencimiento ya se cumplio
        [Fact]
        public void DueDateCalculation_Case_2_No_Ok()
        {
            var plant = new Plant
            {
                Id = "1",
                Name = "Lavender",
                Type = "herb",
                WateringFrequencyDays = 5,
                LastWateredDate = DateTime.Now.AddDays(-4),
                Location = "Window"
            };

            var nextWateringDate = plant.LastWateredDate.Value.AddDays(plant.WateringFrequencyDays.Value);

            // Assert
            Assert.NotEqual(DateTime.Now.AddDays(2).Date, nextWateringDate.Date);
        }
    }
}