namespace Runtime.Models
{
    public class ScoreModel : IScoreModel
    {
        public int Score => _score;
        private int _score;
        
        
        public void IncreaseScore(int score)
        {
            _score += score;
        }
    }
}