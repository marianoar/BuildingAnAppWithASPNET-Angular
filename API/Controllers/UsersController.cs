using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController: ControllerBase
{
    private readonly DataContext _dbcontext;

    public UsersController(DataContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
        var users = await _dbcontext.Users.ToListAsync();
        return users;
    }

    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUser(int id){
        // var user =_dbcontext.Users.Find(id);
        // return user;

        return _dbcontext.Users.Find(id);
    }

}