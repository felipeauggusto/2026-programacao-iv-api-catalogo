using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umfgcloud.loja.dominio.service.DTO;

namespace umfgcloud.aplicacao.service.testes.Classes
{
    [TestClass]
    public sealed class ProdutoServicoTestes : AbstractServicoTestes
    {
        private const string C_OWNER = "Juliano Maciel";
        private const string C_CATEGORY = "produto";

        #region AdicionarAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_Sucesso()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = "123456789",
                    ValorCompra = 39.90m,
                    ValorVenda = 89.90m,
                };

                // Act
                await servico.AdicionarAsync(dto);
                var produto = (await servico.ObterTodosAsync()).FirstOrDefault();

                // Assert
                Assert.IsNotNull(produto);
                Assert.AreNotEqual(Guid.Empty, produto.Id);
                Assert.AreEqual("TESTE", produto.Descricao);
                Assert.AreEqual("123456789", produto.EAN);
                Assert.AreEqual(39.90m, produto.ValorCompra);
                Assert.AreEqual(89.90m, produto.ValorVenda);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaValorCompraNegativo()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = "123456789",
                    ValorCompra = -39.90m,
                    ValorVenda = 89.90m,
                };

                // Act & Assert
                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaValorVendaNegativo()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = "123456789",
                    ValorCompra = 39.90m,
                    ValorVenda = -89.90m,
                };

                // Act & Assert
                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaDescricaoNula()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = null!,
                    EAN = "123456789",
                    ValorCompra = 39.90m,
                    ValorVenda = 89.90m,
                };

                // Act & Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AdicionarAsync_FalhaEANNulo()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "TESTE",
                    EAN = null!,
                    ValorCompra = 39.90m,
                    ValorVenda = 89.90m,
                };

                // Act & Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => servico.AdicionarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion AdicionarAsync

        #region ObterTodosAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterTodosAsync_SucessoListaVazia()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);

                // Act
                var produtos = await servico.ObterTodosAsync();

                // Assert
                Assert.IsNotNull(produtos);
                Assert.AreEqual(0, produtos.Count());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterTodosAsync_SucessoComProdutos()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO A",
                    EAN = "111111111",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });
                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO B",
                    EAN = "222222222",
                    ValorCompra = 15.00m,
                    ValorVenda = 30.00m,
                });

                // Act
                var produtos = await servico.ObterTodosAsync();

                // Assert
                Assert.IsNotNull(produtos);
                Assert.AreEqual(2, produtos.Count());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion ObterTodosAsync

        #region ObterPorIdAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterPorIdAsync_Sucesso()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO TESTE",
                    EAN = "987654321",
                    ValorCompra = 25.00m,
                    ValorVenda = 50.00m,
                });
                var idProduto = (await servico.ObterTodosAsync()).First().Id;

                // Act
                var produto = await servico.ObterPorIdAsync(idProduto);

                // Assert
                Assert.IsNotNull(produto);
                Assert.AreEqual(idProduto, produto.Id);
                Assert.AreEqual("PRODUTO TESTE", produto.Descricao);
                Assert.AreEqual("987654321", produto.EAN);
                Assert.AreEqual(25.00m, produto.ValorCompra);
                Assert.AreEqual(50.00m, produto.ValorVenda);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_ObterPorIdAsync_FalhaIdNaoEncontrado()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                var idInexistente = Guid.NewGuid();

                // Act & Assert
                await Assert.ThrowsExceptionAsync<ApplicationException>(() => servico.ObterPorIdAsync(idInexistente));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion ObterPorIdAsync

        #region RemoverAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_RemoverAsync_Sucesso()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO REMOVER",
                    EAN = "555555555",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });
                var idProduto = (await servico.ObterTodosAsync()).First().Id;

                // Act
                await servico.RemoverAsync(idProduto);
                var produtos = await servico.ObterTodosAsync();

                // Assert
                Assert.AreEqual(0, produtos.Count());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_RemoverAsync_FalhaIdNaoEncontrado()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                var idInexistente = Guid.NewGuid();

                // Act & Assert
                await Assert.ThrowsExceptionAsync<ApplicationException>(() => servico.RemoverAsync(idInexistente));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion RemoverAsync

        #region AtualizarAsync

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_Sucesso()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO ORIGINAL",
                    EAN = "000000001",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });
                var idProduto = (await servico.ObterTodosAsync()).First().Id;
                var dtoAtualizar = new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = idProduto,
                    Descricao = "PRODUTO ATUALIZADO",
                    EAN = "000000002",
                    ValorCompra = 15.00m,
                    ValorVenda = 30.00m,
                };

                // Act
                await servico.AtualizarAsync(dtoAtualizar);
                var produto = await servico.ObterPorIdAsync(idProduto);

                // Assert
                Assert.IsNotNull(produto);
                Assert.AreEqual(idProduto, produto.Id);
                Assert.AreEqual("PRODUTO ATUALIZADO", produto.Descricao);
                Assert.AreEqual("000000002", produto.EAN);
                Assert.AreEqual(15.00m, produto.ValorCompra);
                Assert.AreEqual(30.00m, produto.ValorVenda);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_FalhaIdNaoEncontrado()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                var dto = new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = Guid.NewGuid(),
                    Descricao = "PRODUTO INEXISTENTE",
                    EAN = "999999999",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                };

                // Act & Assert
                await Assert.ThrowsExceptionAsync<ApplicationException>(() => servico.AtualizarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_FalhaValorCompraNegativo()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO",
                    EAN = "000000003",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });
                var idProduto = (await servico.ObterTodosAsync()).First().Id;
                var dto = new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = idProduto,
                    Descricao = "PRODUTO",
                    EAN = "000000003",
                    ValorCompra = -10.00m,
                    ValorVenda = 20.00m,
                };

                // Act & Assert
                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AtualizarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public async Task ProdutoServico_AtualizarAsync_FalhaValorVendaNegativo()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());
                var servico = GetProdutoServicoValidJWT(context);
                await servico.AdicionarAsync(new ProdutoDTO.ProdutoRequest()
                {
                    Descricao = "PRODUTO",
                    EAN = "000000004",
                    ValorCompra = 10.00m,
                    ValorVenda = 20.00m,
                });
                var idProduto = (await servico.ObterTodosAsync()).First().Id;
                var dto = new ProdutoDTO.ProdutoRequestWithId()
                {
                    Id = idProduto,
                    Descricao = "PRODUTO",
                    EAN = "000000004",
                    ValorCompra = 10.00m,
                    ValorVenda = -20.00m,
                };

                // Act & Assert
                await Assert.ThrowsExceptionAsync<InvalidDataException>(() => servico.AtualizarAsync(dto));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion AtualizarAsync

        #region Instanciar

        [TestMethod]
        [Owner(C_OWNER)]
        [TestCategory(C_CATEGORY)]
        public void ProdutoServico_Instanciar_Falha()
        {
            try
            {
                // Arrange
                using var context = GetSqlServerDatabaseContext(Guid.NewGuid().ToString());

                // Act & Assert
                Assert.ThrowsException<InvalidDataException>(() => GetProdutoServicoInvalidJWT(context));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        #endregion Instanciar
    }
}
