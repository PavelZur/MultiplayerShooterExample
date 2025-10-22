
public interface IWeapon
{
    void Shoot();          
    void Reload();         
    void Show();         
    void Hide();   

    bool IsReloading { get; }
}