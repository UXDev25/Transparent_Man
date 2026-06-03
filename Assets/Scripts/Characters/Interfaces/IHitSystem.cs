using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public interface IHitSystem
{
    bool IsAttacking { get; set; }
    public abstract void ShowHitBox(int hitboxNumber);

    public abstract void HideHitBox();
}
