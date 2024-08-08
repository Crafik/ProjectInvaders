public interface IPickable{
    /// <summary>
    /// Type of pickable:
    /// 0 - power up;
    /// 1 - extra life;
    /// 2 - ???
    /// </summary>
    public int type { get; }

    public int GetPicked();

}