using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController: BaseAPIController
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
    public async Task<ActionResult<AppUser>> GetUser(int id){
        // var user =_dbcontext.Users.Find(id);
        // return user;

        return await _dbcontext.Users.FindAsync(id);
    }

}