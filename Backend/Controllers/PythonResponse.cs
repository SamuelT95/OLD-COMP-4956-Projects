using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PythonResponse : ControllerBase
    {

        [HttpGet(Name = "GetPythonResponse")]
        public IActionResult GetPythonFromString(string pythonCode)
        {
            try
            {
                PythonRunner pyRunner = new PythonRunner();
                string output = pyRunner.RunPythonFromString(pythonCode);
                return Ok(output);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(Name = "PostPythonFromFile")]
        public async Task<IActionResult> PostPythonFromFile(List<IFormFile> files)
        {
            try
            {
                long size = files.Sum(f => f.Length);
                StringBuilder output = new StringBuilder();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = Path.GetTempFileName();
                        Console.WriteLine(filePath);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        PythonRunner pyRunner = new PythonRunner();
                        output.Append(pyRunner.RunPythonFromFile(filePath));
                    }
                }

                // Process uploaded files
                // Don't rely on or trust the FileName property without validation.
                return Ok(output.ToString());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}