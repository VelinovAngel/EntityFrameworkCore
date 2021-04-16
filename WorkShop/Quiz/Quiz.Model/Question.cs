using System.Collections.Generic;

namespace Quiz.Models
{
    public class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }
        public int Id { get; set; }

        public string Titile { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}