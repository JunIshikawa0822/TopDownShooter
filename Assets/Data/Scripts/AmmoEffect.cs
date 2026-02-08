[System.Serializable]
public class Effect
{
    public ElementType type;//Ignite, Electric, Freeze
    public float magnitude;//「効果の強さ」。ダメージにするか時間は受け手次第
    public float chance;//発生確率
}