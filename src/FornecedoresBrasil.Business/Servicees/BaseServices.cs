﻿using FluentValidation;
using FluentValidation.Results;
using FornecedoresBrasil.Business.Interfaces;
using FornecedoresBrasil.Business.Models;
using FornecedoresBrasil.Business.Models.Validations.Documentos;
using FornecedoresBrasil.Business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FornecedoresBrasil.Business.Servicees
{
    public abstract class BaseServices
    {
        private readonly INotificador _notificador;

        protected BaseServices(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var item in validationResult.Errors)
            {
                Notificar(item.ErrorMessage);
            }
        }
        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificacao(mensagem));
        }
        protected bool ExecutarValidacao<TV, TE>(TV validacao,TE entidade)
            where TV : AbstractValidator<TE>
            where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notificar(validator);

            return false;
        }
    }
}
