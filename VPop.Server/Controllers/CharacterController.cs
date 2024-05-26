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

        [HttpPut("{id}/click")]
        public async Task<IActionResult> UpdateClickCount(string id, [FromBody] int clickCount)
        {
            // Limit the number of clicks that can be registered per second
            if (clickCount > 20)
            {
                return BadRequest("Too many clicks per second");
            }

            var character = await characterDbService.GetCharacterAsync(id);
            if (character == null)
            {
                return NotFound();
            }

            character.ClickCount += clickCount;
            await characterDbService.UpdateCharacterAsync(character);

            return Ok();
        }
    }
}
