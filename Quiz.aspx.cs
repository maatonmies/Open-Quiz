
//Get quiz questions and answers from OpenTrivia Database and pass them to the view on every page load

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public partial class Quiz : System.Web.UI.Page
{
    public class QuizObject
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string Correct_Answer { get; set; }
        public string[] Incorrect_Answers { get; set; }
    }

    public static Random Random = new Random();

    public static QuizObject quizObject = new QuizObject();

    //Backup questions in case Open Trivia Database is busy
    public static string[] BackupQuestionsPool =
    {
        "{\"response_code\":0,\"results\":[{\"category\":\"Mythology\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"Who was the only god from Greece who did not get a name change in Rome?\",\"correct_answer\":\"Apollo\",\"incorrect_answers\":[\"Demeter\",\"Zeus\",\"Athena\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"Science: Computers\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"Which company was established on April 1st, 1976 by Steve Jobs, Steve Wozniak and Ronald Wayne?\",\"correct_answer\":\"Apple\",\"incorrect_answers\":[\"Microsoft\",\"Atari\",\"Commodore\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"Sports\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"In Baseball, how many times does the ball have to be pitched outside of the strike zone before the batter is walked?\",\"correct_answer\":\"4\",\"incorrect_answers\":[\"1\",\"2\",\"3\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"Science & Nature\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"Who is the chemical element Curium named after?\",\"correct_answer\":\"Marie &amp; Pierre Curie\",\"incorrect_answers\":[\"The Curiosity Rover\",\"Curious George\",\"Stephen Curry\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"Entertainment: Music\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"When was Gangnam Style uploaded to YouTube?\",\"correct_answer\":\"2012\",\"incorrect_answers\":[\"2013\",\"2014\",\"2011\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"Science: Gadgets\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"The term &quot;battery&quot; to describe an electrical storage device was coined by?\",\"correct_answer\":\"Benjamin Franklin\",\"incorrect_answers\":[\"Nikola Tesla\",\"Luigi Galvani\",\" Alessandro Volta\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"Geography\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"What colour is the circle on the Japanese flag?\",\"correct_answer\":\"Red\",\"incorrect_answers\":[\"White\",\"Yellow\",\"Black\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"Entertainment: Television\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"In the television show Breaking Bad, what is the street name of Walter and Jesse&#039;s notorious product?\",\"correct_answer\":\"Blue Sky\",\"incorrect_answers\":[\"Baby Blue\",\"Rock Candy\",\"Pure Glass\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"History\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"How many manned moon landings have there been?\",\"correct_answer\":\"6\",\"incorrect_answers\":[\"1\",\"3\",\"7\"]}]}",
        "{\"response_code\":0,\"results\":[{\"category\":\"General Knowledge\",\"type\":\"multiple\",\"difficulty\":\"easy\",\"question\":\"What color is the &quot;Ex&quot; in FedEx Ground?\",\"correct_answer\":\"Green\",\"incorrect_answers\":[\"Red\",\"Light Blue\",\"Orange\"]}]}",
    };

    public void Page_Load(object sender, EventArgs e)
    {
        //Check progress
        if (int.Parse(Score.Value) <= 0 || int.Parse(Score.Value) > 6) return;

        //call Open Triva API
        var jsonString = GetJsonData().Result;

        //parse result to QuizObject
        DeserializeJson(jsonString);

        Start();
    }

    public static async Task<string> GetJsonData()
    {
        var openTriviaDb = new HttpClient();

        //only get selected categories 
        int[] categories = { 9, 10, 11, 12, 17, 18, 19, 20, 21, 22, 23, 24, 25, 27, 28, 30 };

        //choose a random category
        var randomCategory = categories[Random.Next(0, categories.Length - 1)].ToString();

        //call the API
        var httpResponse = openTriviaDb.GetAsync("https://opentdb.com/api.php?amount=1&type=multiple&&difficulty=easy&category=" + randomCategory).Result;


        if (!httpResponse.IsSuccessStatusCode) return BackupQuestionsPool[Random.Next(0, BackupQuestionsPool.Length - 1)];

        var json = await httpResponse.Content.ReadAsStringAsync();

        return json;
    }

    //Parse result to QuizObject
    public void DeserializeJson(string json)
    {
        var parsedJson = JObject.Parse(json);

        IList<JToken> results = parsedJson["results"].Children().ToList();

        quizObject = results[0].ToObject<QuizObject>();

        //Convert HTML-encoded characters to letters
        quizObject.Question = HttpUtility.HtmlDecode(quizObject.Question);
        quizObject.Correct_Answer = HttpUtility.HtmlDecode(quizObject.Correct_Answer);

        for (var i = 0; i < quizObject.Incorrect_Answers.Length; i++)
        {
            quizObject.Incorrect_Answers[i] = HttpUtility.HtmlDecode(quizObject.Incorrect_Answers[i]);
        }
    }

    public void Start()
    {
        //Pass the correct answer to a hidden field so it can be accessed by client scripts
        CorrectAnswerField.Value = quizObject.Correct_Answer;

        Heading.Text = quizObject.Category;

        Question.Text = quizObject.Question;

        var answers = quizObject.Incorrect_Answers.ToList();
        answers.Add(quizObject.Correct_Answer);

        ////Shuffle the order of the answers so the correct answer is not predictable
        Shuffle(answers);

        Answer1.Text = answers[0];
        Answer2.Text = answers[1];
        Answer3.Text = answers[2];
        Answer4.Text = answers[3];
    }

    public static void Shuffle<T>(IList<T> list)
    {
        var n = list.Count;

        while (n > 1)
        {
            n--;
            var k = Random.Next(n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void Page_Unload(object sender, EventArgs e)
    {
        // Wait for 800 milliseconds before reloading the page for better user experience
        System.Threading.Thread.Sleep(800);
    }

}