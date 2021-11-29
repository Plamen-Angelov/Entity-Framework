using System.Collections.Generic;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class EmployeeExportDto
    {
        public string Username { get; set; }

        public List<TaskExportDto> Tasks { get; set; }
    }
}
