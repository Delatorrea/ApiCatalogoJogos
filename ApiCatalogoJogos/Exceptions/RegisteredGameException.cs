using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Exceptions
{
    public class RegisteredGameException : Exception
    {
        public RegisteredGameException() 
            : base("Este jogo já está cadastrado")
        { }
    }
}
