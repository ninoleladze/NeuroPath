using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Social;

namespace NeuroPath.API.Controllers;

[Route("api/friends")]
[Authorize]
public class FriendsController : ApiControllerBase
{
    private readonly IFriendshipService _friends;

    public FriendsController(IFriendshipService friends) => _friends = friends;

    [HttpGet]
    public async Task<ActionResult<List<FriendshipDto>>> GetFriends() => Ok(await _friends.GetFriendsAsync(CurrentUserId));

    [HttpGet("requests")]
    public async Task<ActionResult<List<FriendRequestDto>>> GetPending() => Ok(await _friends.GetPendingRequestsAsync(CurrentUserId));

    [HttpPost("requests")]
    public async Task<IActionResult> SendRequest(SendFriendRequestDto dto)
        => await _friends.SendRequestAsync(dto, CurrentUserId) ? NoContent() : Conflict(new ProblemDetails { Title = "A request is already pending." });

    [HttpPost("requests/{requestId:int}/accept")]
    public async Task<IActionResult> Accept(int requestId) => NoContentOrNotFound(await _friends.AcceptRequestAsync(requestId, CurrentUserId));

    [HttpPost("requests/{requestId:int}/reject")]
    public async Task<IActionResult> Reject(int requestId) => NoContentOrNotFound(await _friends.RejectRequestAsync(requestId, CurrentUserId));

    [HttpDelete("{friendId}")]
    public async Task<IActionResult> Remove(string friendId) => NoContentOrNotFound(await _friends.RemoveFriendAsync(CurrentUserId, friendId));
}

[Route("api/messages")]
[Authorize]
public class MessagesController : ApiControllerBase
{
    private readonly IDirectMessageService _messages;

    public MessagesController(IDirectMessageService messages) => _messages = messages;

    [HttpGet("conversations")]
    public async Task<ActionResult<List<DirectMessageDto>>> GetConversations([FromQuery] int limit = 20)
        => Ok(await _messages.GetRecentConversationsAsync(CurrentUserId, limit));

    [HttpGet("conversations/{otherUserId}")]
    public async Task<ActionResult<List<DirectMessageDto>>> GetConversation(string otherUserId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        => Ok(await _messages.GetConversationAsync(CurrentUserId, otherUserId, page, pageSize));

    [HttpPost]
    public async Task<ActionResult<DirectMessageDto>> Send(SendMessageDto dto) => Ok(await _messages.SendAsync(dto, CurrentUserId));

    [HttpPost("{id:int}/read")]
    public async Task<IActionResult> MarkRead(int id) => NoContentOrNotFound(await _messages.MarkReadAsync(id, CurrentUserId));
}

[Route("api/teams")]
public class TeamsController : ApiControllerBase
{
    private readonly ITeamService _teams;

    public TeamsController(ITeamService teams) => _teams = teams;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<TeamDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _teams.GetAllAsync(page, pageSize));

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<TeamDto>> GetById(int id) => OkOrNotFound(await _teams.GetByIdAsync(id));

    [HttpGet("{id:int}/members")]
    [AllowAnonymous]
    public async Task<ActionResult<List<TeamMemberDto>>> GetMembers(int id) => Ok(await _teams.GetMembersAsync(id));

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TeamDto>> Create(CreateTeamDto dto)
    {
        var created = await _teams.CreateAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult<TeamDto>> Update(int id, CreateTeamDto dto)
        => OkOrNotFound(await _teams.UpdateAsync(id, dto, CurrentUserId));

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> Archive(int id) => NoContentOrNotFound(await _teams.ArchiveAsync(id, CurrentUserId, IsAdmin));

    [HttpDelete("{id:int}/members/{userId}")]
    [Authorize]
    public async Task<IActionResult> RemoveMember(int id, string userId)
        => NoContentOrNotFound(await _teams.RemoveMemberAsync(id, userId, CurrentUserId));

    [HttpPost("{id:int}/join")]
    [Authorize]
    public async Task<IActionResult> Join(int id)
        => await _teams.JoinAsync(id, CurrentUserId) ? NoContent() : Conflict(new ProblemDetails { Title = "You are already a member." });

    [HttpPost("{id:int}/leave")]
    [Authorize]
    public async Task<IActionResult> Leave(int id) => NoContentOrNotFound(await _teams.LeaveAsync(id, CurrentUserId));

    [HttpPut("{id:int}/members/{userId}/role")]
    [Authorize]
    public async Task<IActionResult> SetRole(int id, string userId, SetTeamRoleDto dto)
        => NoContentOrNotFound(await _teams.SetMemberRoleAsync(id, userId, dto.Role, CurrentUserId));
}
