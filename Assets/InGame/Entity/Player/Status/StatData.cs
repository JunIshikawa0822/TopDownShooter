public class StatsData
{
    //----------------------------------------------------------------
    //加算式のパラメーターはここです
    //----------------------------------------------------------------
    public float MaxHP = 0f;
    public float MaxStamina = 0f;

    //回復＆減少率
    public float HPRegenValue = 0f;
    public float BleedValue = 0f;
    public float StaminaRegenValue = 0f;

    //----------------------------------------------------------------
    //乗算のパラメーターはここです（〜Multiplierとか）
    //----------------------------------------------------------------

    //今後、他のステータスがここに追加されるかも

    //他のStatsDataと合算し、自身を更新するメソッド
    public void Add(StatsData other)
    {
        this.MaxHP += other.MaxHP;
        this.MaxStamina += other.MaxStamina;
        this.HPRegenValue += other.HPRegenValue;
        this.BleedValue += other.BleedValue;
        this.StaminaRegenValue += other.StaminaRegenValue;
    }

    //全ての値を初期化するメソッド
    public void Reset()
    {
        this.MaxHP = this.MaxStamina = 0f;
        this.HPRegenValue = this.BleedValue = this.StaminaRegenValue = 0f;

        //〜Multiplierはここで1.0fに初期化
    }
}
