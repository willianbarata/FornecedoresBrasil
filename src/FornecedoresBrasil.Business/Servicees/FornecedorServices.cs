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

        public FornecedorServices(IFornecedorRepository fornecedorRepository,
                              INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor)
                || !ExecutarValidacao(new EnderecoValidation(), fornecedor.Endereco)) return;

            //Validar se já não existe outro fornecedor com o mesmo doc.
            if(_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado.");
                return;
            }

            await _fornecedorRepository.Adicionar(fornecedor);
        }
        public async Task Atualizar(Fornecedor fornecedor)
        {
            if(!ExecutarValidacao(new FornecedorValidation(), fornecedor)) return;

            if(_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado.");
                return;
            }

            await _fornecedorRepository.Atualizar(fornecedor);
        }
        public async Task Remover(Guid id)
        {
            var fornecedor = await _fornecedorRepository.ObterFornecedorProdutosEndereco(id);

            if(fornecedor == null)
            {
                Notificar("Fornecedor não existe");
                return;
            }

            if(fornecedor.produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return;
            }

            var endereco = await _fornecedorRepository.ObterEnderecoPorFornecedor(id);

            if(endereco != null)
            {
                await _fornecedorRepository.RemoverEnderecoFornecedor(endereco);
            }

            await _fornecedorRepository.Remover(id);
        }
        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
        }

    }
}
