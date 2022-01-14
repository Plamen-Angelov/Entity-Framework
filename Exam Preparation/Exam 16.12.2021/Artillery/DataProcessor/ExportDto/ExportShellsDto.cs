using System.Collections.Generic;

namespace Artillery.DataProcessor.ExportDto
{
    public class ExportShellsDto
    {
        public double ShellWeight { get; set; }

        public string Caliber { get; set; }

        public List<ExportGunDto> Guns { get; set; }
    }
}
