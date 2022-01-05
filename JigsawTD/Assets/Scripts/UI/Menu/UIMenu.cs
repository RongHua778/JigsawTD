using UnityEngine;

public class UIMenu : IUserInterface
{
    Animator anim;
    [SerializeField] GameObject m_ContinueGameBtn = default;

    public override void Initialize()
    {
        base.Initialize();
        anim = this.GetComponent<Animator>();
        m_ContinueGameBtn.SetActive(LevelManager.Instance.LastGameSave.HasLastGame);
    }

    public override void Show()
    {
        base.Show();
        anim.SetBool("Show", true);
    }

    public override void ClosePanel()
    {
        base.ClosePanel();
        anim.SetBool("Show", false);

    }

    public void ContinueBtnClick()
    {
        MenuManager.Instance.ContinueGame();
    }

    public void StartGameBtnClick()
    {
        MenuManager.Instance.StartGame();

    }

    public void TujianBtnClick()
    {
        MenuManager.Instance.OpenTujian();
    }

    public void SettingBtnClick()
    {
        MenuManager.Instance.OpenSetting();
    }

    public void WishListBtnClick()
    {
        Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");

    }

    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }
}
