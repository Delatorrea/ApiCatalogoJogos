using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogos.InputModel
{
    public class GameInputModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage ="O nome do jogo deve conter entre 3 e 100 caracteres")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome da produtora deve conter entre 3 e 100 caracteres")]
        public string Producer { get; set; }

        [Required]
        [Range(1,1000, ErrorMessage = "O preço deve ser no mínimo 1 real e no máximo 1.000 reais")]
        public double Price { get; set; }
    }
}
