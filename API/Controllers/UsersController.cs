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
            //return _mapper.Map<PhotoDto>(photo);
            return CreatedAtAction(nameof(GetUser), new { username = user.UserName}, _mapper.Map<PhotoDto>(photo)); //routeValue
        }

        return BadRequest("problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
        
        if (user == null) return NotFound();

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        
        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        
        if (currentMain != null) currentMain.IsMain = false;
        
        photo.IsMain = true;

        if(await _userRepository.SaveAllAsync() ) return NoContent();

        return BadRequest("Problems setting the main photo");
    }

    [HttpDelete("delete-photo/{photoIs}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());

        if (user == null) return NotFound();

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("This is your main photo");

        if(photo.PublicId != null)
        {
            var result = await _photoService.DeletoPhotoAsync(photo.PublicId);
            if(result.Error!= null) { return BadRequest(result.Error.Message); }
        }

        user.Photos.Remove(photo);

        if(await _userRepository.SaveAllAsync() ) { return Ok(); }

        return BadRequest("problem deleting photo");
    }

}