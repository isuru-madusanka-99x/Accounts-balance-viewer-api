namespace AccountsBalanceViewerAPI.Domain.Interfaces;

public interface IWithId : IWithId<Guid> { }

public interface IWithId<TId>
{
    TId Id { get; set; }
}
