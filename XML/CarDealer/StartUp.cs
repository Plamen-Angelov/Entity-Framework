﻿using AutoMapper;
using CarDealer.Data;
using CarDealer.Dto.Input;
using CarDealer.Dto.Output;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            ////Problem 9
            //string inputSupliers = File.ReadAllText("datasets/suppliers.xml");
            //System.Console.WriteLine(ImportSuppliers(context, inputSupliers));
            ////Problem 10
            //string inputParts = File.ReadAllText("datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, inputParts));
            ////Problem 11
            //string inputCars = File.ReadAllText("datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, inputCars));
            ////Problem 12
            //string inputCustomers = File.ReadAllText("datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, inputCustomers));
            ////Problem 13
            //string inputSales = File.ReadAllText("datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, inputSales));
            //Problem 14
            Console.WriteLine(GetCarsWithDistance(context));
        }

        //Problem 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            CarsWithDistanceOutoutDto[] cars = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .Select(c => new CarsWithDistanceOutoutDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(CarsWithDistanceOutoutDto[]), new XmlRootAttribute("cars"));
            using StringWriter writer = new StringWriter(sb);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            serializer.Serialize(writer, cars, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SaleInputDto[]), new XmlRootAttribute("Sales"));
            using StringReader reader = new StringReader(inputXml);

            SaleInputDto[] dtos = (SaleInputDto[])serializer.Deserialize(reader);

            List<Sale> sales = new List<Sale>();

            foreach (var dto in dtos)
            {
                if (!context.Cars.Any(c => c.Id == dto.CarId))
                {
                    continue;
                }

                Sale sale = new Sale()
                {
                    CarId = dto.CarId,
                    CustomerId = dto.CustomerId,
                    Discount = dto.Discount
                };

                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count()}";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerInputDto[]), new XmlRootAttribute("Customers"));

            using StringReader reader = new StringReader(inputXml);

            CustomerInputDto[] dtos = (CustomerInputDto[])serializer.Deserialize(reader);

            MapperConfiguration configuration = new MapperConfiguration(c => c.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(configuration);

            Customer[] customers = mapper.Map<Customer[]>(dtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count()}";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CarInputDto[]), new XmlRootAttribute("Cars"));

            using StringReader reader = new StringReader(inputXml);
            CarInputDto[] dtos = (CarInputDto[])serializer.Deserialize(reader);

            List<Car> cars = new List<Car>();

            foreach (var dto in dtos)
            {
                Car car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TravelledDistance = dto.TravelledDistance,
                    PartCars = new List<PartCar>()
                };

                foreach (var partId in dto.PartIds.Select(p => p.Id).Distinct())
                {
                    if (context.Parts.Any(p => p.Id == partId))
                    {
                        PartCar partCar = new PartCar()
                        {
                            Car = car,
                            PartId = partId
                        };
                        
                        car.PartCars.Add(partCar);
                    }
                }
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Parts");
            XmlSerializer serializer = new XmlSerializer(typeof(PartInputDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            PartInputDto[] dtos = (PartInputDto[])serializer.Deserialize(reader);

            List<Part> parts = new List<Part>();

            foreach (var dto in dtos)
            {
                if (context.Suppliers.Any(s => s.Id == dto.SupplierId))
                {
                    Part part = new Part()
                    {
                        Name = dto.Name,
                        Price = dto.Price,
                        Quantity = dto.Quantity,
                        SupplierId = dto.SupplierId
                    };

                    parts.Add(part);
                }
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count()}";
        }

        //Problem 9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Suppliers");
            XmlSerializer serializer = new XmlSerializer(typeof(SuppliersInputDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            SuppliersInputDto[] dtos = (SuppliersInputDto[])serializer.Deserialize(reader);
            
            MapperConfiguration configuration = new MapperConfiguration(cfg => cfg.AddProfile<CarDealerProfile>());
            IMapper mapper = new Mapper(configuration);

            Supplier[] suppliers = mapper.Map<Supplier[]>(dtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}";
        }
    }
}