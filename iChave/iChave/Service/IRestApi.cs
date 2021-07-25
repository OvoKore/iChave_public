using System.Threading.Tasks;
using iChave.ModelDTO;
using iChave.Model;
using Refit;
using System.Collections;
using System.Collections.Generic;

namespace iChave.Service
{
    public interface IRestApi
    {
        //CEP
        [Get("/{cep}/json")]
        Task<CepDTO> Cep(string cep);

        //ACCESS
        [Post("/update-fcm")]
        Task<UpdateFcmDTO> UpdateFCM([Body(BodySerializationMethod.Serialized)] UpdateFcmDTO updateFcm);
        [Post("/send-password-reset")]
        Task<Msg> SendPasswordReset([Body(BodySerializationMethod.Serialized)] IDictionary email);
        [Post("/logout")]
        Task<Msg> Logout([Body(BodySerializationMethod.Serialized)] LogoutDTO logout);


        //USER
        [Post("/create-user")]
        Task<Msg> CreateUser([Body(BodySerializationMethod.Serialized)] UserDTO user);

        [Post("/update-user")]
        Task<Msg> UpdateUser([Body(BodySerializationMethod.Serialized)] UserDTO user);

        [Get("/get-user/{email}")]
        Task<UserDTO> GetUser(string email);

        [Get("/get-user-with-address/{cpf}")]
        Task<UserWithAddressDTO> GetUserWithAddres(string cpf);


        //LOCKSMITH
        [Post("/create-locksmith")]
        Task<Msg> CreateLocksmith([Body(BodySerializationMethod.Serialized)] LocksmithDTO locksmith);

        [Post("/update-locksmith")]
        Task<Msg> UpdateLocksmith([Body(BodySerializationMethod.Serialized)] LocksmithDTO locksmith);

        [Get("/get-locksmith-list")]
        Task<List<LocksmithListDTO>> GetLocksmithList();

        [Get("/get-locksmith-email/{email}")]
        Task<LocksmithDTO> GetLocksmithByEmail(string email);

        [Get("/get-locksmith-list-cnpj/{cnpj}")]
        Task<LocksmithListDTO> GetLocksmithListByCnpj(string cnpj);


        //ADDRESS
        [Post("/set-address/{role}/{email}")]
        Task<Msg> SetAddress(string role, string email, [Body(BodySerializationMethod.Serialized)] AddressDTO address);

        [Get("/get-address/{role}/{email}")]
        Task<AddressDTO> GetAddress(string role, string email);


        //CHAT
        [Get("/get-chat-list/{chat.role}/{chat.my_email}")]
        Task<List<ChatDTO>> GetChatList(ChatDTO chat);

        [Post("/send-chat")]
        Task<Msg> SendChat([Body(BodySerializationMethod.Serialized)] ChatDTO chat);

        //SERVICE
        [Get("/get-service-list/{email}")]
        Task<List<ServiceDTO>> GetLocksmithServices(string email);

        [Post("/add-service/{email}")]
        Task<Msg> AddService(string email, [Body(BodySerializationMethod.Serialized)] ServiceDTO service);

        [Post("/update-service")]
        Task<Msg> UpdateService([Body(BodySerializationMethod.Serialized)] ServiceDTO service);

        [Post("/delete-service")]
        Task<Msg> DeleteService([Body(BodySerializationMethod.Serialized)] ServiceDTO service);
    }
}