using System;

namespace ApiCatalogoJogos.Exceptions
{
    public class RegisteredGameException : Exception
    {
        public RegisteredGameException() 
            : base("Este jogo já está cadastrado")
        { }
    }
}
