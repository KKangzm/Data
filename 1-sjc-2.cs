// CharacterSwitcher.cs
public class CharacterSwitcher : MonoBehaviour
{
    public GameObject[] characters;
    private int currentIndex = 0;

    void Start() {
        ActivateCharacter(currentIndex);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)){
            SwitchNextCharacter();
        }
    }

    void SwitchNextCharacter(){
        characters[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % characters.Length;
        ActivateCharacter(currentIndex);
    }

    void ActivateCharacter(int index){
        characters[index].SetActive(true);
        Camera.main.GetComponent<FollowCamera>().target = 
            characters[index].transform;
    }
}