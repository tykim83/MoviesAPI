using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IDataProtector protector;

        public SecurityController(IDataProtectionProvider protectionProvider)
        {
            protector = protectionProvider.CreateProtector("value_secret_and_unique");
        }

        [HttpGet]
        public IActionResult Get()
        {
            string plainText = "Fabrizio Agate";
            string encryptedText = protector.Protect(plainText);
            string decryptedText = protector.Unprotect(encryptedText);

            return Ok(new { plainText, encryptedText, decryptedText });
        }
    }
}