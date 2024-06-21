using System.ComponentModel;

namespace ICorteApi.Enums;

public enum StreetType
{
    [Description("Alameda")]    AL = 1,
    [Description("Avenida")]    AV,
    [Description("Beco")]       BC,
    [Description("Caminho")]    CAM,
    [Description("Chácara")]    CH,
    [Description("Estrada")]    EST,
    [Description("Estância")]   ETN,
    [Description("Fazenda")]    FAZ,
    [Description("Paralela")]   PAR,
    [Description("Rua")]        R,
    [Description("Rodovia")]    ROD,
    [Description("Sitio")]      SIT,
    [Description("Travessa")]   TV
}