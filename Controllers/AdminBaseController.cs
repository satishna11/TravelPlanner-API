using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TravelAI.Controllers;
[Authorize(Roles="Admin")]
[ApiController]
public abstract class AdminBaseController:BaseController
{
    
}