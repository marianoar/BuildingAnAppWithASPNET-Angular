using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController: BaseAPIController
{
    //private readonly DataContext _dbcontext;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();

        //var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username){
        // var user =_dbcontext.Users.Find(id);
        // return user;

        //var user = await _userRepository.GetUserByUsernameAsync(username);

        //return _mapper.Map<MemberDto>(user);

        return await _userRepository.GetMemberAsync(username);
    }

}