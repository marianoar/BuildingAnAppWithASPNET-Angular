using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
public class UsersController: BaseAPIController
{
    //private readonly DataContext _dbcontext;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
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

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        //var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());

        if(user == null)
            return NotFound();
        
        _mapper.Map(memberUpdateDto, user);

        if(await _userRepository.SaveAllAsync()) 
            return NoContent();

        return BadRequest("No se pudo actualizar el user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
        
        if (user == null) return NotFound();
        
        var result = await _photoService.AddPhotoAsync(file);
        
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
        };

        if(user.Photos.Count()==0) photo.IsMain= true;

        user.Photos.Add(photo);

        if(await _userRepository.SaveAllAsync())
        {
            return _mapper.Map<PhotoDto>(photo);
        }

        return BadRequest("problem adding photo");


    }

}