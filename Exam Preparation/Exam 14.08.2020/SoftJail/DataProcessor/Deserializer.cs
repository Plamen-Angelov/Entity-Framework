namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Common;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            DepartmentImportDto[] departmentDtos = JsonConvert.DeserializeObject<DepartmentImportDto[]>(jsonString);
            List<Department> departments = new List<Department>();
            StringBuilder sb = new StringBuilder();
            
            foreach (var dto in departmentDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                Department department = new Department()
                {
                    Name = dto.Name
                };

                List<Cell> cells = new List<Cell>();
                bool hasInvalidCell = false;

                foreach (var cellDto in dto.Cells)
                {
                    if (!IsValid(cellDto))
                    {
                        hasInvalidCell = true;
                        break;
                    }

                    Cell cell = new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    };

                    cells.Add(cell);
                }

                if (hasInvalidCell)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                if (!cells.Any())
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                department.Cells = cells;
                departments.Add(department);
                sb.AppendLine($"Imported {department.Name} with {cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            PrisonerImportDto[] prisonerDtos = JsonConvert.DeserializeObject<PrisonerImportDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();
            List<Prisoner> prisoners = new List<Prisoner>();

            foreach (var dto in prisonerDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                bool incancerationDateIsValid = DateTime.TryParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime incancerationDate);

                if (!incancerationDateIsValid)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                bool releaseDateIsValid = DateTime.TryParseExact(dto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime realeaseDate);

                if (dto.ReleaseDate!= null && !releaseDateIsValid)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                Prisoner prisoner = new Prisoner()
                {
                    FullName = dto.FullName,
                    Nickname = dto.Nickname,
                    Age = dto.Age,
                    IncarcerationDate = incancerationDate,
                    ReleaseDate = realeaseDate,
                    Bail = dto.Bail,
                    CellId = dto.CellId
                };

                List<Mail> mails = new List<Mail>();
                bool hasInvalidMail = false;

                foreach (var mailDto in dto.Mails)
                {
                    if (!IsValid(mailDto))
                    {
                        hasInvalidMail = true;
                        break;
                    }

                    Mail mail = new Mail()
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    };
                    mails.Add(mail);
                }

                if (hasInvalidMail)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                prisoner.Mails = mails;
                prisoners.Add(prisoner);
                sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
            }
            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(OfficerImportDto[]), new XmlRootAttribute("Officers"));

            using StringReader reader = new StringReader(xmlString);
            OfficerImportDto[] officerDtos = (OfficerImportDto[])serializer.Deserialize(reader);
            StringBuilder sb = new StringBuilder();
            List<Officer> officers = new List<Officer>();

            foreach (var dto in officerDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                bool isValidPosition = Enum.TryParse(typeof(Position), dto.Position, out var position);

                if (!isValidPosition)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                bool isValidWeapon = Enum.TryParse(typeof(Weapon), dto.Weapon, out var weapon);

                if (!isValidWeapon)
                {
                    sb.AppendLine(GlobalConstants.Error_Message);
                    continue;
                }

                Officer officer = new Officer()
                {
                    FullName = dto.FullName,
                    Salary = dto.Salary,
                    Position = (Position)position,
                    Weapon = (Weapon)weapon,
                    DepartmentId = dto.DepartmentId
                };

                List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();

                foreach (var prisonerDto in dto.Prisoners)
                {
                    OfficerPrisoner officerPrisoner = new OfficerPrisoner()
                    {
                        PrisonerId = int.Parse(prisonerDto.Id),
                        Officer = officer
                    };

                    officerPrisoners.Add(officerPrisoner);
                }

                officer.OfficerPrisoners = officerPrisoners;
                officers.Add(officer);
                sb.AppendLine($"Imported {officer.FullName} ({officerPrisoners.Count} prisoners)");
            }
            context.Officers.AddRange(officers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}