namespace CliqueMatchmaker;

public sealed record TicketRecord(
    string Ticket,
    string OwningSessionId,
    string PartyId,
    string[] Members,
    int CountMultiple,
    long CreatedAt,
    (int AtSec, string Query)[] QueryLadder,
    (int AtSec, int Min, int Max)[] MinMaxLadder,
    Dictionary<string, object> Properties);

public interface ITicketStore
{
    // OnAdd and OnRemove run under the matchmaker lock: buffer the change, never block or throw
    void OnAdd(TicketRecord ticket);
    void OnRemove(string ticket);

    // runs outside the lock after each sweep; must not throw, a failed write stays buffered for the next Flush
    void Flush();

    IReadOnlyList<TicketRecord> LoadAll();
}
