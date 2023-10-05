using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TicTacToeWeb.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrainingModelController : Controller
    {
        [HttpGet]
        [Route("ThreeByThree")]
        public string GetThreeByThree([FromQuery]string difficulty)
        {
            var temp = System.IO.File.ReadAllText(Environment.CurrentDirectory + $"/TrainedModels/ThreeByThree/{difficulty}");
            return temp;
        }

        [HttpGet]
        [Route("FourByFourWithFourWL")]
        public string GetFourByFourWithFourWL(string difficulty)
        {
            var temp = System.IO.File.ReadAllText(Environment.CurrentDirectory + $"/TrainedModels/FourByFourWithFourWL/{difficulty}");
            return temp;
        }

        [HttpGet]
        [Route("FourByFourWithThreeWL")]
        public string GetFourByFourWithThreeWL(string difficulty)
        {
            var temp = System.IO.File.ReadAllText(Environment.CurrentDirectory + $"/TrainedModels/FourByFourWithThreeWL/{difficulty}");
            return temp;
        }
    }
}
