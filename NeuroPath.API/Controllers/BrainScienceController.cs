using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.BrainScience;

namespace NeuroPath.API.Controllers;

[Route("api/brain-areas")]
public class BrainAreasController : ApiControllerBase
{
    private readonly IBrainAreaService _service;

    public BrainAreasController(IBrainAreaService service) => _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<BrainAreaDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<BrainAreaDetailDto>> GetById(int id) => OkOrNotFound(await _service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<BrainAreaDto>> Create(CreateBrainAreaDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<BrainAreaDto>> Update(int id, CreateBrainAreaDto dto) => OkOrNotFound(await _service.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _service.DeleteAsync(id));
}

[Route("api/brain-regions")]
public class BrainRegionsController : ApiControllerBase
{
    private readonly IBrainRegionService _service;

    public BrainRegionsController(IBrainRegionService service) => _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<BrainRegionDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<BrainRegionDto>> GetById(int id) => OkOrNotFound(await _service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<BrainRegionDto>> Create(CreateBrainRegionDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }
}

[Route("api/neurotransmitters")]
[AllowAnonymous]
public class NeurotransmittersController : ApiControllerBase
{
    private readonly INeurotransmitterService _service;

    public NeurotransmittersController(INeurotransmitterService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<NeurotransmitterDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NeurotransmitterDto>> GetById(int id) => OkOrNotFound(await _service.GetByIdAsync(id));
}

[Route("api/cognitive-skills")]
[AllowAnonymous]
public class CognitiveSkillsController : ApiControllerBase
{
    private readonly ICognitiveSkillService _service;

    public CognitiveSkillsController(ICognitiveSkillService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<CognitiveSkillDto>>> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CognitiveSkillDto>> GetById(int id) => OkOrNotFound(await _service.GetByIdAsync(id));
}
