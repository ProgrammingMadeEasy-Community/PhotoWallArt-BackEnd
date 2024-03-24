using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Shared;

[Index(nameof(AccountId))]
[Index(nameof(CreatedBy))]
[Index(nameof(LastModifiedBy))]
[Index(nameof(DeletedBy))]
public class Entity<TEntityId> : IAuditable
{
    public TEntityId Id { set; get; } = default!;
    public Guid AccountId { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted => DeletedBy != null && DeletedOn != null;

    [NotMapped]
    private readonly List<DomainEvent> _domainEvents = null!;

    [NotMapped]
    public ICollection<DomainEvent> DomainEvents => _domainEvents;

    protected void Raise(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

}