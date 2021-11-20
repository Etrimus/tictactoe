using TicTacToe.Dal.User;
using AutoMapper;
using TicTacToe.Domain;
using TicTacToe.Core;

namespace TicTacToe.App.User;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(UserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserModel> GetAsync(string userName, string password)
    {
        var result = await _userRepository.GetAsync(userName, password);
        if (result != null)
        {
            return _mapper.Map<UserModel>(result);
        }

        return null;
    }

    public async Task<UserModel> GetAsync(string userName, bool asNoTracking = false)
    {
        var result = await _userRepository.GetAsync(userName, asNoTracking);
        if (result != null)
        {
            return _mapper.Map<UserModel>(result);
        }

        return null;
    }

    public async Task CreateAsync(string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            throw new TicTacToeException("Имя пользователя и пароль не должны быть пустыми.");
        }

        if (await _userRepository.GetAsync(userName, false) != null)
        {
            throw new TicTacToeException("Пользователь с указанным именем уже существует.");
        }

        await _userRepository.AddAsync(new UserEntity
        {
            Name = userName.Trim(),
            Password = password.Trim()
        });
    }
}