// 技能树控制器
public class SkillTree : MonoBehaviour {
    [SerializeField] private SkillNode rootNode;
    private Dictionary<SkillType, bool> unlockedSkills = new Dictionary<SkillType, bool>();
    
    public void UnlockSkill(SkillType skill){
        if(CanUnlock(skill)){
            unlockedSkills[skill] = true;
            ApplySkillEffect(skill);
        }
    }

    bool CanUnlock(SkillType skill){
        return CheckPrerequisites(skill) && 
               HasEnoughResources(skill) &&
               MeetLevelRequirement(skill);
    }
}

// 道具合成系统
public class CraftingSystem : MonoBehaviour {
    public bool TryCraftItem(Recipe recipe){
        if(Inventory.HasIngredients(recipe.requiredItems)){
            foreach(var ingredient in recipe.requiredItems){
                Inventory.RemoveItem(ingredient);
            }
            Inventory.AddItem(recipe.resultItem);
            return true;
        }
        return false;
    }
}