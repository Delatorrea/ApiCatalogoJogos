using System;

namespace ApiCatalogoJogos.Exceptions
{
    public class GameNotRegisteredException : Exception
    {
        public GameNotRegisteredException() 
            : base("Este jogo não está cadastrado")
        { }
    }
}
