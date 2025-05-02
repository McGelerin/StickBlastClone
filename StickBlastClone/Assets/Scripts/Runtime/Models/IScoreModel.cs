namespace Runtime.Models
{
    public interface IScoreModel
    {
        public int Score { get; }

        public void IncreaseScore(int score);
    }
}