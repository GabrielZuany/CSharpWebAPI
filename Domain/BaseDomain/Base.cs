namespace Domain.BaseDomain;

public class Base
{
    public Guid Id { get; protected set; }
    public DateTime Created { get; protected set; }
    public DateTime? Updated { get; protected set; }

    public void Update()
    {
        Updated = DateTime.Now;
    }
}