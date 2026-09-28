namespace NeuroPath.Core.Models.DTOs.BrainScience;

public class BrainAreaDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? IconUrl { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; }
}

public class BrainAreaDetailDto : BrainAreaDto
{
    public string? ScientificDescription { get; set; }
    public List<NeurotransmitterDto> Neurotransmitters { get; set; } = new();
    public List<CognitiveSkillDto> CognitiveSkills { get; set; } = new();
}

public class CreateBrainAreaDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ScientificDescription { get; set; }
    public string? IconUrl { get; set; }
    public string? Color { get; set; }
    public int DisplayOrder { get; set; }
}

public class BrainRegionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LatinName { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Functions { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public class CreateBrainRegionDto
{
    public string Name { get; set; } = string.Empty;
    public string? LatinName { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Functions { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public class NeurotransmitterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Function { get; set; } = string.Empty;
    public string? ChemicalFormula { get; set; }
    public string? BoostTips { get; set; }
    public string? ImageUrl { get; set; }
}

public class CognitiveSkillDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? MeasurementUnit { get; set; }
}
