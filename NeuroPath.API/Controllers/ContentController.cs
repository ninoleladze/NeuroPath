using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroPath.Core.Common;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Admin;
using NeuroPath.Core.Models.DTOs.Content;

namespace NeuroPath.API.Controllers;

[Route("api/articles")]
public class ArticlesController : ApiControllerBase
{
    private readonly IArticleService _articles;
    private readonly ICommentService _comments;

    public ArticlesController(IArticleService articles, ICommentService comments)
    {
        _articles = articles;
        _comments = comments;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ArticleDto>>> GetAll([FromQuery] bool includeDrafts = false, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _articles.GetAllAsync(publishedOnly: !(includeDrafts && IsContentManager), page, pageSize));

    [HttpGet("featured")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ArticleDto>>> GetFeatured() => Ok(await _articles.GetFeaturedAsync());

    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ArticleCategoryDto>>> GetCategories() => Ok(await _articles.GetCategoriesAsync());

    [HttpGet("categories/{categoryId:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<ArticleDto>>> GetByCategory(int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _articles.GetByCategoryAsync(categoryId, page, pageSize));

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ArticleDetailDto>> GetById(int id)
    {
        var article = await _articles.GetByIdAsync(id, includeUnpublished: IsContentManager);
        if (article == null) return NotFound();
        if (article.IsPublished) await _articles.IncrementViewCountAsync(id);
        return Ok(article);
    }

    [HttpPost]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<ArticleDetailDto>> Create(CreateArticleDto dto)
    {
        var created = await _articles.CreateAsync(dto, CurrentUserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<ArticleDetailDto>> Update(int id, CreateArticleDto dto)
        => OkOrNotFound(await _articles.UpdateAsync(id, dto, CurrentUserId, IsAdmin));

    [HttpPost("{id:int}/publish")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<IActionResult> Publish(int id) => NoContentOrNotFound(await _articles.PublishAsync(id, CurrentUserId, IsAdmin));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _articles.DeleteAsync(id, CurrentUserId, IsAdmin));

    [HttpGet("{id:int}/comments")]
    [AllowAnonymous]
    public async Task<ActionResult<List<CommentDto>>> GetComments(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _comments.GetByArticleAsync(id, page, pageSize));
}

[Route("api/comments")]
[Authorize]
public class CommentsController : ApiControllerBase
{
    private readonly ICommentService _comments;

    public CommentsController(ICommentService comments) => _comments = comments;

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(CreateCommentDto dto) => Ok(await _comments.CreateAsync(dto, CurrentUserId));

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CommentDto>> Update(int id, UpdateCommentDto dto)
        => OkOrNotFound(await _comments.UpdateAsync(id, dto.Content, CurrentUserId));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _comments.DeleteAsync(id, CurrentUserId, IsAdmin));

    [HttpPost("{id:int}/like")]
    public async Task<IActionResult> Like(int id)
        => await _comments.LikeAsync(id, CurrentUserId) ? NoContent() : Conflict(new ProblemDetails { Title = "Already liked." });

    [HttpDelete("{id:int}/like")]
    public async Task<IActionResult> Unlike(int id) => NoContentOrNotFound(await _comments.UnlikeAsync(id, CurrentUserId));
}

[Route("api/bookmarks")]
[Authorize]
public class BookmarksController : ApiControllerBase
{
    private readonly IBookmarkService _bookmarks;

    public BookmarksController(IBookmarkService bookmarks) => _bookmarks = bookmarks;

    [HttpGet]
    public async Task<ActionResult<List<BookmarkDto>>> GetMine([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _bookmarks.GetUserBookmarksAsync(CurrentUserId, page, pageSize));

    [HttpPost]
    public async Task<ActionResult<BookmarkDto>> Create(CreateBookmarkDto dto) => Ok(await _bookmarks.CreateAsync(dto, CurrentUserId));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _bookmarks.DeleteAsync(id, CurrentUserId));
}

[Route("api/daily-tips")]
public class DailyTipsController : ApiControllerBase
{
    private readonly IDailyTipService _tips;

    public DailyTipsController(IDailyTipService tips) => _tips = tips;

    [HttpGet("today")]
    [AllowAnonymous]
    public async Task<ActionResult<DailyTipDto>> GetToday() => OkOrNotFound(await _tips.GetTodayAsync());

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<List<DailyTipAdminDto>>> GetAll() => Ok(await _tips.GetAllAsync());

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<DailyTipAdminDto>> Create(SaveDailyTipDto dto) => Ok(await _tips.CreateAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<DailyTipAdminDto>> Update(int id, SaveDailyTipDto dto) => OkOrNotFound(await _tips.UpdateAsync(id, dto));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _tips.DeleteAsync(id));
}

[Route("api/tags")]
public class TagsController : ApiControllerBase
{
    private readonly ITagService _tags;

    public TagsController(ITagService tags) => _tags = tags;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<TagDto>>> GetAll() => Ok(await _tags.GetAllAsync());

    [HttpPost]
    [Authorize(Roles = AppRoles.ContentManagers)]
    public async Task<ActionResult<TagDto>> Create(CreateTagDto dto) => Ok(await _tags.CreateAsync(dto));
}

[Route("api/faq")]
public class FaqController : ApiControllerBase
{
    private readonly IFaqService _faq;

    public FaqController(IFaqService faq) => _faq = faq;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<FaqCategoryDto>>> GetAll() => Ok(await _faq.GetAllWithFaqsAsync());

    [HttpGet("categories")]
    [AllowAnonymous]
    public async Task<ActionResult<List<FaqCategoryDto>>> GetCategories() => Ok(await _faq.GetCategoriesAsync());

    [HttpPost]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<FaqDto>> Create(CreateFaqDto dto) => Ok(await _faq.CreateFaqAsync(dto));

    [HttpPut("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<ActionResult<FaqDto>> Update(int id, CreateFaqDto dto) => OkOrNotFound(await _faq.UpdateFaqAsync(id, dto));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Delete(int id) => NoContentOrNotFound(await _faq.DeleteFaqAsync(id));
}
