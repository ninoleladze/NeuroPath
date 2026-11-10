using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Core.Models.DTOs.Progress;

namespace NeuroPath.API.Controllers;

[Route("api/achievements")]
public class AchievementsController : ApiControllerBase
{
    private readonly IAchievementService _achievements;

    public AchievementsController(IAchievementService achievements) => _achievements = achievements;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<AchievementDto>>> GetAll() => Ok(await _achievements.GetAllAsync(includeSecret: IsAdmin));

    [HttpGet("mine")]
    [Authorize]
    public async Task<ActionResult<List<UserAchievementDto>>> GetMine() => Ok(await _achievements.GetUserAchievementsAsync(CurrentUserId));

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<AchievementDto>> Create(CreateAchievementDto dto) => Ok(await _achievements.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<AchievementDto>> Update(int id, CreateAchievementDto dto) => OkOrNotFound(await _achievements.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _achievements.DeleteAsync(id));
}

[Route("api/leaderboard")]
public class LeaderboardController : ApiControllerBase
{
    private readonly ILeaderboardService _leaderboard;

    public LeaderboardController(ILeaderboardService leaderboard) => _leaderboard = leaderboard;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<LeaderboardEntryDto>>> Get([FromQuery] string period = "weekly")
        => Ok(await _leaderboard.GetLeaderboardAsync(period));

    [HttpPost("refresh")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Refresh([FromQuery] string period = "weekly")
    {
        await _leaderboard.RefreshLeaderboardAsync(period);
        return NoContent();
    }
}

[Route("api/shop")]
[Authorize]
public class ShopController : ApiControllerBase
{
    private readonly IShopService _shop;

    public ShopController(IShopService shop) => _shop = shop;

    [HttpGet("items")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ShopItemDto>>> GetItems() => Ok(await _shop.GetItemsAsync());

    [HttpGet("items/all")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<List<ShopItemDto>>> GetAllItems() => Ok(await _shop.GetAllItemsAsync());

    [HttpPost("items")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<ShopItemDto>> CreateItem(SaveShopItemDto dto) => Ok(await _shop.CreateItemAsync(dto));

    [HttpPut("items/{itemId:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<ShopItemDto>> UpdateItem(int itemId, SaveShopItemDto dto) => OkOrNotFound(await _shop.UpdateItemAsync(itemId, dto));

    [HttpPost("items/{itemId:int}/purchase")]
    public async Task<ActionResult<UserInventoryDto>> Purchase(int itemId) => Ok(await _shop.PurchaseAsync(itemId, CurrentUserId));

    [HttpGet("inventory")]
    public async Task<ActionResult<List<UserInventoryDto>>> GetInventory() => Ok(await _shop.GetInventoryAsync(CurrentUserId));

    [HttpPut("inventory/{inventoryId:int}/equipped")]
    public async Task<IActionResult> SetEquipped(int inventoryId, SetEquippedDto dto)
        => NoContentOrNotFound(await _shop.SetEquippedAsync(inventoryId, dto.IsEquipped, CurrentUserId));

    [HttpGet("streak")]
    public async Task<ActionResult<StreakDto>> GetStreak() => OkOrNotFound(await _shop.GetStreakAsync(CurrentUserId));
}

[Route("api/progress")]
[Authorize]
public class ProgressController : ApiControllerBase
{
    private readonly IProgressSnapshotService _progress;

    public ProgressController(IProgressSnapshotService progress) => _progress = progress;

    [HttpGet("summary")]
    public async Task<ActionResult<ProgressSummaryDto>> GetSummary() => Ok(await _progress.GetSummaryAsync(CurrentUserId));

    [HttpGet("brain-areas")]
    public async Task<ActionResult<List<BrainAreaBreakdownDto>>> GetBrainAreas() => Ok(await _progress.GetBrainAreaBreakdownAsync(CurrentUserId));

    [HttpGet("snapshots")]
    public async Task<ActionResult<List<ProgressSnapshotDto>>> GetSnapshots([FromQuery] int page = 1, [FromQuery] int pageSize = 30)
        => Ok(await _progress.GetSnapshotsAsync(CurrentUserId, page, pageSize));

    [HttpPost("snapshots")]
    public async Task<IActionResult> TakeSnapshot()
    {
        await _progress.TakeSnapshotAsync(CurrentUserId);
        return NoContent();
    }
}

[Route("api/challenges")]
public class ChallengesController : ApiControllerBase
{
    private readonly IWeeklyChallengeService _challenges;

    public ChallengesController(IWeeklyChallengeService challenges) => _challenges = challenges;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<WeeklyChallengeDto>>> GetAll() => Ok(await _challenges.GetAllAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<WeeklyChallengeDto>> GetById(int id) => OkOrNotFound(await _challenges.GetByIdAsync(id));

    [HttpGet("{id:int}/leaderboard")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ChallengeLeaderboardEntryDto>>> GetLeaderboard(int id) => Ok(await _challenges.GetLeaderboardAsync(id));

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<WeeklyChallengeDto>> Create(CreateWeeklyChallengeDto dto)
    {
        var created = await _challenges.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<WeeklyChallengeDto>> Update(int id, CreateWeeklyChallengeDto dto) => OkOrNotFound(await _challenges.UpdateAsync(id, dto));

    [HttpPut("{id:int}/active")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetActive(int id, SetActiveDto dto) => NoContentOrNotFound(await _challenges.SetActiveAsync(id, dto.IsActive));

    [HttpPost("{id:int}/join")]
    [Authorize]
    public async Task<IActionResult> Join(int id)
        => await _challenges.JoinAsync(id, CurrentUserId) ? NoContent() : Conflict(new ProblemDetails { Title = "Already joined." });

    [HttpPost("{id:int}/teams/{teamId:int}/join")]
    [Authorize]
    public async Task<IActionResult> JoinAsTeam(int id, int teamId)
        => await _challenges.JoinAsTeamAsync(id, teamId, CurrentUserId) ? NoContent() : Conflict(new ProblemDetails { Title = "Team already joined." });
}
