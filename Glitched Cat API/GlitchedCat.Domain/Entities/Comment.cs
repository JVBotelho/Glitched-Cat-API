using System;

namespace GlitchedCat.Domain.Entities;

public class Comment : BaseEntity
{
    public string Content { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; }
}