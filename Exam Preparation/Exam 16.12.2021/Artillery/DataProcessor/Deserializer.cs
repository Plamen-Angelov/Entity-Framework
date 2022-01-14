namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportCountryDto[]), new XmlRootAttribute("Countries"));
            using StringReader sr = new StringReader(xmlString);

            ImportCountryDto[] dtos = (ImportCountryDto[])serializer.Deserialize(sr);

            StringBuilder sb = new StringBuilder();
            List<Country> countries = new List<Country>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = dto.CountryName,
                    ArmySize = dto.ArmySize
                };

                countries.Add(country);
                sb.AppendLine(string.Format(SuccessfulImportCountry, dto.CountryName, dto.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportManufactorerDto[]), new XmlRootAttribute("Manufacturers"));
            StringReader sr = new StringReader(xmlString);
            ImportManufactorerDto[] dtos = (ImportManufactorerDto[])serializer.Deserialize(sr);

            List<Manufacturer> manufacturers = new List<Manufacturer>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (manufacturers.Any(m => m.ManufacturerName == dto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = dto.ManufacturerName,
                    Founded = dto.Founded
                };

                string[] foundedParts = dto.Founded
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries);
                string townAndCountry = $"{foundedParts[foundedParts.Length - 2]}, {foundedParts[foundedParts.Length - 1]}";

                manufacturers.Add(manufacturer);
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, dto.ManufacturerName, townAndCountry));
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportShellDto[]), new XmlRootAttribute("Shells"));
            StringReader sr = new StringReader(xmlString);

            ImportShellDto[] dtos = (ImportShellDto[])serializer.Deserialize(sr);
            List<Shell> shells = new List<Shell>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = dto.ShellWeight,
                    Caliber = dto.Caliber
                };

                shells.Add(shell);
                sb.AppendLine(string.Format(SuccessfulImportShell, dto.Caliber, dto.ShellWeight));
            }

            context.AddRange(shells);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            ImportGunDto[] dtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);
            List<Gun> guns = new List<Gun>();
            StringBuilder sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isGunTypeValid = Enum.TryParse(typeof(GunType), dto.GunType, out object gunType);

                if (!isGunTypeValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun()
                {
                    ManufacturerId = dto.ManufacturerId,
                    GunWeight = dto.GunWeight,
                    BarrelLength = dto.BarrelLength,
                    NumberBuild = dto.NumberBuild,
                    Range = dto.Range,
                    GunType = (GunType)gunType,
                    ShellId = dto.ShellId
                };

                List<CountryGun> countiesGuns = new List<CountryGun>();

                foreach (var countryId in dto.Countries)
                {
                    CountryGun countryGun = new CountryGun()
                    {
                        Gun = gun,
                        CountryId = countryId.Id
                    };
                    countiesGuns.Add(countryGun);
                }

                gun.CountriesGuns = countiesGuns;

                guns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, dto.GunType, dto.GunWeight, dto.BarrelLength));
            }

            context.AddRange(guns);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
