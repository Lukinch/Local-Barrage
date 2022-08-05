
public interface IDamageCollision
{
    int OwnerInputId { get; }
    void TakeProjectileDamage(float damage, int ownerIndex);
    void TakePickableDamage(float damage);
}
