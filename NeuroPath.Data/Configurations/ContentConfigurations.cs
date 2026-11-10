using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeuroPath.Core.Models.Domain.Content;

namespace NeuroPath.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasOne(a => a.Author).WithMany(u => u.AuthoredArticles).HasForeignKey(a => a.AuthorUserId);
        builder.Property(a => a.Title).HasMaxLength(200);
        builder.Property(a => a.Summary).HasMaxLength(500);
        builder.Property(a => a.ImageUrl).HasMaxLength(500);
        builder.HasIndex(a => new { a.IsPublished, a.PublishedAt });
    }
}

public class ArticleCategoryConfiguration : IEntityTypeConfiguration<ArticleCategory>
{
    public void Configure(EntityTypeBuilder<ArticleCategory> builder)
    {
        builder.Property(c => c.Name).HasMaxLength(100);
        builder.HasIndex(c => c.Name).IsUnique();
    }
}

public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTag>
{
    public void Configure(EntityTypeBuilder<ArticleTag> builder)
    {
        builder.HasKey(x => new { x.ArticleId, x.TagId });
    }
}

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.Property(t => t.Name).HasMaxLength(50);
        builder.HasIndex(t => t.Name).IsUnique();
    }
}

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasOne(c => c.ParentComment).WithMany(c => c.Replies).HasForeignKey(c => c.ParentCommentId).OnDelete(DeleteBehavior.Restrict);
        builder.Property(c => c.Content).HasMaxLength(2000);
        builder.HasIndex(c => new { c.ArticleId, c.ParentCommentId });
    }
}

public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
    public void Configure(EntityTypeBuilder<CommentLike> builder)
    {
        builder.HasIndex(x => new { x.CommentId, x.UserId }).IsUnique();
    }
}

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.Property(b => b.Note).HasMaxLength(500);
        builder.HasIndex(b => new { b.UserId, b.BookmarkType, b.ReferenceId }).IsUnique();
    }
}

public class DailyTipConfiguration : IEntityTypeConfiguration<DailyTip>
{
    public void Configure(EntityTypeBuilder<DailyTip> builder)
    {
        builder.Property(t => t.Title).HasMaxLength(200);
        builder.Property(t => t.Category).HasMaxLength(50);
    }
}

public class ResearchStudyConfiguration : IEntityTypeConfiguration<ResearchStudy>
{
    public void Configure(EntityTypeBuilder<ResearchStudy> builder)
    {
        builder.Property(r => r.Title).HasMaxLength(300);
        builder.Property(r => r.Doi).HasMaxLength(100);
        builder.Property(r => r.Journal).HasMaxLength(200);
    }
}

public class ResearchStudyBrainAreaConfiguration : IEntityTypeConfiguration<ResearchStudyBrainArea>
{
    public void Configure(EntityTypeBuilder<ResearchStudyBrainArea> builder)
    {
        builder.HasKey(x => new { x.ResearchStudyId, x.BrainAreaId });
    }
}
