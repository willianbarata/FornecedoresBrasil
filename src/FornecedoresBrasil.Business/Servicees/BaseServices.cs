using FluentValidation;
using FornecedoresBrasil.Business.Models;
using FornecedoresBrasil.Business.Models.Validations.Documentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FornecedoresBrasil.Business.Servicees
{
    public abstract class BaseServices
    {
        protected bool ExecutarValidacao<TV, TE>(TV validacao,TE entidade)
            where TV : AbstractValidator<TE>
            where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            //Lançamento de notificacoes

            return false;
        }
    }
}
