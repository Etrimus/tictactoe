using TicTacToe.Dal.User;
using AutoMapper;

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

    public async Task<UserModel> GetAsync(string userName)
    {
        var result = await _userRepository.GetAsync(userName);
        if (result != null)
        {
            return _mapper.Map<UserModel>(result);
        }

        return null;
    }
}