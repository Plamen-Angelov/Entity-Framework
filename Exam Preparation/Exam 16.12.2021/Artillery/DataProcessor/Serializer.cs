
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            ExportShellsDto[] shells = context
                .Shells
                .Where(s => s.ShellWeight > shellWeight)
                .Select(s => new ExportShellsDto
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                    .Where(g => g.GunType == GunType.AntiAircraftGun)
                    .Select(g => new ExportGunDto
                    {
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    })
                    .OrderByDescending(g => g.GunWeight)
                    .ToList()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            string json = JsonConvert.SerializeObject(shells, Formatting.Indented);
            return json;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            ExportGunCountriesDto[] guns = context
                .Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .OrderBy(g => g.BarrelLength)
                .Select(g => new ExportGunCountriesDto
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight.ToString(),
                    BarrelLength = g.BarrelLength.ToString(),
                    Range = g.Range.ToString(),
                    Countries = g.CountriesGuns
                    .Where(c => c.Country.ArmySize > 4500000)
                    .Select(c => new ExportCountryDto
                    {
                        CountryName = c.Country.CountryName,
                        ArmySize = c.Country.ArmySize.ToString()
                    })
                    .OrderBy(c => c.ArmySize)
                    .ToArray()
                })
                .ToArray();

            XmlSerializer serializer = new XmlSerializer(typeof(ExportGunCountriesDto[]), new XmlRootAttribute("Guns"));
            StringBuilder sb = new StringBuilder();

            StringWriter sw = new StringWriter(sb);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            serializer.Serialize(sw, guns, ns);

            return sb.ToString().TrimEnd();
        }
    }
}
