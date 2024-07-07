using FornecedoresBrasil.Business.Interfaces;
using FornecedoresBrasil.Business.Models;
using FornecedoresBrasil.Business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FornecedoresBrasil.Business.Servicees
{
    public class FornecedorServices : BaseServices, IFornecedorServices
    {
        private readonly IFornecedorRepository _fornecedorRepository;

        public FornecedorServices(IFornecedorRepository fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)
                || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            //Validar se já não existe outro fornecedor com o mesmo doc.


            await _fornecedorRepository.Adicionar(fornecedor);
        }
        public async Task Atualizar(Fornecedor fornecedor)
        {
            if(!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

            await _fornecedorRepository.Atualizar(fornecedor);
        }
        public async Task Remover(Guid id)
        {
            await _fornecedorRepository.Remover(id);
        }
        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
        }

    }
}
