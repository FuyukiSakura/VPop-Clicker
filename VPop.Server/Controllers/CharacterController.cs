using Microsoft.AspNetCore.Mvc;
using VPop.Data;
using VPop.Server.Services.Database;

namespace VPop.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CharacterController(CharacterDbService characterDbService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddCharacter([FromBody] Character character)
        {
            await characterDbService.AddCharacterAsync(character);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter(string id)
        {
            await characterDbService.DeleteCharacterAsync(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacter(string id)
        {
            var character = await characterDbService.GetCharacterAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            return character;
        }

        [HttpGet]
        public async Task<IEnumerable<Character>> GetCharacters()
        {
            return await characterDbService.GetCharactersAsync("SELECT * FROM c");
        }
    }
}
