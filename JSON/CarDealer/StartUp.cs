using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Input;
using CarDealer.DTO.Output;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            ////Task 8
            //string input = File.ReadAllText("datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, input));
            ////Task 9
            //string inputParts = File.ReadAllText("datasets/parts.json");
            //Console.WriteLine(ImportParts(context, inputParts));
            ////Task 10
            //string inputCars = File.ReadAllText("datasets/cars.json");
            //Console.WriteLine(ImportCars(context, inputCars));
            ////Task 11
            //string inputCustomers = File.ReadAllText("datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, inputCustomers));
            ////Task 12
            //string salesInput = File.ReadAllText("datasets/sales.json");
            //Console.WriteLine(ImportSales(context, salesInput));
            ////Task 13
            //Console.WriteLine(GetOrderedCustomers(context));
            ////Task 14
            //Console.WriteLine(GetCarsFromMakeToyota(context));
            ////Task 15
            //Console.WriteLine(GetLocalSuppliers(context));
            ////Task 16
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            ////Task 17
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            //Task 18
            Console.WriteLine(GetSalesWithAppliedDiscount(context));

        }

        //Task 18
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            SaleOutputDto[] sales = context
                .Sales
                .Select(s => new SaleOutputDto
                {
                    Car = new CarOutputDto
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount.ToString("F2"),
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("F2"),
                    PriceWithDiscount = ((s.Car.PartCars.Sum(pc => pc.Part.Price)) * (100 - s.Discount) / 100).ToString("F2")
                })
                .Take(10)
                .ToArray();

            //JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            //{
            //    Formatting = Formatting.Indented
            //};

            string json = JsonConvert.SerializeObject(sales);
            return json;
        }

        //Task 17
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            CustomersAndSalesOutputDto[] customers = context
                .Customers
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new CustomersAndSalesOutputDto()
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SpentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToArray();

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string json = JsonConvert.SerializeObject(customers, settings);
            return json;

        }

        //Task 16
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            CarAndPartsOutputDto[] carsAndParts = context
                .Cars
                .Select(c => new CarAndPartsOutputDto
                {
                    Car = new CarOutputDto()
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TravelledDistance = c.TravelledDistance
                    },

                    Parts = c.PartCars
                    .Select(p => new PartOutputDto
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price.ToString("F2")
                    })
                    .ToList()
                })
                .ToArray();

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                //ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string json = JsonConvert.SerializeObject(carsAndParts, settings);
            return json;
        }

        //Task 15
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            LocalSuppliersOutputDto[] localSuppliers = context
                .Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new LocalSuppliersOutputDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToArray();

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            string json = JsonConvert.SerializeObject(localSuppliers, serializerSettings);

            return json;
        }

        //Task 14
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyoytaCars = context
                .Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new CarsToyoytaOutputDto
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .ToArray();

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            };
            string json = JsonConvert.SerializeObject(toyoytaCars, settings);

            return json;
        }

        //Task 13
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            CustomerOutputDto[] customers = context
                .Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new CustomerOutputDto
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToArray();

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };

            string json = JsonConvert.SerializeObject(customers, serializerSettings);
            return json;
        }

        //Task 12
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            ICollection<SaleInputDto> salesDtos = JsonConvert.DeserializeObject<ICollection<SaleInputDto>>(inputJson);

            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(configuration);

            ICollection<Sale> sales = mapper.Map<ICollection<Sale>>(salesDtos);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        //Task 11
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            ICollection<CustomerInputDto> customerDtos = JsonConvert.DeserializeObject<ICollection<CustomerInputDto>>(inputJson);

            ICollection<Customer> customers = new List<Customer>();

            foreach (CustomerInputDto customerDto in customerDtos)
            {
                Customer customer = new Customer()
                {
                    Name = customerDto.Name,
                    BirthDate = DateTime.Parse(customerDto.BirthDate),
                    IsYoungDriver = bool.Parse(customerDto.IsYoungDriver)
                };

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        //Task 10
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IEnumerable<CarsInputDto> carDtos = JsonConvert.DeserializeObject<IEnumerable<CarsInputDto>>(inputJson);

            List<Car> cars = new List<Car>();

            foreach (var carDto in carDtos)
            {
                Car car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                foreach (var partId in carDto.PartsId.Distinct())
                {
                    PartCar partCar = new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    };

                    car.PartCars.Add(partCar);
                }

                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //Task 9
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            List<PartsInputDto> partsDtos = JsonConvert.DeserializeObject<List<PartsInputDto>>(inputJson);

            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(configuration);

            List<Part> parts = mapper.Map<List<Part>>(partsDtos);
            
            int[] supplierIds = context
                .Suppliers.Select(x => x.Id)
                .ToArray();

            parts = parts
                .Where(x => supplierIds.Contains(x.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        //Task8
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            List<SupplierInputDto> suppliersDtos = JsonConvert.DeserializeObject<List<SupplierInputDto>>(inputJson);

            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(configuration);

            List<Supplier> suppliers = mapper.Map<List<Supplier>>(suppliersDtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }
    }
}