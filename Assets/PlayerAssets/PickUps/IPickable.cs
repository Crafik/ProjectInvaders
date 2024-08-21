public enum PlayerPickUpType{
    powerUp,
    bomb,
    extraLife
}

public interface IPickable{
    public PlayerPickUpType type { get; }

    public int GetPicked();

}